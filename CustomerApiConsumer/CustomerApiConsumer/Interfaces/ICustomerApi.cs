using CustomerApiConsumer.Models;
using Refit;

namespace CustomerApiConsumer.Interfaces
{
    public interface ICustomerApi
    {
        [Get("/Customer/GetCustomers")]
        Task<List<CustomerDto>?> GetAllCustomer();
        [Get("/Customer/GetCustomerById/{id}")]
        Task<CustomerDto?> GetCustomerById(Guid id);
        [Post("/Customer/CreateCustomer")]
        Task<CustomerDto?> CreateCustomer(CustomerDto item);
        [Put("/Customer/UpdateCustomer")]
        Task<CustomerDto?> UpdateCustomer(CustomerDto item);
        [Delete("/Customer/UpdateCustomer/{id}")]
        Task<CustomerDto?> DeleteCustomer(Guid id);
    }
}
