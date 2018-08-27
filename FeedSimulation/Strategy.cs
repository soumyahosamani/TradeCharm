using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace FeedSimulation
{
    public class Strategy
    {
        public Strategy(string name)
        {
            Name = name;
        }
        public string Name
        {
            get; private set;
        }

        public string Description
        {
            get
            {
                return "Prints the tick";
            }
        }

        public void OnTick(Tick tick)
        {
            var randomNumber = Randomizer.GetRandomNumber() /2;
            Console.WriteLine(Name + "sleeping for " + randomNumber);
            Thread.Sleep(randomNumber);
            Console.WriteLine("Thread {0} : Strategy {1} : {2}", Thread.CurrentThread.ManagedThreadId, Name, tick );
        }
    }
}
