using HomeAssigment.Interface;
using HomeAssigment.DataAccessLayer;
using HomeAssigment.Models;
using Microsoft.EntityFrameworkCore;

namespace HomeAssigment.Services
{
    public class CustomerService : ICustomer
    {
        #region Private Attributes
        private readonly ILogger<CustomerService> _logger;
        private readonly CustomerDbContext _customerContext;
        #endregion

        #region Constructor
        public CustomerService(ILogger<CustomerService> logger, CustomerDbContext customerContext)
        {
            _logger = logger;
            _customerContext = customerContext;

        }
        #endregion Constructor

        #region Methods

        public async Task<Customer?> GetCustomerById(Guid id)
        {
            try
            {
                _logger.LogInformation(new EventId(), "CustomerService-GetCustomerServiceById-Started.Id:{Id}", id);
                Customer? customer = await _customerContext.Customers.SingleOrDefaultAsync(x => x.id == id);
                _logger.LogInformation(new EventId(),"CustomerService-GetCustomerServiceById-Ended.Id:{Id}", id);

                return customer;
            }
            catch (Exception ex)
            {
                _logger.LogError(new EventId(), ex, "CustomerService-GetCustomerServiceById raised exception.Id:{Id}", id);
            }

            return null;
        }

        public async Task<List<Customer>?> GetAllCustomer()
        {
            try
            {
                _logger.LogInformation(new EventId(), "CustomerService-GetAllCustomer-Started");
                List<Customer> all = await _customerContext.Customers.ToListAsync();
                _logger.LogInformation(new EventId(), "CustomerService-GetAllCustomer-Ended.CountCustomers:{number}", all.Count);

                return all;
            }
            catch (Exception ex)
            {
                _logger.LogError(new EventId(), ex, "CustomerService-GetCustomerAll raised exception");
            }

            return null;
        }

        public async Task<Customer?> CreateCustomer(Customer item)
        {
            try
            {
                _logger.LogInformation(new EventId(), "CustomerService-CreateCustomer-FullName:{fullname}-Started...", $"{item.firstname}-{item.lastname}");
                item = new Customer()
                {
                    firstname = item.firstname,
                    emailaddress = item.emailaddress,
                    middlename = item.middlename,
                    lastname = item.lastname,
                    phonenumber=item.phonenumber,
                    creationdate = DateTime.Now,
                    updateddate = item.updateddate
                };
                await _customerContext.Customers.AddAsync(item);
                await _customerContext.SaveChangesAsync();

                _logger.LogInformation(new EventId(), "CustomerService-CreateCustomer-FullName:{fullname}-Ended...", $"{item.firstname}-{item.lastname}");
                return item;
            }
            catch (Exception ex)
            {
                _logger.LogError(new EventId(), ex, "CustomerService-CreateCustomer raised exception.Id:{Id}", item.id);
            }

            return null;
        }

        public async Task<Customer?> UpdateCustomer(Customer customer)
        {
            try
            {
                _logger.LogInformation(new EventId(), "CustomerService-UpdateCustomer-Id:{customer.Id} started...", customer.id);
                if (IsValidItem(customer))
                {
                    if (customer.id != Guid.Empty)
                    {
                        Customer? checkItem = await _customerContext.Customers.SingleOrDefaultAsync(x => x.id == customer.id);
                        if (checkItem is not null && checkItem?.id != Guid.Empty)
                        {
                            checkItem!.firstname=customer.firstname;
                            checkItem.lastname=customer.lastname;
                            checkItem.phonenumber=customer.phonenumber;
                            checkItem.emailaddress=customer.emailaddress;
                            customer.updateddate = DateTime.Now;

                            _customerContext.ChangeTracker.Clear();
                            _customerContext.Attach(customer);
                            _customerContext.Entry(customer).State = EntityState.Modified;
                            _customerContext.SaveChanges();
                            _logger.LogInformation(new EventId(), "CustomerService-UpdateCustomer-Id:{customer.Id} ended successfully.", customer.id);
                        }

                        return customer;
                    }
                    _logger.LogInformation(new EventId(), "CustomerService-UpdateCustomer-Id:{customer.Id} failed.", customer.id);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(new EventId(), ex, "CustomerService-Updatecustomer failed and raised exception.-Id:{customer.Id} failed.", customer.id);
            }

            return null;
        }

        public async Task<Customer?> DeleteCustomer(Guid customerId)
        {
            try
            {
                _logger.LogInformation(new EventId(), "CustomerService-DeleteCustomer-Id:{customerId} started...", customerId);

                if (customerId != Guid.Empty)
                {
                    Customer? checkItem = await _customerContext.Customers.SingleOrDefaultAsync(x => x.id == customerId);
                    if (checkItem is not null && checkItem?.id != Guid.Empty)
                    {
                        if (_customerContext.Customers.Entry(checkItem!).State == EntityState.Detached)
                        {
                            _customerContext.Customers.Attach(checkItem!);
                        }
                        _customerContext.Customers.Remove(checkItem!);
                        await _customerContext.SaveChangesAsync();  
                        _logger.LogInformation(new EventId(), "CustomerService-DeleteCustomer-Id:{customerId} ended successfully.", customerId);
                    }

                    return checkItem;
                }
                _logger.LogInformation(new EventId(), "CustomerService-DeleteCustomer-Id:{customerId} failed.", customerId);
            }
            catch (Exception ex)
            {
                _logger.LogError(new EventId(), ex, "CustomerService-DeleteCustomer-Id:{customerId} raised exception.", customerId);
            }

            return null;
        }


        #endregion

        #region Private Methods

        private static bool IsValidItem(Customer customer)
        {
            if (customer == null)
            {
                return false;
            }
            else
            {
                return customer.id != Guid.Empty;
            }
        }

        #endregion
    }
}
