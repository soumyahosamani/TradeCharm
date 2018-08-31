using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TradeSimulator;
using System.Collections.Concurrent;
namespace TradeAutomationTests
{


    [TestClass]
    public class TradeSimulationTests
    {
        [TestMethod]
        public void ExecuteStrategyTest()
        {
            // create strategy
            IStrategy testStrategy = new TestStrategy() { Name = "Test", Symbol = "Symbol" };

            // create datafeed
            IFeedProvider feedProvider = new TestFeedProvider();

            DataFeed feed = new DataFeed(feedProvider, new FeedQueue<Tick>());
            feed.Start();

            // subscribe to feed
            feed.Subscribe(testStrategy);

            // ensure strategy is running.. ??? 
            var purchasedTicks = testStrategy.BoughtTicks. ///i need this 


            feed.Stop();                        
        }
    }

    public class TestStrategy : IStrategy
    {
         public string Name
        {
            get; set;
        }

        public string Symbol
        {
            get; set;
        }

        public void OnTick(Tick tick)
        {
            if (tick.Price > 1)
                Buy(tick);
        }

        
    }

    public class TestFeedProvider : IFeedProvider
    {
        public event NewTickEventHandler NewTickEvent;

        public void Start()
        {
            throw new NotImplementedException();
        }

        public void Stop()
        {
            throw new NotImplementedException();
        }

        public void Subscribe(string Symbol)
        {
            throw new NotImplementedException();
        }
    }
}
