using System;

namespace TradeSimulator
{
    public class TickEventArgs: EventArgs
    {
        public TickEventArgs(Tick tick)
        {
            Tick = tick;
        }
        public Tick Tick { get; private set; }
    }
}