using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ModelLayer.Model;

namespace BusinessLayer.Interface
{
    public interface IGreetingBL
    {
        //UC2
        string HelloGreeting();

        //UC4
        void SaveGreeting(GreetingModel greeting);

        //UC5
        GreetingModel GetGreetingById(int id);

        //UC6
        List<GreetingModel> GetAllGreetings();

        //UC7
        bool UpdateGreeting(int id, GreetingModel greeting);
    }
}
