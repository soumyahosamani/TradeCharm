using FeedSimulation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace TradeCharm
{
    class Program
    {
        static void Main(string[] args)
        {
            ExecuteSecondImplementation();
        }

        static void ExecuteFistImplementation()
        {
            DataFeed dataFeed = new DataFeed();
            dataFeed.Subscribe(new Strategy("Strategy1"));
            dataFeed.Subscribe(new Strategy("Strategy2"));
            dataFeed.Start();
            Console.WriteLine("End");
            Console.ReadKey();
            dataFeed.Stop();
        }

        static void ExecuteSecondImplementation()
        {           
            TradeSimulator.DataFeed datafeed = new TradeSimulator.DataFeed(new TradeSimulator.FeedProvider(), new TradeSimulator.FeedQueue<TradeSimulator.Tick>());
            datafeed.Subscribe(new TradeSimulator.PrintStrategy("Print Strategy"));
            datafeed.Start();
            Console.WriteLine("End");
            Console.ReadKey();
            datafeed.Stop();
        }
    }
}
