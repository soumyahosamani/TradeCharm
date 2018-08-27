using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Utility;

namespace TradeSimulator
{
    public class FeedProvider : IFeedProvider
    {
        private Thread feedProviderThread;
        
        private int TickId = 1, ctr = 0;

        public event NewTickEventHandler NewTickEvent;

        public void Start()
        {
            feedProviderThread = new Thread(ProcessTicks);
            feedProviderThread.IsBackground = false;
            Console.WriteLine("Starting feed stream");
            feedProviderThread.Start();
        }

        public void Stop()
        {
            Console.WriteLine("Stopping feed stream");
            feedProviderThread.Abort();
        }        

        private void RaiseNewTickEvent(Tick tick)
        {
            NewTickEvent?.Invoke(this, new TickEventArgs(tick));
        }

        private void ProcessTicks()
        {
            while (true)
            {
                var randomNumber = Randomizer.GetRandomNumber();
                Console.WriteLine("Thread {0} : Generator sleeping for {1} Tick id {2}", Thread.CurrentThread.ManagedThreadId, randomNumber, TickId);
                //Thread.Sleep(randomNumber);
                Thread.Sleep(1000);
                ctr++;
                if (ctr == 5)
                    Thread.Sleep(10000);
                RaiseNewTickEvent(new Tick(TickId, randomNumber, DateTime.Now));
                TickId++;
            }
        }
    }
}
