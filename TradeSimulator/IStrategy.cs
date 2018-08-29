using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TradeSimulator
{
   public interface IStrategy
    {
        string Name { get; }
        string Symbol { get; }
        void OnTick(Tick tick);
    }
}
