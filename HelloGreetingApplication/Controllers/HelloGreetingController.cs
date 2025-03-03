using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
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
        /// <summary>
        /// Logs captured by logger instance
        /// </summary>
        private static readonly Logger logger = LogManager.GetCurrentClassLogger();

        /// <summary>
        /// Get greeting message fron GET method
        /// </summary>
        /// <returns>"Hello, World!"</returns>
        [HttpGet]
        [Route("get")]
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
        [Route("post")]
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
        [Route("put")]
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
        [Route("patch")]
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
        /// </summary>
        /// <param name="requestModel"></param>
        /// <returns>response model</returns>
        [HttpDelete]
        [Route("delete")]
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