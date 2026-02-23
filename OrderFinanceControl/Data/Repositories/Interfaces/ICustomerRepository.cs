using OrderFinanceControl.Entities;

namespace OrderFinanceControl.Data.Repositories.Interfaces;

public interface ICustomerRepository
{
    Task<IEnumerable<Customer>> GetAllAsync();
    Task<Customer?> GetByEmailAsync(string email);
    Task<Customer?> GetByIdAsync(string id);
    Task AddAsync(Customer customer);
}