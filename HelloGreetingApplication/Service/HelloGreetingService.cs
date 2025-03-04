using HelloGreetingApplication.Interface;

namespace HelloGreetingApplication.Service
{
    public class HelloGreetingService :IHelloGreetingService
    {
        public string HelloGreeting()
        {
            return "Greeting Message: Hello world";
        }
    }
}
