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
        public string HelloGreeting()
        {
            return "Greeting Message:Hello ji!";
        }
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

    }
}
