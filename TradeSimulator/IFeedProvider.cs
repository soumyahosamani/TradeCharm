
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TradeSimulator
{
    public delegate void NewTickEventHandler(object sender, TickEventArgs e);
    public interface IFeedProvider 
    {
        event NewTickEventHandler NewTickEvent;

        void Subscribe(string Symbol);
        void Start();
        void Stop();       
    }
    
}
