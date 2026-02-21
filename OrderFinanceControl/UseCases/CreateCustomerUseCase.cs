using OrderFinanceControl.Data.Repositories.Interfaces;
using OrderFinanceControl.Dtos.Customers;
using OrderFinanceControl.Entities;
using OrderFinanceControl.Exceptions;

namespace OrderFinanceControl.UseCases;

public class CreateCustomerUseCase(ICustomerRepository customerRepository)
{
    private readonly ICustomerRepository _customerRepository = customerRepository;
    public async Task ExecuteAsync(CustomerDto customerDto)
    {
        var existingCustomer = await _customerRepository.GetByEmailAsync(customerDto.Email);
        if (existingCustomer != null)
        {
            throw new AlreadyExistsException("A customer with the same email already exists.");
        }

        var customer = new Customer(customerDto.Name, customerDto.Email);

        await _customerRepository.AddAsync(customer);
        await _customerRepository.SaveChangesAsync();
    }
}
