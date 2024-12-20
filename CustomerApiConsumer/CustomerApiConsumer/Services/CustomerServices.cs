using CustomerApiConsumer.Interfaces;
using CustomerApiConsumer.Models;
using Refit;


namespace CustomerApiConsumer.Services
{
    public class CustomerServices
    {
        #region Private Attributes
        private readonly ICustomerApi _customerApi;
        #endregion


        public CustomerServices(string baseUrl)
        {
            _customerApi = RestService.For<ICustomerApi>(baseUrl);
        }

        public async Task<List<CustomerDto>?> GetAllCustomers()
        {
            try
            {
                return await _customerApi.GetAllCustomer();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"CustomerServices-GetAllCustomers-raised exception: {ex.Message}");
                return new List<CustomerDto>();
            }
        }

        public async Task<CustomerDto?> GetCustomerById(Guid id)
        {
            try
            {
                return await _customerApi.GetCustomerById(id);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"CustomerServices-GetCustomerById-raised exception: {ex.Message}");
                return null;
            }
        }
        public async Task<CustomerDto?> CreateCustomer(CustomerDto customer)
        {
            try
            {
                var createdCustomer = await _customerApi.CreateCustomer(customer);
                Console.WriteLine($"Created Customer: ID={createdCustomer!.id}, LastName={createdCustomer!.lastname}, FirstName={createdCustomer!.firstname}");
                return createdCustomer;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"CustomerServices-CreateCustomerById-raised exception: {ex.Message}");
            }
            return null;
        }

        public async Task<CustomerDto?> UpdateCustomer(CustomerDto customer)
        {
            try
            {
              var updatedCustomer=  await _customerApi.UpdateCustomer(customer);
                Console.WriteLine($"Created Customer: ID={updatedCustomer!.id}, LastName={updatedCustomer!.lastname}, FirstName={updatedCustomer!.firstname}.");
               return updatedCustomer;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"CustomerServices-UpdateCustomer-raised exception: {ex.Message}");
                return null;
            }
        }

        public async Task<CustomerDto?> DeleteCustomer(Guid id)
        {
            try
            {
              var item=  await _customerApi.DeleteCustomer(id);
                Console.WriteLine($"Deleted Customer: ID={item!.id}, LastName={item!.lastname}, FirstName={item!.firstname}.");
                return item;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"CustomerServices-DeleteCustomer-raised exception: {ex.Message}");
                return null;
            }
        }
    }
}
