using HomeAssigment.Interface;
using HomeAssigment.Models;
using Moq;
using HomeAssigment.DataAccessLayer;

namespace HomeAssignment.Testings.UnitTesting
{
    [TestClass]
    public class CustomerServiceUnitTests
    {

        [TestMethod]
        public async Task GetCustomerById_Should_Return_Valid_Data()
        {
            //Arrange
            var customerApi = new Mock<ICustomer>();
            var customerContext = new Mock<CustomerDbContext>();
            customerApi.Setup(service => service.GetCustomerById(It.IsAny<Guid>()))
                .ReturnsAsync(new Customer
                {
                    id = Guid.NewGuid(),
                    firstname = "Bob",
                    middlename = "Nick",
                    lastname = "john",
                    emailaddress = "bob.john@hotmail.com",
                    phonenumber = "1514-800-789",
                    creationdate = DateTime.Now,
                    updateddate = null
                });

            //Act
            var result = await customerApi.Object.GetCustomerById(Guid.NewGuid());

            //Assert
            Assert.AreEqual("Bob", result!.firstname);
        }

        [TestMethod]
        public async Task GetCustomers_Should_Return_Valid_Data()
        {
            //Arrange
            List<Customer> customers = [ new Customer
                {
                    id = Guid.NewGuid(),
                    firstname = "Bob",
                    middlename = "Nick",
                    lastname = "john",
                    emailaddress = "bob.john@hotmail.com",
                    phonenumber = "1514-800-789",
                    creationdate = DateTime.Now,
                    updateddate = null
                },new Customer
                {
                    id = Guid.NewGuid(),
                    firstname = "Kack",
                    middlename = "Eve",
                    lastname = "Volvo",
                    emailaddress = "kack.volvo@yahoo.com",
                    phonenumber = "1-513-912-7777",
                    creationdate = DateTime.Now,
                    updateddate = null
                }];
            var customerApi = new Mock<ICustomer>();
            var customerContext = new Mock<CustomerDbContext>();
            customerApi.Setup(service => service.GetAllCustomer())
                .ReturnsAsync(customers);

            //Act
            var result = await customerApi.Object.GetAllCustomer();

            //Assert
            Assert.IsTrue(customers.Count == result!.Count);
            Assert.AreEqual("Bob", result![0].firstname);
            Assert.AreEqual("Volvo", result![1].lastname);
        }

        [TestMethod]
        public async Task CreateCustomer_Should_Return_Valid_Data()
        {
            //Arrange
            var customerApi = new Mock<ICustomer>();
            var customerContext = new Mock<CustomerDbContext>();
            customerApi.Setup(service => service.CreateCustomer(It.IsAny<Customer>()))
                .ReturnsAsync(new Customer
                {
                    id = Guid.NewGuid(),
                    firstname = "Kack",
                    middlename = "Eve",
                    lastname = "Volvo",
                    emailaddress = "kack.volvo@yahoo.com",
                    phonenumber = "1-513-912-7777",
                    creationdate = DateTime.Now,
                    updateddate = null
                });

            //Act
            var result = await customerApi.Object.CreateCustomer(It.IsAny<Customer>());

            //Assert
            Assert.IsNotNull(result);
            Assert.AreEqual("Eve", result.middlename);
            Assert.AreEqual("Volvo", result.lastname);
        }

        [TestMethod]
        public async Task UpdateCustomer_Should_Return_Valid_Data()
        {
            //Arrange
            var customerApi = new Mock<ICustomer>();
            var customerContext = new Mock<CustomerDbContext>();
            customerApi.Setup(service => service.UpdateCustomer(It.IsAny<Customer>()))
                .ReturnsAsync(new Customer
                {
                    id = Guid.NewGuid(),
                    firstname = "Kackup",
                    middlename = "Eveup",
                    lastname = "Volvoup",
                    emailaddress = "kackup.volvoup@yahoo.com",
                    phonenumber = "1-512-000-1111",
                    creationdate = DateTime.Now,
                    updateddate = null
                });

            //Act
            var result = await customerApi.Object.UpdateCustomer(It.IsAny<Customer>());

            //Assert
            Assert.IsNotNull(result);
            Assert.AreEqual("Kackup", result.firstname);
            Assert.AreEqual("Eveup", result.middlename);
            Assert.AreEqual("kackup.volvoup@yahoo.com", result.emailaddress);
        }

        [TestMethod]
        public async Task DeleteCustomer_Should_Return_Valid_Data()
        {
            //Arrange
            var customerApi = new Mock<ICustomer>();
            var customerContext = new Mock<CustomerDbContext>();
            customerApi.Setup(service => service.DeleteCustomer(It.IsAny<Guid>()))
                .ReturnsAsync(new Customer
                {
                    id = Guid.NewGuid(),
                    firstname = "Kackup",
                    middlename = "Eveup",
                    lastname = "Volvoup",
                    emailaddress = "kackup.volvoup@yahoo.com",
                    phonenumber = "1-512-000-1111",
                    creationdate = DateTime.Now,
                    updateddate = null
                });

            //Act
            var result = await customerApi.Object.DeleteCustomer(It.IsAny<Guid>());

            //Assert
            Assert.IsNotNull(result);
            Assert.AreEqual("1-512-000-1111", result.phonenumber);
        }
    }
}