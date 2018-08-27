using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace FeedSimulation
{
    public interface IStrategy
    {
        string Name { get; }
        void OnTick(Tick tick);
    }

    public interface IFeedGenerator
    {
        DataFeed Feed { get; }
        void GenerateFeed();
    }
    
    public class DataFeed
    {
        private Queue<Tick> feed = new Queue<Tick>();
        private IList<Strategy> subscribers = new List<Strategy>();
        private AutoResetEvent tickAdded = new AutoResetEvent(false);
        private Thread GeneratorThread;
        private Thread ProcessThread;
        private int previousTickId = 0;

        public void Start()
        {
               var generator = new FeedGenerator(this);
                GeneratorThread = new Thread(generator.GenerateFeed);
                GeneratorThread.Name = "Generator";
                GeneratorThread.IsBackground = false;
                Console.WriteLine("Starting data feed generation");
                GeneratorThread.Start();

                ProcessThread = new Thread(ProcessQueue);
                ProcessThread.Name = "Processor";
                ProcessThread.IsBackground = false;
                Console.WriteLine("Starting Processing feed ");
                ProcessThread.Start();
            
        }

        public void Stop()
        {
            Console.WriteLine("Stopping Generator thread");
            GeneratorThread.Abort();

            Console.WriteLine("Stopping Processing thread");
            ProcessThread.Abort();
        }

        public void Subscribe(Strategy strategy)
        {
            if(subscribers.Contains(strategy) == false)
            subscribers.Add(strategy);
        } 
        
        private void ProcessQueue()
        {            
            while(true)
            {
                Tick tick = GetNextTick();
                Console.WriteLine("Thread {0}: Processing Tick {1}", Thread.CurrentThread.ManagedThreadId, tick.Id);

                // check if any tick is lost
                if ((tick.Id -1) != previousTickId)
                {
                    Console.WriteLine("Thread {2}: Lost ticks : PreviousId {0} CurrentId {1}", previousTickId, tick.Id,Thread.CurrentThread.ManagedThreadId);
                }

                foreach (var subscriber in subscribers)
                {
                    try
                    {
                        subscriber.OnTick(tick);
                    }
                    catch(Exception e)
                    {
                        Console.WriteLine("Thread {2}: Subscriber {0} exception {1}", subscriber.Name, e.Message, Thread.CurrentThread.ManagedThreadId);
                        throw e;
                    }                    
                }

                previousTickId = tick.Id;
            }
        }

        private Tick GetNextTick()
        {            
            Tick tick = GetTick();
            if (tick == null)
            {
                // wait while Q IS EMTPY
                tickAdded.WaitOne();

                // get a tick 
                tick = GetTick();
            }           
            
            return tick;            
        }      

        internal void AddFeed(Tick tick)
        {
            lock (feed)
            {
                feed.Enqueue(tick);
                tickAdded.Set();
            }
        }

        private Tick GetTick()
        {
            lock (feed)
            {
                if (feed.Count == 0)
                    return null;
                return feed.Dequeue();
            }
        }
    }
}
