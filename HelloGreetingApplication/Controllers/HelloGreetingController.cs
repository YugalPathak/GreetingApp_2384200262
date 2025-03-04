using BusinessLayer.Interface;
using Microsoft.AspNetCore.Mvc;
using ModelLayer.Model;
using NLog;

namespace HelloGreetingApplication.Controllers
{
    /// <summary>
    /// API class of HelloGreeting
    /// </summary>
    [ApiController]
    [Route("[controller]")]
    public class HelloGreetingController : ControllerBase
    {
        private readonly IGreetingBL _greetingBL;

        /// <summary>
        /// Constructor to initialize the controller with Greeting Business Logic Layer.
        /// </summary>
        /// <param name="greetingBL">Instance of IGreetingBL for handling greetings.</param>
        public HelloGreetingController(IGreetingBL greetingBL)
        {
            _greetingBL = greetingBL;
        }

        /// <summary>
        /// Default GET method to retrieve a greeting message.
        /// </summary>
        /// <returns>JSON response with a greeting message.</returns>
        [HttpGet]
        public IActionResult GetGreetMessage()
        {
            var message = _greetingBL.GetGreetMessage();
            return Ok(new { Message = message });
        }
        /// <summary>
        /// Logs captured by logger instance
        /// </summary>
        private static readonly Logger logger = LogManager.GetCurrentClassLogger();

        /// <summary>
        /// Get greeting message fron GET method
        /// </summary>
        /// <returns>"Hello, World!"</returns>
        [HttpGet]
        [Route("get-greeting")]
        public IActionResult Get()
        {
            logger.Info("GET request received for greeting.");

            ResponseModel<string> responseModel = new ResponseModel<string>();
            responseModel.Success = true;
            responseModel.Message = "Hello to Greeting App API Endpoint";
            responseModel.Data = "Hello, World!";
            return Ok(responseModel);
        }

        /// <summary>
        /// Receive request from POST method
        /// </summary>
        /// <param name="requestModel"></param>
        /// <returns>response model</returns>
        [HttpPost]
        [Route("post-greeting")]
        public IActionResult Post(RequestModel requestModel)
        {
            logger.Info($"POST request received with Key: {requestModel.Key}, Value: {requestModel.Value}");

            ResponseModel<string> responseModel = new ResponseModel<string>();
            responseModel.Success = true;
            responseModel.Message = "Request received successfully";
            responseModel.Data = $"Key: {requestModel.Key}, Value: {requestModel.Value}";
            return Ok(responseModel);
        }

        /// <summary>
        /// Receive request from PUT method
        /// </summary>
        /// <param name="requestModel"></param>
        /// <returns>response model</returns>
        [HttpPut]
        [Route("update-greeting")]
        public IActionResult Put(RequestModel requestModel)
        {
            logger.Info($"PUT request received. Updating greeting to: {requestModel.Value}");

            ResponseModel<string> responseModel = new ResponseModel<string>();
            responseModel.Success = true;
            responseModel.Message = "Value updated successfully";
            responseModel.Data = requestModel.Value;
            return Ok(responseModel);
        }

        /// <summary>
        /// Receive request from PATCH method
        /// </summary>
        /// <param name="requestModel"></param>
        /// <returns>response model</returns>
        [HttpPatch]
        [Route("modify-greeting")]
        public IActionResult Patch(RequestModel requestModel)
        {
            logger.Info($"PATCH request received. Modifying greeting with: {requestModel.Value}");

            ResponseModel<string> responseModel = new ResponseModel<string>();
            responseModel.Success = true;
            responseModel.Message = "Value updated successfully";
            responseModel.Data = requestModel.Value;
            return Ok(responseModel);
        }

        /// <summary>
        /// Delete Method to remove the greeting message
        /// </summary>
        /// <param name="requestModel"></param>
        /// <returns>response model</returns>
        [HttpDelete]
        [Route("delete-greeting")]
        public IActionResult Delete(RequestModel requestModel)
        {
            logger.Info($"DELETE request received. Removing greeting for key: {requestModel.Key}");

            ResponseModel<string> responseModel = new ResponseModel<string>();
            responseModel.Success = true;
            responseModel.Message = "Value deleted successfully";
            responseModel.Data = string.Empty;
            return Ok(responseModel);

        }
    }
}