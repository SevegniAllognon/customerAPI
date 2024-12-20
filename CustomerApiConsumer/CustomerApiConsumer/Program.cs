using CustomerApiConsumer.Models;
using CustomerApiConsumer.Services;
using Microsoft.Extensions.Configuration;


namespace CustomerApiConsumer
{
    public class Program
    {
        static async Task Main(string[] args)
        {
            // Load configuration
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();

            var baseUrl = configuration["ApiSettings:BaseUrl"];
            var customerService = new CustomerServices(baseUrl!);

            Console.WriteLine("Customer API Consumer");
            Console.WriteLine("----------------------");

            await GetAllCustomers(customerService);
            await GetCustomerById(customerService);
            await CreateCustomer(customerService);
            await UpdateCustomer(customerService);
            await DeleteCustomer(customerService);
        }

        static async Task GetAllCustomers(CustomerServices customerService)
        {
            Console.WriteLine("\nGet all customers...");
            var customers = await customerService.GetAllCustomers();

            foreach (CustomerDto customer in customers!)
            {
                Console.WriteLine($"GetAllCustomers:\nID: {customer.id}, FullName: {customer.firstname}-{customer.lastname}, Email: {customer.emailaddress}");
            }
        }

        static async Task GetCustomerById(CustomerServices customerService)
        {
            Console.WriteLine("\nGet customer by Id...");
            var customer = await customerService.GetCustomerById(Guid.Parse("0193bc5d-d620-7f7c-b8b4-ca7e1d1f4c1f"));
           Console.WriteLine($"GetCustomerById:\nID: {customer!.id}, FullName: {customer.firstname}-{customer.lastname}, Email: {customer.emailaddress}");
        }

        static async Task CreateCustomer(CustomerServices customerService)
        {
            Console.WriteLine("\nAdding a new customer...");
            var newCustomer = new CustomerDto
            {
                id = Guid.NewGuid(),
                firstname = "Noe",
                middlename = "Rich",
                lastname = "Bmw",
                emailaddress = "noe.mw@yahoo.com",
                phonenumber = "4-510-777-8963",
                creationdate = DateTime.Now,
                updateddate = null
            };

            var customerAdded= await customerService.CreateCustomer(newCustomer);
            Console.WriteLine($"CreateCustomer:\nID: {customerAdded!.id}, FullName: {customerAdded.firstname}-{customerAdded.lastname}, Email: {customerAdded.emailaddress}");
        }

        static async Task UpdateCustomer(CustomerServices customerService)
        {
            Console.WriteLine("\nUpdating customer");
            var updateCustomer = new CustomerDto
            {
                id = Guid.Parse("0193bc5d-d620-7f7c-b8b4-ca7e1d1f4c1f"),
                firstname = "Noe",
                middlename = "Rich",
                lastname = "Bmw",
                emailaddress = "noe.mw@yahoo.com",
                phonenumber = "(1)-513-777-2221",
                creationdate = DateTime.Now,
                updateddate = DateTime.Now
            };

            var customerUpdated = await customerService.UpdateCustomer(updateCustomer);
            Console.WriteLine($"UpdateCustomer:\nID: {customerUpdated!.id}, FullName: {customerUpdated.firstname}-{customerUpdated.lastname}, Email: {customerUpdated.emailaddress}");
        }

        static async Task DeleteCustomer(CustomerServices customerService)
        {
            Console.WriteLine("\nDeleting customer...");
            var deleteCustomer = await customerService.DeleteCustomer(Guid.Parse("0193bc5d-d620-7f7c-b8b4-ca7e1d1f4c1f"));
            Console.WriteLine($"DeleteCustomer:\nID: {deleteCustomer!.id}, FullName: {deleteCustomer.firstname}-{deleteCustomer.lastname}, Email: {deleteCustomer.emailaddress}");
        }

    }
}
