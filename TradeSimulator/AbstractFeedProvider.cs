
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace TradeSimulator
{
    public abstract class AbstractFeedProvider : IFeedProvider
    {
        private IList<string> subscribedSymbols = new List<string>();
        protected IList<Thread> processThreads = new List<Thread>();

        public event NewTickEventHandler NewTickEvent;

        public void Start()
        {
            CreateProcesThreads();
            foreach (var thread in ProcesThreads)
            {
                thread.Start();
            }

        }

        public void Stop()
        {
            foreach (var thread in ProcesThreads)
            {
                thread.Abort();
            }
        }

        protected IList<Thread> ProcesThreads
        {
            get
            {
                return processThreads;
            }
        }

        protected IList<string> SubscribedSymbols
        {
            get
            {
                return subscribedSymbols;
            }
        }

        public void Subscribe(string symbol)
        {
            if (subscribedSymbols.Contains(symbol) == false)
                subscribedSymbols.Add(symbol);
        }        

        protected  void RaiseNewTickEvent(Tick tick)
        {
            NewTickEvent?.Invoke(this, new TickEventArgs(tick));
        }

        protected abstract void CreateProcesThreads();
    }
}
