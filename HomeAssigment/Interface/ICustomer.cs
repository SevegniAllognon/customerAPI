using HomeAssigment.Models;

namespace HomeAssigment.Interface
{
    public interface ICustomer
    {
        Task<List<Customer>?> GetAllCustomer();
        Task<Customer?> GetCustomerById(Guid id);
        Task<Customer?> CreateCustomer(Customer item);
        Task<Customer?> UpdateCustomer(Customer item);
        Task<Customer?> DeleteCustomer(Guid id);
    }
}
