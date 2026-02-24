# 📦 OrderFinanceControl

Projeto backend desenvolvido em **.NET** para controle de pedidos financeiros, utilizando duas abordagens de persistência:

* [**SQL Server (Entity Framework Core)**](https://github.com/JoaoVitorAguiar/order-finance-control) → modelo relacional tradicional
* [**MongoDB**](https://github.com/JoaoVitorAguiar/order-finance-control/tree/mongo-db) → modelo orientado a documentos

O objetivo foi comparar abordagens de banco relacional e NoSQL em um mesmo domínio.


## 🔵 SQL Server

Utilizado com **Entity Framework Core**:

* Estrutura relacional normalizada
* Uso de chaves estrangeiras
* Integridade referencial

**Principais entidades:**

* Customer
* Product
* Order
* OrderItem

### 📊 Diagrama Entidade-Relacionamento (DER)
<p align="center">
  <img src="./docs/der-sql.png" width="500"/>
</p>

### 🔎 Exemplo de consulta com Entity Framework Core

Em um banco relacional, mesmo uma consulta aparentemente simples pode envolver múltiplos relacionamentos.

No exemplo abaixo, ao buscar um pedido completo com cliente e itens, o Entity Framework Core precisa resolver internamente **joins entre várias tabelas**: `Orders`, `Customers`, `OrderItems` e `Products`.

```csharp
public async Task<OrderResponseDto?> GetByIdAsync(int id)
{
    return await _dbContext.Orders
        .AsNoTracking()
        .Select(o => new OrderResponseDto
        {
            Id = o.Id,

            Customer = new CustomerResponseDto
            {
                Id = o.Customer.Id,
                Name = o.Customer.Name
            },

            CreatedAt = o.CreatedAt,
            TotalAmount = o.TotalAmount,
            Status = o.Status.ToString(),

            Items = o.Items.Select(i => new OrderItemResponseDto
            {
                ProductId = i.ProductId,
                ProductName = i.Product.Name,
                Quantity = i.Quantity,
                UnitPriceAtOrderTime = i.UnitPriceAtOrderTime
            }).ToList()
        })
        .FirstOrDefaultAsync(o => o.Id == id);
}
```

Apesar de o código em C# parecer simples e expressivo, o EF Core traduz essa expressão LINQ para uma consulta SQL semelhante a:

```sql
SELECT o.Id,
       o.CreatedAt,
       o.TotalAmount,
       o.Status,
       c.Id AS CustomerId,
       c.Name AS CustomerName,
       i.ProductId,
       p.Name AS ProductName,
       i.Quantity,
       i.UnitPriceAtOrderTime
FROM Orders o
INNER JOIN Customers c ON o.CustomerId = c.Id
LEFT JOIN OrderItems i ON i.OrderId = o.Id
LEFT JOIN Products p ON i.ProductId = p.Id
WHERE o.Id = @id;
```

Ou seja, para montar um único objeto `OrderResponseDto`, o banco precisa:

* Consultar a tabela `Orders`
* Realizar join com `Customers`
* Realizar join com `OrderItems`
* Realizar join com `Products`

Essa abordagem garante **forte consistência e integridade referencial**, mas pode envolver consultas mais complexas e múltiplos joins para recuperar dados relacionados.


## 🟢 MongoDB

O **MongoDB** foi utilizado como banco NoSQL orientado a documentos, onde os dados são armazenados em formato **BSON (Binary JSON)**. Diferente de bancos relacionais, ele permite uma estrutura mais flexível e otimizada para leitura de objetos completos.

Principais características dessa abordagem:

* Dados armazenados em formato JSON/BSON
* Estrutura flexível (schema dinâmico)
* Alta performance para leitura de documentos
* Possibilidade de documentos embutidos (*embedding*) ou referências (*referencing*)


### Documentos embutidos (Embedding)

Consiste em armazenar dados relacionados dentro do próprio documento.
Isso reduz a necessidade de múltiplas consultas e melhora a performance de leitura.

Exemplo utilizado no projeto:

```json
{
  "_id": "ObjectId",
  "customer": {
    "_id": "ObjectId",
    "name": "string",
    "email": "string"
  },
  "items": [
    {
      "productId": "string",
      "productName": "string",
      "unitPrice": "decimal",
      "quantity": "int"
    }
  ],
  "createdAt": "datetime",
  "status": "string",
  "paidAt": "datetime?"
}
```

Nesse caso:

* Os dados do **customer** estão embutidos no pedido
* Os **items** também fazem parte do mesmo documento
* O nome e preço do produto são armazenados como *snapshot* no momento da compra

Essa estratégia é útil quando:

* Os dados são frequentemente consultados juntos
* A performance de leitura é prioridade
* Queremos preservar histórico (ex.: preço do produto na data da compra)


### Referência de documentos (Referencing)

Outra abordagem seria armazenar apenas o ID dos dados relacionados:

```json
{
  "customerId": "ObjectId",
  "productIds": ["ObjectId"]
}
```

Nesse caso:

* Os dados completos ficam em outras collections
* Pode ser necessário fazer múltiplas consultas ou usar agregações (`$lookup`)

Essa estratégia é indicada quando:

* Os dados são muito grandes ou frequentemente atualizados
* Há reutilização intensa entre documentos
* A normalização é desejada


### Escolha da abordagem

Não existe uma única abordagem correta.
A escolha entre **embedding** e **referencing** depende principalmente de:

* Volume e frequência de leitura
* Necessidade de consistência dos dados
* Complexidade das consultas
* Evolução do domínio do sistema

No contexto deste projeto, optou-se por **embedding** para os pedidos, visando:

* Melhor performance de leitura
* Simplicidade nas consultas
* Preservação do histórico dos dados relacionados



## ⚖️ Comparativo SQL Server vs MongoDB

| Característica      | SQL Server           | MongoDB                        |
| ------------------- | -------------------- | ------------------------------ |
| Tipo de banco       | Relacional           | Documento (NoSQL)              |
| Schema              | Fixo                 | Flexível                       |
| Relacionamentos     | FK / JOIN            | Embed ou referência            |
| Performance leitura | Boa com joins        | Muito rápida sem joins         |
| Escalabilidade      | Vertical             | Horizontal                     |
| Evolução do modelo  | Migration necessária | Alteração simples              |


## 🧠 Quando usar cada um

### SQL recomendado quando:

* Forte integridade de dados
* Regras financeiras complexas
* Relatórios e consultas analíticas

### MongoDB recomendado quando:

* Alta escala
* Leitura rápida de documentos completos
* Modelo flexível



## 🧩 Aderência do Domínio ao Banco

No MongoDB, a entidade pode ser representada quase exatamente como foi modelada no domínio:

```csharp
public class Order(Customer customer, IEnumerable<OrderItem> items)
{
    public string Id { get; set; }
    public Customer Customer { get; set; } = customer;
    public List<OrderItem> Items { get; set; } = items.ToList();
    public decimal TotalAmount => Items.Sum(i => i.Quantity * i.UnitPrice);
}
```

O pedido já contém o cliente e os itens, mantendo a estrutura natural do objeto.

No SQL Server, a modelagem relacional exige algumas adaptações, como a inclusão de chave estrangeira:

```csharp
public class Order
{
    private Order() { }
    public Order(int customerId, ICollection<OrderItem> items)
    {
        CustomerId = customerId;
        Items = items;
        TotalAmount = items.Sum(i => i.Quantity * i.UnitPriceAtOrderTime);
    }

    public int Id { get; set; }
    public int CustomerId { get; set; }
    public Customer Customer { get; set; } 
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public decimal TotalAmount { get; set; }
    public OrderStatus Status { get; set; } = OrderStatus.Created;
    public DateTime? PaidAt { get; set; }
    public ICollection<OrderItem> Items { get; set; }
}
```

A propriedade `CustomerId` existe por exigência do modelo relacional, não do domínio.

Assim, o MongoDB permite maior proximidade com o modelo orientado a objetos, enquanto o SQL Server exige adaptação para manter integridade referencial.



