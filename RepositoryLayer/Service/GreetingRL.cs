using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ModelLayer.Model;
using Microsoft.Extensions.Logging;
using RepositoryLayer.Content;
using RepositoryLayer.Entity;
using RepositoryLayer.Interface;
using NLog;
using Microsoft.EntityFrameworkCore;

namespace RepositoryLayer.Service
{
    public class GreetingRL :IGreetingRL
    {
        private readonly GreetingDbContext dbContext;
        private readonly ILogger<GreetingRL> _logger;
        public GreetingRL(GreetingDbContext dbContext, ILogger<GreetingRL> logger)
        {
            this.dbContext = dbContext;
            this._logger = logger;
        }

        // UC4 : Save greeting to the database
        public void SaveGreeting(GreetingModel greeting)
        {
            try
            {
                _logger.LogInformation("Saving new greeting to the database.");

                if (string.IsNullOrEmpty(greeting.Message))
                {
                    throw new ArgumentException("Greeting message cannot be empty.");
                }

                // Create new greeting entity
                var newGreeting = new GreetingEntity
                {
                    GreetingMessage = greeting.Message,
                };

                // Add to database and save changes
                dbContext.Greetings.Add(newGreeting);
                dbContext.SaveChanges();

                _logger.LogInformation("Greeting saved successfully.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while saving the greeting.");
                throw;
            }
        }

        //UC5 :Retrieves a greeting message by its ID.
        public GreetingModel GetGreetingById(int id)
        {
            _logger.LogInformation("Fetching greeting with ID: {Id}", id);
            var entity = dbContext.Greetings.Find(id);
            if (entity == null)
            {
                _logger.LogWarning("Greeting with ID {Id} not found.", id);
                return null;
            }
            return new GreetingModel { Id = entity.Id, Message = entity.GreetingMessage };
        }

        //UC6 :Retrieves all greeting messages from the database
        public List<GreetingModel> GetAllGreetings()
        {
            _logger.LogInformation("Fetching all greetings from the database.");
            return dbContext.Greetings
                .Select(g => new GreetingModel { Id = g.Id, Message = g.GreetingMessage })
                .ToList();
        }

        //UC7: Updates an existing greeting message in the database
        public bool UpdateGreeting(int id, GreetingModel greeting)
        {
            var existingGreeting = dbContext.Greetings.Find(id);
            if (existingGreeting == null)
            {
                _logger.LogWarning("RL: Greeting with ID {Id} not found.", id);
                return false;
            }

            existingGreeting.GreetingMessage = greeting.Message;
            dbContext.SaveChanges();

            _logger.LogInformation("RL: Greeting with ID {Id} updated successfully.", id);
            return true;
        }

        //UC8: Deletes a greeting message from the database
        public bool DeleteGreeting(int id)
        {
            _logger.LogInformation("Attempting to delete greeting with ID: {Id}", id);
            var entity = dbContext.Greetings.Find(id);
            if (entity == null)
            {
                _logger.LogWarning("Greeting with ID {Id} not found for deletion.", id);
                return false;
            }

            dbContext.Greetings.Remove(entity);
            dbContext.SaveChanges();
            _logger.LogInformation("Greeting with ID {Id} deleted successfully.", id);
            return true;
        }

    }
}
