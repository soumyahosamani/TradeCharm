using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Utility;

namespace TradeSimulator
{
    public class FeedProvider : AbstractFeedProvider
    {
        
        protected override void CreateProcesThreads()
        {
            foreach (var symbol in SubscribedSymbols)
            {
                var feedProviderThread = new Thread(() => GenerateTicks(symbol));
                feedProviderThread.Name = "Gen_" + symbol;
                feedProviderThread.IsBackground = false;
                ProcesThreads.Add(feedProviderThread);
            }
        }

        private void GenerateTicks(string symbol)
        {
            int TickId = 1, ctr = 0;
            while (true)
            {
                var randomNumber = Randomizer.GetRandomNumber();
                Console.WriteLine("Thread {0} : Generator sleeping for {1} Tick id {2}", Thread.CurrentThread.ManagedThreadId, randomNumber, TickId);
                //Thread.Sleep(randomNumber);
                Thread.Sleep(1000);
                ctr++;
                if (ctr == 5)
                    Thread.Sleep(10000);
                RaiseNewTickEvent(new Tick(TickId, symbol, randomNumber, DateTime.Now));
                TickId++;
            }
        }
    }
}
