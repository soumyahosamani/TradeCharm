﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace TradeSimulator
{
    public class DataFeed :IDisposable
    {
        private FeedQueue<Tick> queue;
        private IList<IStrategy> subscribers = new List<IStrategy>();
        //private AutoResetEvent tickAdded = new AutoResetEvent(false);
        private Thread processThread;
        private int previousTickId = 0;

        private bool started = false;

        public DataFeed(IFeedProvider feedProvider)
        {
            if (feedProvider == null)
                throw new ArgumentNullException("Feed Provider is invalid");
            FeedQueue<Tick> feedQueue = new FeedQueue<Tick>();
            queue = feedQueue;
            this.FeedProvider = feedProvider;
            FeedProvider.NewTickEvent += OnNewTick;

            lock (this)
            {
                Start();
                started = true;
            }
            
        }       

        public IFeedProvider FeedProvider { get; private set; }
        public void Subscribe(IStrategy strategy)
        {
            if (subscribers.Contains(strategy) == false)
            {
                if (strategy.Name == null)
                {
                    throw new ArgumentNullException("Name", "Name is not provided for Strategy" );
                }
                if (strategy.Symbol == null)
                {
                    throw new ArgumentNullException("Symbol","Symbol is not provided for Strategy" + strategy.Name);
                }
                subscribers.Add(strategy);
                FeedProvider.Subscribe(strategy.Symbol);
            }
        }

        private void Start()
        {
            // start feed stream
            /* so here i should know which symbol feeds to start */
            FeedProvider.Start();
            StartProcessTicks();
        }

        private void Stop()
        {
            FeedProvider.Stop();
            Console.WriteLine("Stopping Processing thread");
            processThread.Abort();
        }

        private void OnNewTick(object sender, TickEventArgs e)
        {
            queue.AddFeed(e.Tick);
            //tickAdded.Set();            
        }


        private Tick GetNextTick()
        {
            Tick tick;

            tick = queue.GetTick();

            // wait while Q IS EMTPY
            //while ((tick = queue.GetTick()) == null)
            //tickAdded.WaitOne();

            return tick;
        }

        private void ProcessTicks()
        {
            while (true)
            {
                Tick tick = GetNextTick();
                Console.WriteLine("Thread {0}: Processing Tick {1}", Thread.CurrentThread.ManagedThreadId, tick.Id);

                // check if any tick is lost
                if ((tick.Id - 1) != previousTickId)
                {
                    Console.WriteLine("Thread {2}: Lost ticks : PreviousId {0} CurrentId {1}", previousTickId, tick.Id, Thread.CurrentThread.ManagedThreadId);
                }

                foreach (var subscriber in subscribers)
                {
                    try
                    {
                        if (subscriber.Symbol == tick.Symbol )
                            subscriber.OnTick(tick);
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine("Thread {2}: Subscriber {0} exception {1}", subscriber.Name, e.Message, Thread.CurrentThread.ManagedThreadId);
                        throw e;
                    }
                }

                previousTickId = tick.Id;
            }
        }

        private void StartProcessTicks()
        {
            processThread = new Thread(ProcessTicks);
            processThread.IsBackground = false;
            Console.WriteLine("Starting Processing feed ");
            processThread.Start();
        }

        public void Dispose()
        {
            lock (this)
            {
                if (started)
                    Stop();
            }
        }
    }

}
