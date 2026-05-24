using OrderFinanceControl.Common;
using OrderFinanceControl.Common.Errors;
using OrderFinanceControl.Data.Repositories.Interfaces;
using OrderFinanceControl.Dtos.Customers;
using OrderFinanceControl.Entities;

namespace OrderFinanceControl.UseCases.Customers;

public class CreateCustomerUseCase(ICustomerRepository customerRepository)
{
    private readonly ICustomerRepository _customerRepository = customerRepository;
    public async Task<Result<Customer>> ExecuteAsync(CustomerDto customerDto)
    {
        var existingCustomer = await _customerRepository.GetByEmailAsync(customerDto.Email);
        if (existingCustomer != null)
        {
            return CustomerErrors.EmailAlreadyExists;
        }

        var customer = new Customer(customerDto.Name, customerDto.Email);

        await _customerRepository.AddAsync(customer);

        return customer;
    }
}
