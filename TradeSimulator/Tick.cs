using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TradeSimulator
{
  public  class Tick
    {
        public double Price;
        public DateTime Time;
        public int Id;    

        public Tick(int id, string symbol, double price, DateTime time)
        {
            Price = price;
            Time = time;
            Id = id;
            Symbol = symbol; 
        }

        public string Symbol  { get; private set; }

        public override string ToString()
        {
            return string.Format("{0}: {3} {1} {2} ", Time, Price, Id, Symbol);
        }
    }
}
