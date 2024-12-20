using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System;
using HomeAssigment.DataAccessLayer;

namespace HomeAssigment
{
    public class CustomerWebApplicationFactory : WebApplicationFactory<Startup>
    {
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureServices(services =>
            {
                // Remove the existing DbContext registration
                var descriptor = services.SingleOrDefault(
                    d => d.ServiceType == typeof(DbContextOptions<CustomerDbContext>));

                if (descriptor != null)
                {
                    services.Remove(descriptor);
                }
                
                // Add an in-memory database for testing
                services.AddDbContext<CustomerDbContext>(options =>
                {
                    options.UseInMemoryDatabase("TestDb");
                });

                // Optional: Seed the in-memory database with initial data
                var sp = services.BuildServiceProvider();
                using var scope = sp.CreateScope();
                var db = scope.ServiceProvider.GetRequiredService<CustomerDbContext>();
                db.Database.EnsureCreated();
                db.Customers.Add(new Models.Customer
                {
                    id = Guid.NewGuid(),
                    firstname = "Bob",
                    middlename = null,
                    lastname = "Nick",
                    emailaddress = "bob.nick@hmail.com",
                    phonenumber = "(1)513-256-0000"
                });

                db.Customers.Add(new Models.Customer
                {
                    id = Guid.NewGuid(),
                    firstname = "Eve",
                    middlename = "Mary",
                    lastname = "Wash",
                    emailaddress = "eve.wash@yahoo.com",
                    phonenumber = "(1)510-111-2222"
                });
                db.SaveChanges();
            });
        }
    }
}
