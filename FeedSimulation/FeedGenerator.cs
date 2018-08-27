using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace FeedSimulation
{
   public class FeedGenerator
    {
        private int TickId = 1;
        public FeedGenerator(DataFeed feed)
        {
            Feed = feed;
        }
        public void GenerateFeed()
        {
            while (true)
            {
                var randomNumber = 1000;// Randomizer.GetRandomNumber();
                Console.WriteLine("Thread {0} : Generator sleeping for {1} Tick id {2}", Thread.CurrentThread.ManagedThreadId, randomNumber, TickId);
                Thread.Sleep(randomNumber);
                Feed.AddFeed(new Tick(TickId, randomNumber, DateTime.Now));
                TickId++;
            }
        }

        private DataFeed Feed { get; set; }
    }
}
