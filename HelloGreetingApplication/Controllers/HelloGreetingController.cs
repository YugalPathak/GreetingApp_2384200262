using BusinessLayer.Interface;
using Microsoft.AspNetCore.Mvc;
using ModelLayer.Model;
using NLog;
using RepositoryLayer.Entity;

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
        /// Logs captured by logger instance
        /// </summary>
        private static readonly Logger logger = LogManager.GetCurrentClassLogger();

        /// <summary>
        /// Constructor to initialize the controller with Greeting Business Logic Layer.
        /// </summary>
        /// <param name="greetingBL">Instance of IGreetingBL for handling greetings.</param>
        /// 
        public HelloGreetingController(IGreetingBL greetingBL)
        {
            _greetingBL = greetingBL;
        }

        /// <summary>
        /// Default GET method to retrieve a greeting message.
        /// </summary>
        /// <returns>JSON response with a greeting message.</returns>

        [HttpGet]
        [Route("GetPersonalGreet")]
        public IActionResult GetPersonalGreet(string? firstName, string? lastName)
        {
            logger.Info($"GET request received for greeting with FirstName: {firstName}, LastName: {lastName}");

            string message = _greetingBL.GetGreetMessage(firstName, lastName);

            ResponseModel<string> responseModel = new ResponseModel<string>
            {
                Success = true,
                Message = "Greeted successfully",
                Data = message
            };

            return Ok(responseModel);
        }

        /// <summary>
        /// Post method to saved greeting method successfully
        /// </summary>
        /// <param name="helloEntity"></param>
        /// <returns>Saved Greeting Message</returns>
        [HttpPost]
        [Route("SAVE")]
        public IActionResult SaveGreetingMessage(HelloGreetingEntity helloEntity)
        {
            try
            {
                _greetingBL.SaveGreeting(helloEntity.message);
                return Ok(new { Success = true, Message = "Saved Successfully" });
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Error saving greeting");
                return StatusCode(500, new { Success = false, Message = "An error occurred" });
            }
        }

        /// <summary>
        /// Retrieves a greeting message by its ID.
        /// </summary>
        /// <param name="id"></param>
        /// <returns>The greeting message</returns>
        [HttpGet]
        [Route("GetID")]
        public IActionResult RetrievesById(int id)
        {
            try
            {
                var text = _greetingBL.GetMessageById(id);
                if (text == null)
                {
                    return NotFound(new { Success = false, Message = "Message not found" });
                }

                return Ok(new { Success = true, Message = "Message retrieved successfully", Data = text });
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Error retrieving greeting message");
                return StatusCode(500, new { Success = false, Message = "An error occurred" });
            }
        }

        /// <summary>
        /// Retrieves all the messages from the database
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("GetMessages")]
        public IActionResult GetAllMessage()
        {
            try
            {
                var text = _greetingBL.GetMessages();
                if (text == null || text.Count == 0)
                {
                    return NotFound(new { Success = false, Message = "No Message found" });
                }

                return Ok(new { Success = true, Message = "Message retrieved successfully", Data = text });
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Error retrieving all greetings");
                return StatusCode(500, new { Success = false, Message = "An error occurred" });
            }
        }

        /// <summary>
        /// Updates an existing message in the database
        /// </summary>
        /// <param name="id"></param>
        /// <param name="newMessage"></param>
        /// <returns>True or False</returns>
        [HttpPut]
        [Route("Change")]
        public IActionResult UpdateGreetingMessage(int id, string updatedMessage)
        {
            try
            {
                var isUpdated = _greetingBL.UpdateMessage(id, updatedMessage);
                if (!isUpdated)
                {
                    return NotFound(new { Success = false, Message = "Message not found" });
                }

                return Ok(new { Success = true, Message = "Message updated successfully" });
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Error");
                return StatusCode(500, new { Success = false, Message = "An error occurred" });
            }
        }

        /// <summary>
        /// Deletes the message from the database
        /// </summary>
        /// <param name="id"></param>
        /// <returns>True or false</returns>
        [HttpDelete]
        [Route("DeleteMessage")]
        public IActionResult DeleteMessageById(int id)
        {
            try
            {
                var isDeleted = _greetingBL.DeleteMessage(id);
                if (!isDeleted)
                {
                    return NotFound(new { Success = false, Message = "Message not found" });
                }

                return Ok(new { Success = true, Message = "Message deleted successfully" });
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Error");
                return StatusCode(500, new { Success = false, Message = "An error occurred" });
            }
        }


        /// <summary>
        /// Get greeting message fron GET method
        /// </summary>
        /// <returns>"Hello, World!"</returns>
        [HttpGet]
        [Route("GET")]
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
        [Route("POST")]
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
        [Route("PUT")]
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
        [Route("PATCH")]
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
        [Route("DELETE")]
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