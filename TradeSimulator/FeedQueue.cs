using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TradeSimulator
{
    public class FeedQueue<T>
    {
        /*
        private Queue<T> feed = new Queue<T>();

        public void AddFeed(T item)
        {
            lock (feed)
            {
                feed.Enqueue(item);
            }

            //tickAdded.Set();
        }

        public T GetTick()
        {
            lock (feed)
            {
                if (feed.Count == 0)
                    return default(T);
                return feed.Dequeue();
            }
        }

        */

        private BlockingCollection<T> feed = new BlockingCollection<T>();

        public void AddFeed(T item)
        {
            feed.Add(item);
        }

        public T GetTick()
        {
            return  feed.Take();
        }
    }
}
