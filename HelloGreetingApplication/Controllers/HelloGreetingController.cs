
using BusinessLayer.Interface;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.Extensions.Logging;
using ModelLayer.Model;
using System.Collections.Generic;
using System.Linq;

namespace HelloGreetingApplication.Controllers
{
    /// <summary>
    /// Provides API for HelloGreetingApp
    /// </summary>
    [ApiController]
    [Route("[controller]")]
    public class HelloGreetingController : ControllerBase
    {
        private readonly ILogger<HelloGreetingController> _logger;
        private readonly IGreetingBL greetingBL;
        private static Dictionary<int, RequestModel> inMemoryStore = new Dictionary<int, RequestModel>
        {
            { 1, new RequestModel { Key = "Greeting", Value = "Hello, World!" } },
            { 2, new RequestModel { Key = "Farewell", Value = "Goodbye!" } },
            { 3, new RequestModel { Key = "Welcome", Value = "Welcome to the API!" } }
        };

        public HelloGreetingController(IGreetingBL greetings,ILogger<HelloGreetingController> logger)
        {
            greetingBL = greetings;
            _logger = logger;
        }

        /// <summary>
        /// This is test Exception API
        /// </summary>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        [HttpGet("exception")]
        public IActionResult ThrowException()
        {
            throw new Exception("This is a test exception");
        }

        /// <summary>
        /// Delete method to delete a greeting message.
        /// </summary>
        /// <param name="id"></param>
        /// <returns>response model</returns>
        [HttpDelete("deleteGreeting/{id}")]
        public IActionResult DeleteGreeting(int id)
        {
            _logger.LogInformation("API: Received request to delete greeting with ID: {Id}", id);

            bool isDeleted = greetingBL.DeleteGreeting(id);
            if (!isDeleted)
            {
                _logger.LogWarning("API: Greeting with ID {Id} not found for deletion.", id);
                return NotFound(new { message = "Greeting not found" });
            }

            _logger.LogInformation("API: Greeting with ID {Id} deleted successfully.", id);
            return Ok(new { success = true, message = "Greeting deleted successfully" });
        }

        /// <summary>
        /// Updates a greeting message by id
        /// </summary>
        /// <param name="id"></param>
        /// <param name="greetingModel"></param>
        /// <returns>response model</returns>
        [HttpPut("updateGreeting/{id}")]
        public IActionResult UpdateGreeting(int id, [FromBody] GreetingModel greetingModel)
        {
            _logger.LogInformation("API: Received request to update greeting with ID: {Id}", id);

            if (greetingModel == null || string.IsNullOrEmpty(greetingModel.Message))
            {
                _logger.LogError("API: Invalid greeting data for update.");
                return BadRequest(new { success = false, message = "Greeting message cannot be empty" });
            }

            bool updated = greetingBL.UpdateGreeting(id, greetingModel);

            if (!updated)
            {
                _logger.LogWarning("API: Greeting with ID {Id} not found.", id);
                return NotFound(new { success = false, message = "Greeting not found" });
            }

            _logger.LogInformation("API: Greeting with ID {Id} updated successfully.", id);
            return Ok(new { success = true, message = "Greeting updated successfully" });
        }


        /// <summary>
        /// Post method to Retrieve all greeting messages.
        /// </summary>
        /// <returns>response model</returns>
        [HttpGet("retrieveGreeting/all")]
        public IActionResult GetAllGreetings()
        {
            _logger.LogInformation("API: Fetching all greetings.");
            var greetings = greetingBL.GetAllGreetings();
            return Ok(new ResponseModel<List<GreetingModel>>
            {
                success = true,
                message = "All greetings retrieved successfully",
                data = greetings
            });
        }

        /// <summary>
        /// Retrieves a greeting message by ID.
        /// </summary>
        /// <param name="id"></param>
        /// <returns>response model</returns>
        [HttpGet("retrieveGreetingById/{id}")]
        public IActionResult GetGreetingById(int id)
        {
            _logger.LogInformation("API: Received request to fetch greeting with ID: {Id}", id);
            var greeting = greetingBL.GetGreetingById(id);

            if (greeting == null)
            {
                _logger.LogWarning("API: Greeting with ID {Id} not found.", id);
                return NotFound(new { message = "Greeting not found" });
            }

            return Ok(new ResponseModel<GreetingModel>
            {
                success = true,
                message = "Greeting retrieved successfully",
                data = greeting
            });
        }

        /// <summary>
        /// Post method to save greeting in database
        /// </summary>
        /// <param name="greetingModel"></param>
        /// <returns>string</returns>
        [HttpPost("greeting/save")]
        public IActionResult SaveGreeting(GreetingModel greetingModel)
        {
            try
            {
                if (greetingModel == null || string.IsNullOrEmpty(greetingModel.Message))
                {
                    _logger.LogError("Invalid greeting data received.");
                    return BadRequest(new
                    {
                        success = false,
                        message = "Greeting message cannot be empty"
                    });
                }

                greetingBL.SaveGreeting(greetingModel);
                return Ok("Greeting saved successfully.");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error saving greeting: {ex.Message}");
                return StatusCode(500, "An error occurred while saving the greeting.");
            }
        }
        /// <summary>
        /// Get method to fetch all records
        /// </summary>
        /// <returns>response model</returns>
        [HttpGet]
        public IActionResult Get()
        {
            _logger.LogInformation("GET request received to fetch all records.");
            return Ok(new ResponseModel<Dictionary<int, RequestModel>>
            {
                success = true,
                message = "Retrieved all records",
                data = inMemoryStore
            });
        }

        /// <summary>
        /// Get method to fetch greeting message
        /// </summary>
        /// <returns>response model</returns>
        [HttpGet("greeting")]
        public IActionResult GetGreeting(){
            _logger.LogInformation("Fetching greeting message.");
            string message = greetingBL.HelloGreeting();

            return Ok(new ResponseModel<string>
            {
                success = true,
                message = "Greeting retrieved successfully",
                data = message
            });
        }

        /// <summary>
        /// Get method to generate greeting message based on attributes provided
        /// </summary>
        /// <param name="firstName"></param>
        /// <param name="lastName"></param>
        /// <returns></returns>
        [HttpGet("greet")]
        public IActionResult GetGreetingByName(string? firstName,string? lastName)
        {
            string message = "Hello";

            if (!string.IsNullOrEmpty(firstName) && !string.IsNullOrEmpty(lastName))
                message += $", {firstName} {lastName}!";
            else if (!string.IsNullOrEmpty(firstName))
                message += $", {firstName}!";
            else if (!string.IsNullOrEmpty(lastName))
                message += $", Mr./Ms. {lastName}!";
            else
                message += ", World!";

            return Ok(new { success = true, message = "Greeting message generated", data = message });
        }


        /// <summary>
        /// Post method to add a key-Value pair
        /// </summary>
        /// <param name="requestModel"></param>
        /// <returns>response model</returns>
        [HttpPost]
        public IActionResult Post(RequestModel requestModel)
        {
            if (requestModel == null)
            {
                _logger.LogWarning("POST request failed due to invalid request data.");
                return BadRequest("Invalid request data.");
            }

            int newId = inMemoryStore.Keys.Any() ? inMemoryStore.Keys.Max() + 1 : 1;
            inMemoryStore[newId] = requestModel;

            _logger.LogInformation($"New record added: ID={newId}, Key={requestModel.Key}, Value={requestModel.Value}");

            return Created($"/HelloGreeting/{newId}", new ResponseModel<string>
            {
                success = true,
                message = "Record added successfully",
                data = $"ID: {newId}, Key: {requestModel.Key}, Value: {requestModel.Value}"
            });
        }

       /// <summary>
       /// Put method to update value by id
       /// </summary>
       /// <param name="id"></param>
       /// <param name="requestModel"></param>
       /// <returns>response model</returns>
        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody] RequestModel requestModel)
        {
            if (!inMemoryStore.ContainsKey(id))
            {
                _logger.LogWarning($"PUT request failed. Record with ID={id} not found.");
                return NotFound(new { message = "Record not found" });
            }

            inMemoryStore[id] = requestModel;
            _logger.LogInformation($"Record updated: ID={id}, Key={requestModel.Key}, Value={requestModel.Value}");

            return Ok(new ResponseModel<string>
            {
                success = true,
                message = "Record updated successfully",
                data = $"ID: {id}, Key: {requestModel.Key}, Value: {requestModel.Value}"
            });
        }

        /// <summary>
        /// patch method to patch key-value pair
        /// </summary>
        /// <param name="id"></param>
        /// <param name="requestModel"></param>
        /// <returns>response model</returns>
        [HttpPatch("{id}")]
        public IActionResult Patch(int id, RequestModel requestModel)
        {
            if (!inMemoryStore.ContainsKey(id))
            {
                _logger.LogWarning($"PATCH request failed. Record with ID={id} not found.");
                return NotFound(new { message = "Record not found" });
            }

            var existingRecord = inMemoryStore[id];

            if (!string.IsNullOrWhiteSpace(requestModel.Key))
                existingRecord.Key = requestModel.Key;

            if (!string.IsNullOrWhiteSpace(requestModel.Value))
                existingRecord.Value = requestModel.Value;

            inMemoryStore[id] = existingRecord;
            _logger.LogInformation($"Record patched: ID={id}, Key={existingRecord.Key}, Value={existingRecord.Value}");

            return Ok(new ResponseModel<string>
            {
                success = true,
                message = "Record patched successfully",
                data = $"ID: {id}, Key: {existingRecord.Key}, Value: {existingRecord.Value}"
            });
        }

       /// <summary>
       /// Delete method to delete a record by id
       /// </summary>
       /// <param name="id"></param>
       /// <returns></returns>
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            if (!inMemoryStore.ContainsKey(id))
            {
                _logger.LogWarning($"DELETE request failed. Record with ID={id} not found.");
                return NotFound(new { message = "Record not found" });
            }

            inMemoryStore.Remove(id);
            _logger.LogInformation($"Record deleted: ID={id}");

            return Ok(new ResponseModel<string>
            {
                success = true,
                message = "Record deleted successfully",
                data = $"Deleted ID: {id}"
            });
        }
    }
}
