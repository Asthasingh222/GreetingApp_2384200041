using Microsoft.AspNetCore.Mvc;
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

        private static Dictionary<int, RequestModel> inMemoryStore = new Dictionary<int, RequestModel>
        {
            { 1, new RequestModel { Key = "Greeting", Value = "Hello, World!" } },
            { 2, new RequestModel { Key = "Farewell", Value = "Goodbye!" } },
            { 3, new RequestModel { Key = "Welcome", Value = "Welcome to the API!" } }
        };

        public HelloGreetingController(ILogger<HelloGreetingController> logger)
        {
            _logger = logger;
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
