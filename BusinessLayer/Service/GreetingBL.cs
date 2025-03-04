using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RepositoryLayer.Interface;
using BusinessLayer.Interface;
using Microsoft.Extensions.Logging;
using ModelLayer.Model;

namespace BusinessLayer.Service
{
    public class GreetingBL :IGreetingBL
    {
        private readonly IGreetingRL greetingRL;
        private readonly ILogger<GreetingBL> logger;
        public GreetingBL(IGreetingRL greetingRL,ILogger<GreetingBL> logger)
        {
            this.greetingRL = greetingRL;
            this.logger = logger;
        }

        //UC2 : returns Greeting message
        public string HelloGreeting()
        {
            return "Greeting Message:Hello ji!";
        }

        //UC4 : save greeting in database
        public void SaveGreeting(GreetingModel greetingModel)
        {
            if (greetingModel == null || string.IsNullOrEmpty(greetingModel.Message))
            {
                logger.LogError("Invalid greeting data provided.");
                throw new ArgumentException("Greeting message cannot be empty.");
            }

            logger.LogInformation($"Saving Greeting: {greetingModel.Message}");
            greetingRL.SaveGreeting(greetingModel);
        }

        //UC5 :Fetches a greeting message by its ID.
        public GreetingModel GetGreetingById(int id)
        {
            logger.LogInformation("BL: Fetching greeting with ID: {Id}", id);
            return greetingRL.GetGreetingById(id);
        }

    }
}
