using OrderFinanceControl.Data.Repositories.Interfaces;
using OrderFinanceControl.Entities;

namespace OrderFinanceControl.UseCases.Customers;

public class GetCustomersUseCase(ICustomerRepository customerRepository)
{
    private readonly ICustomerRepository _customerRepository = customerRepository;

    public async Task<IEnumerable<Customer>> ExecuteAsync()
    {
        return await _customerRepository.GetAllAsync();
    }
}
