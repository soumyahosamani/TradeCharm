using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TradeSimulator;
using System.Collections.Concurrent;
using Moq;
using System.Threading;
using System.Collections.Generic;

namespace TradeAutomationTests
{
    [TestClass]
    public class TradeSimulationTests
    {
        [TestMethod]
        public void TradeAutomationTest()
        {
            // setup test data
            string symbol = "INFY";
            var tick = new Tick(1, symbol, 1.0, DateTime.Now);

            // set up dependencies
            var strategyMock = new Mock<IStrategy>();
            strategyMock.Setup(s => s.Name).Returns("Test");
            strategyMock.Setup(s => s.Symbol).Returns(symbol);
            strategyMock.Setup(s => s.OnTick(tick)).Verifiable();

            var feedProviderMock = new Mock<IFeedProvider>();
            feedProviderMock.Setup(f => f.Start()).Verifiable();
            feedProviderMock.Setup(f => f.Stop()).Verifiable();
            feedProviderMock.Setup(f => f.Subscribe(symbol)).Verifiable();

            using (DataFeed feed = new DataFeed(feedProviderMock.Object))
            {
                // test
                // subscribe to feed
                feed.Subscribe(strategyMock.Object);
                feedProviderMock.Verify(f => f.Subscribe(symbol));

                // raise new tick event in feed provider
                feedProviderMock.Raise(f => f.NewTickEvent += null, this, new TickEventArgs(tick));

                // verify if on tick was called            
                strategyMock.Verify();
            }
        }

        [TestMethod]
        public void SingleValidStrategy()
        {
            // set up data
            string symbol = "SYMBOL";
            Tick tick = new Tick(1, symbol, 0.1, DateTime.Now);

            // set up dependencies
            var strategyMock = new Mock<IStrategy>();
            strategyMock.Setup(s => s.Name).Returns("Test");
            strategyMock.Setup(s => s.Symbol).Returns(symbol);
            strategyMock.Setup(s => s.OnTick(tick)).Verifiable();

            var feedProviderMock = new Mock<IFeedProvider>();
            feedProviderMock.Setup(f => f.Subscribe(symbol)).Verifiable();

            // main class under test
            using (DataFeed feed = new DataFeed(feedProviderMock.Object))
            {

                // test
                // subscribe to feed
                feed.Subscribe(strategyMock.Object);
                feedProviderMock.Verify(f => f.Subscribe(symbol));


                // raise new tick event in feed provider
                feedProviderMock.Raise(f => f.NewTickEvent += null, this, new TickEventArgs(tick));

                // verify if on tick was called            
                strategyMock.Verify();
            }
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException), null)]
        public void SingleInValidSymbolStrategy()
        {
            // set up data
            string symbol = "SYMBOL";
            Tick tick = new Tick(1, symbol, 0.1, DateTime.Now);

            // set up dependencies
            var strategyMock = new Mock<IStrategy>();
            strategyMock.Setup(s => s.Name).Returns("Test");
            strategyMock.Setup(s => s.Symbol).Returns(default(string));
            strategyMock.Setup(s => s.OnTick(tick)).Verifiable();

            var feedProviderMock = new Mock<IFeedProvider>();
            feedProviderMock.Setup(f => f.Subscribe(symbol)).Verifiable();

            // main class under test
            using (DataFeed feed = new DataFeed(feedProviderMock.Object))
            {

                // test
                // subscribe to feed
                feed.Subscribe(strategyMock.Object);
            }
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException), null)]
        public void SingleInValidNameStrategy()
        {
            // set up data
            string symbol = "SYMBOL";
            Tick tick = new Tick(1, symbol, 0.1, DateTime.Now);

            // set up dependencies
            var strategyMock = new Mock<IStrategy>();
            strategyMock.Setup(s => s.Name).Returns(default(string));
            strategyMock.Setup(s => s.Symbol).Returns(symbol);
            strategyMock.Setup(s => s.OnTick(tick)).Verifiable();

            var feedProviderMock = new Mock<IFeedProvider>();
            feedProviderMock.Setup(f => f.Subscribe(symbol)).Verifiable();

            // main class under test
            using (DataFeed feed = new DataFeed(feedProviderMock.Object))
            {


                // test
                // subscribe to feed
                feed.Subscribe(strategyMock.Object);
            }
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException), null)] // exceptoin or ignore?
        public void DuplicateStrategy()
        {
            // set up data
            string symbol = "SYMBOL";
            Tick tick = new Tick(1, symbol, 0.1, DateTime.Now);

            // set up dependencies
            var strategyMock = new Mock<IStrategy>();
            strategyMock.Setup(s => s.Name).Returns("Test");
            strategyMock.Setup(s => s.Symbol).Returns(symbol);
            strategyMock.Setup(s => s.OnTick(tick)).Verifiable();

            var feedProviderMock = new Mock<IFeedProvider>();
            feedProviderMock.Setup(f => f.Subscribe(symbol)).Verifiable();

            // main class under test
            using (DataFeed feed = new DataFeed(feedProviderMock.Object))
            {


                // test
                // subscribe to feed
                feed.Subscribe(strategyMock.Object);

                // test for duplicate entry
                feed.Subscribe(strategyMock.Object);
            }
        }

        [TestMethod]
        public void MultipleStrategySameSymbol()
        {
            // set up data
            string symbol1 = "SYMBOL";
            Tick tick1 = new Tick(1, symbol1, 0.1, DateTime.Now);


            // set up dependencies
            var strategy1 = new Mock<IStrategy>();
            strategy1.Setup(s => s.Name).Returns("strategy1");
            strategy1.Setup(s => s.Symbol).Returns(symbol1);
            strategy1.Setup(s => s.OnTick(tick1)).Verifiable();

            var strategy2 = new Mock<IStrategy>();
            strategy2.Setup(s => s.Name).Returns("strategy2");
            strategy2.Setup(s => s.Symbol).Returns(symbol1);
            strategy2.Setup(s => s.OnTick(tick1)).Verifiable();

            var feedProviderMock = new Mock<IFeedProvider>();
            feedProviderMock.Setup(f => f.Subscribe(symbol1)).Verifiable();
            feedProviderMock.Setup(f => f.Subscribe(symbol1)).Verifiable();

            // main class under test
            using (DataFeed feed = new DataFeed(feedProviderMock.Object))
            {


                // test
                // subscribe to feed
                feed.Subscribe(strategy1.Object);
                feedProviderMock.Verify(f => f.Subscribe(symbol1));

                feed.Subscribe(strategy2.Object);
                feedProviderMock.Verify(f => f.Subscribe(symbol1));


                // raise new tick event in feed provider
                feedProviderMock.Raise(f => f.NewTickEvent += null, this, new TickEventArgs(tick1));

                // verify if on tick was called            
                strategy1.Verify();

                // verify if on tick was called            
                strategy2.Verify();
            }




        }

        [TestMethod]
        public void MultipleStrategyDifferentSymbol()
        {
            // set up data
            string symbol1 = "SYMBOL1";
            string symbol2 = "SYMBOL2";
            Tick tick1 = new Tick(1, symbol1, 0.1, DateTime.Now);
            Tick tick2 = new Tick(2, symbol2, 0.1, DateTime.Now);

            // set up dependencies
            var strategy1 = new Mock<IStrategy>();
            strategy1.Setup(s => s.Name).Returns("strategy1");
            strategy1.Setup(s => s.Symbol).Returns(symbol1);
            strategy1.Setup(s => s.OnTick(tick1)).Verifiable();

            var strategy2 = new Mock<IStrategy>();
            strategy2.Setup(s => s.Name).Returns("strategy2");
            strategy2.Setup(s => s.Symbol).Returns(symbol2);
            strategy2.Setup(s => s.OnTick(tick2)).Verifiable();

            var feedProviderMock = new Mock<IFeedProvider>();
            feedProviderMock.Setup(f => f.Subscribe(symbol1)).Verifiable();
            feedProviderMock.Setup(f => f.Subscribe(symbol2)).Verifiable();

            // main class under test
            using (DataFeed feed = new DataFeed(feedProviderMock.Object))
            {

                // test
                // subscribe to feed
                feed.Subscribe(strategy1.Object);
                feedProviderMock.Verify(f => f.Subscribe(symbol1));

                feed.Subscribe(strategy2.Object);
                feedProviderMock.Verify(f => f.Subscribe(symbol2));


                // raise new tick event in feed provider
                feedProviderMock.Raise(f => f.NewTickEvent += null, this, new TickEventArgs(tick1));

                // verify if on tick was called            
                strategy1.Verify();

                // raise new tick event in feed provider
                feedProviderMock.Raise(f => f.NewTickEvent += null, this, new TickEventArgs(tick2));

                // verify if on tick was called            
                strategy2.Verify();
            }
        }

        [TestMethod]
        public void SubscribeWithDelay()
        {
            // set up data
            string symbol = "SYMBOL";
            Tick tick = new Tick(1, symbol, 0.1, DateTime.Now);

            // set up dependencies
            var strategyMock = new Mock<IStrategy>();
            strategyMock.Setup(s => s.Name).Returns(default(string));
            strategyMock.Setup(s => s.Symbol).Returns(symbol);
            strategyMock.Setup(s => s.OnTick(tick)).Verifiable();

            var feedProviderMock = new Mock<IFeedProvider>();
            feedProviderMock.Setup(f => f.Subscribe(symbol)).Verifiable();

            // main class under test
            using (DataFeed feed = new DataFeed(feedProviderMock.Object))
            {
                Thread.Sleep(1000);

                // test
                // subscribe to feed
                feed.Subscribe(strategyMock.Object);

                feedProviderMock.Verify(f => f.Subscribe(symbol));

                // raise new tick event in feed provider
                feedProviderMock.Raise(f => f.NewTickEvent += null, this, new TickEventArgs(tick));

                // verify if on tick was called            
                strategyMock.Verify();
            }
        }

        [TestMethod]
        public void SubscrbieAfterTick()
        {
            // set up data
            string symbol1 = "SYMBOL";
            Tick tick1 = new Tick(1, symbol1, 0.1, DateTime.Now);


            // set up dependencies
            var strategy1 = new Mock<IStrategy>();
            strategy1.Setup(s => s.Name).Returns("strategy1");
            strategy1.Setup(s => s.Symbol).Returns(symbol1);
            strategy1.Setup(s => s.OnTick(tick1)).Verifiable();

            var strategy2 = new Mock<IStrategy>();
            strategy2.Setup(s => s.Name).Returns("strategy2");
            strategy2.Setup(s => s.Symbol).Returns(symbol1);
            strategy2.Setup(s => s.OnTick(tick1)).Verifiable();

            var feedProviderMock = new Mock<IFeedProvider>();
            feedProviderMock.Setup(f => f.Subscribe(symbol1)).Verifiable();

            // main class under test
            using (DataFeed feed = new DataFeed(feedProviderMock.Object))
            {
                // test
                // subscribe to feed
                feed.Subscribe(strategy1.Object);
                feedProviderMock.Verify(f => f.Subscribe(symbol1));


                // raise new tick event in feed provider
                feedProviderMock.Raise(f => f.NewTickEvent += null, this, new TickEventArgs(tick1));

                // verify if on tick was called            
                strategy1.Verify();

                feed.Subscribe(strategy2.Object);
                feedProviderMock.Verify(f => f.Subscribe(symbol1));

                // verify if on tick was called            
                strategy2.Verify(s => s.OnTick(tick1), Times.Never());
            }
        }

        [TestMethod]
        public void SubscrbieToInvalidCompany()
        {
            // set up data
            string symbol1 = "SYMBOL";
            Tick tick1 = new Tick(1, symbol1, 0.1, DateTime.Now);


            // set up dependencies
            var strategy1 = new Mock<IStrategy>();
            strategy1.Setup(s => s.Name).Returns("strategy1");
            strategy1.Setup(s => s.Symbol).Returns(symbol1);
            strategy1.Setup(s => s.OnTick(tick1)).Verifiable();



            var feedProviderMock = new Mock<IFeedProvider>();
            feedProviderMock.Setup(f => f.Subscribe(symbol1)).Verifiable();

            // main class under test
            using (DataFeed feed = new DataFeed(feedProviderMock.Object))
            {


                // test
                // subscribe to feed
                feed.Subscribe(strategy1.Object);
                feedProviderMock.Verify(f => f.Subscribe(symbol1));

                // doest feel right review            

                // verify if on tick was called            
                strategy1.Verify(s => s.OnTick(tick1), Times.Never());
            }
        } // need to be revwied 


        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException), null)]
        public void InvalidFeedProviderInput()
        {
            // set up data
            string symbol = "SYMBOL";
            Tick tick = new Tick(1, symbol, 0.1, DateTime.Now);

            // set up dependencies
            var strategyMock = new Mock<IStrategy>();
            strategyMock.Setup(s => s.Name).Returns("Test");
            strategyMock.Setup(s => s.Symbol).Returns(default(string));
            strategyMock.Setup(s => s.OnTick(tick)).Verifiable();

            // main class under test
            DataFeed feed = new DataFeed(null);
        }

        [TestMethod]
        public void SubscribeWithMultipleThreads()
        {
            // set up data
            string symbol = "SYMBOL";
            Tick tick = new Tick(1, symbol, 0.1, DateTime.Now);
            var strategies = new List<Mock<IStrategy>> () ;
            for(int i =1; i < 11; i++)
            {
                var strategy = new Mock<IStrategy>();
                strategy.Setup(s => s.Name).Returns("Strategy " + i);
                strategy.Setup(s => s.Symbol).Returns(symbol);
                strategy.Setup(s => s.OnTick(tick)).Verifiable();
                strategies.Add(strategy);
            }

        }

        private void Subscribe(DataFeed dataFeed, IStrategy strategy)
        {
            dataFeed.Subscribe(strategy);
        }
    }
}
