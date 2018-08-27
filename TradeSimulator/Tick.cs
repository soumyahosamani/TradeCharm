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

        public Tick(int id, double price, DateTime time)
        {
            Price = price;
            Time = time;
            Id = id;
        }

        public override string ToString()
        {
            return string.Format("{0}: {1} {2} ", Time, Price, Id);
        }
    }
}
