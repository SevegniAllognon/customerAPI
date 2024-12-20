using HomeAssigment.Interface;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using HomeAssigment.Models;


namespace HomeAssigment.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerController : ControllerBase
    {
        #region Attribute
        private readonly ILogger<CustomerController> _logger;
        private readonly ICustomer _customerService;
        #endregion

        #region Constructor
        public CustomerController(ILogger<CustomerController> logger, ICustomer customerService)
        {
            _logger = logger;
            _customerService = customerService;
        }

        [HttpGet("GetCustomerById")]
        [ProducesResponseType(typeof(Customer), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<Customer>> GetCustomerById(Guid id)
        {
            if (id == Guid.Empty)
            {
                return BadRequest("Please provide a valid Customer Id.");
            }
            _logger.LogInformation("CustomerController-GetCustomerById-Id:{id}-Started...",id);
            var response = await _customerService.GetCustomerById(id);
            _logger.LogInformation("CustomerController-GetCustomerById-Id:{id}-Ended...", id);

            return Ok(response);
        }

        [HttpGet("GetCustomers")]
        [ProducesResponseType(typeof(List<Customer>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<List<Customer>>> GetCustomers()
        {
            _logger.LogInformation("CustomerController-GetCustomers-Started...");
            var response = await _customerService.GetAllCustomer();
            _logger.LogInformation("CustomerController-GetCustomers-Ended...");

            return Ok(response);
        }

        [HttpPost("CreateCustomer")]
        [ProducesResponseType(typeof(Customer), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.BadRequest)]
        public async Task<ActionResult<Customer>> CreateCustomer(Customer item)
        {
            if(string.IsNullOrWhiteSpace(item.firstname))
            {
                return BadRequest("Please provide a valid FirstName.");
            }

            if (string.IsNullOrWhiteSpace(item.lastname))
            {
                return BadRequest("Please provide a valid LastName.");
            }
            _logger.LogInformation("CustomerController-CreateCustomer-FullName:{fullname}-Started...", $"{item.firstname}-{item.lastname}");
            var response = await _customerService.CreateCustomer(item);
            _logger.LogInformation("CustomerController-CreateCustomer-FullName:{fullname}-Ended...", $"{item.firstname}-{item.lastname}");

            return Ok(response);
        }


        [HttpPut("UpdateCustomer")]
        [ProducesResponseType(typeof(Customer), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.BadRequest)]
        public async Task<ActionResult<Customer?>> UpdateCustomer(Customer item)
        {
            if (string.IsNullOrWhiteSpace(item.firstname))
            {
                return BadRequest("Please provide a valid FirstName.");
            }

            if (string.IsNullOrWhiteSpace(item.lastname))
            {
                return BadRequest("Please provide a valid LastName.");
            }
            _logger.LogInformation("CustomerController-UpdateCustomer-FullName:{fullname}-Ended...", $"{item.firstname}-{item.lastname}");
            var response = await _customerService.UpdateCustomer(item);
            _logger.LogInformation("CustomerController-UpdateCustomer-FullName:{fullname}-Ended...", $"{item.firstname}-{item.lastname}");

            return response;
        }

        [HttpDelete("DeleteCustomer")]
        [ProducesResponseType(typeof(Customer), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.BadRequest)]
        public async Task<ActionResult<Customer?>>DeleteCustomer(Guid id)
        {
            if (id == Guid.Empty)
            {
                return BadRequest("Please provide a valid Customer Id.");
            }
            _logger.LogInformation("CustomerController-DeleteCustomer-Id:{id}-Started...", id);
            var response = await _customerService.DeleteCustomer(id);
            _logger.LogInformation("CustomerController-DeleteCustomer-Id:{id}-Ended...", id);

            return response;
        }




        #endregion
    }
}
