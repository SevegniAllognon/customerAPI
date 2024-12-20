using System.Net.Http.Json;
using Xunit;
using Microsoft.Extensions.DependencyInjection;
using HomeAssigment.Models;
using Microsoft.EntityFrameworkCore;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using HomeAssigment;
using HomeAssigment.DataAccessLayer;


namespace HomeAssignment.Testings.IntegrationTesting
{
    [TestClass]
    [Trait("Category", "CustomerAPIIntegrationTest")]
    public class CustomerAPIIntegrationTests: IClassFixture<WebApplicationFactory<Startup>>
    {
        private readonly HttpClient _client;
        public CustomerAPIIntegrationTests(WebApplicationFactory<Startup> factory)
        {
            _client = factory.WithWebHostBuilder(builder =>
            {
                builder.ConfigureServices(services =>
                {
                    // Replace DbContext with In-Memory Database
                    var descriptor = services.SingleOrDefault(
                        d => d.ServiceType == typeof(DbContextOptions<CustomerDbContext>));
                    if (descriptor != null)
                    {
                        services.Remove(descriptor);
                    }

                    services.AddDbContext<CustomerDbContext>(options =>
                    {
                        options.UseInMemoryDatabase("TestDb");
                    });
                });
            }).CreateClient();
        }

        [Fact]
        public async Task PostUser_ShouldAddNewUser()
        {
            // Arrange
            var newCustomer = new Customer
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

            // Act
            var response = await _client.PostAsJsonAsync("/api/Customer/CreateCustomer", newCustomer);

            // Assert
            response.EnsureSuccessStatusCode();
            var createdUser = await response.Content.ReadFromJsonAsync<Customer>();

            createdUser!.lastname.Should().Be("Bmw");
            createdUser.emailaddress.Should().Be("noe.mw@yahoo.com");
        }

        [Fact]
        public async Task GetCustomers_ReturnsData()
        {
            // Act
            var response = await _client.GetAsync("/api/Customer/GetCustomers");

            // Assert
            response.EnsureSuccessStatusCode();
            var customers = await response.Content.ReadFromJsonAsync<Customer[]>();

            customers.Should().NotBeNullOrEmpty();
            customers![0].firstname.Should().Be("Bob");
            customers![1].emailaddress.Should().Be("eve.wash@yahoo.com");
        }

        [Fact]
        public async Task GetCustomerById_ReturnsData()
        {
            // Act 
            var response = await _client.GetAsync($"/api/Customer/GetCustomerById?id={Guid.NewGuid()}");

            // Assert
            response.EnsureSuccessStatusCode();
            var customers = await response.Content.ReadFromJsonAsync<Customer>();

            customers.Should().NotBeNull();
            customers!.firstname.Should().Be("Bob");
            customers!.emailaddress.Should().Be("eve.wash@yahoo.com");
        }


        [Fact]
        public async Task PutUser_ShouldUpdateUser()
        {
            // Arrange
            var updateCustomer = new Customer
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

            // Act
            var response = await _client.PutAsJsonAsync("/api/Customer/UpdateCustomer", updateCustomer);

            // Assert
            response.EnsureSuccessStatusCode();
            var updatedUser = await response.Content.ReadFromJsonAsync<Customer>();

            updatedUser!.lastname.Should().Be("Bmw");
            updatedUser.emailaddress.Should().Be("noe.mw@yahoo.com");
            updatedUser.phonenumber.Should().Be("(1)-513-777-2221");
        }

        [Fact] 
        public async Task DeleteUser_ShouldUpdateUser()
        {
            // Arrange
            Guid id = Guid.Parse("0193bc5d-d620-7f7c-b8b4-ca7e1d1f4c1f");
            // Act
            var response = await _client.DeleteAsync($"/api/Customer/DeleteCustomer? id={id}");

            // Assert
            response.EnsureSuccessStatusCode();
            var deletedUser = await response.Content.ReadFromJsonAsync<Customer>();

            deletedUser!.lastname.Should().Be("Bmw");
            deletedUser.emailaddress.Should().Be("noe.mw@yahoo.com");
            deletedUser.phonenumber.Should().Be("(1)-513-777-2221");
        }
    }
}
