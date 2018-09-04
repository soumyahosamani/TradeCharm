using FeedSimulation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using YahooFinance;
using TradeSimulator;

namespace TradeCharm
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Welcome");
            //Sample sample = new Sample();            sample.GetTick();
            //ExecuteSecondImplementation();
            ExecuteCsvImplementation();
        }

        static void ExecuteFistImplementation()
        {
            FeedSimulation.DataFeed dataFeed = new FeedSimulation.DataFeed();
            dataFeed.Subscribe(new Strategy("Strategy1"));
            dataFeed.Subscribe(new Strategy("Strategy2"));
            dataFeed.Start();
            Console.WriteLine("End");
            Console.ReadKey();
            dataFeed.Stop();
        }

        //static void ExecuteSecondImplementation()
        //{
        //    var symbol = "INFY";
        //    TradeSimulator.DataFeed datafeed = new TradeSimulator.DataFeed(new FeedProvider(), new FeedQueue<TradeSimulator.Tick>());
        //    datafeed.Subscribe(new PrintStrategy("Print Strategy", symbol));
        //    datafeed.Start();
        //    Console.WriteLine("End");
        //    Console.ReadKey();
        //    datafeed.Stop();
        //}

        static void ExecuteCsvImplementation()
        {
            var symbol = "INFY";
            TradeSimulator.DataFeed datafeed = new TradeSimulator.DataFeed(new CSVFeedProvider());
            datafeed.Subscribe(new PrintStrategy("Print Strategy", symbol));
            //datafeed.Start();
            Console.WriteLine("End");
            Console.ReadKey();
            //datafeed.Stop();

        }
    }
}
