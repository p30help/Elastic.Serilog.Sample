using Microsoft.AspNetCore.Mvc;

namespace Elastic.Serilog.Sample.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CustomersController : ControllerBase
    {    
        private readonly ILogger<CustomersController> _logger;

        public CustomersController(ILogger<CustomersController> logger)
        {
            _logger = logger;
        }

        [HttpPost("CreateCustomer")]
        public IActionResult CreateCustomer()
        {
            var userId = Random.Shared.Next(0, 320000000);
            try
            {
                if (Random.Shared.Next(0, 5) > 3)
                {
                    var exp = new Exception("User data is not complete");


                    throw exp;
                }

                return Ok();
            }
            catch (Exception exp)
            {
                _logger.LogError(exp, "can not create user, user:{user_id}", userId.ToString());

                return new StatusCodeResult(500);
            }
        }


        [HttpDelete("DeleteCustomer/{id}")]
        public IActionResult PutCustomer(int id)
        {
            try
            {
                if (Random.Shared.Next(0, 5) > 3)
                {
                    throw new Exception("User not found");
                }

                return Ok();
            }
            catch (Exception exp)
            {
                _logger.LogError(exp, "can not delete the user, user:{user_id}", id.ToString());

                return new StatusCodeResult(500);
            }

        }
    }
}