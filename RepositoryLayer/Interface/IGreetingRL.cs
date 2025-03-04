using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ModelLayer.Model;
using RepositoryLayer.Entity;

namespace RepositoryLayer.Interface
{
    public interface IGreetingRL
    {
        void SaveGreeting(GreetingModel greeting);//UC4
        GreetingModel GetGreetingById(int id);//UC5
        List<GreetingModel> GetAllGreetings(); //UC6
    }
}
