using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;

namespace TradeSimulator
{
    public class CSVFeedProvider : IFeedProvider
    {
        private string baseFolder;        
        
        public CSVFeedProvider(string symbol)
        {
            Symbol = symbol;
            baseFolder = ConfigurationSettings.AppSettings["CsvProviderFolder"];
        }

        public event NewTickEventHandler NewTickEvent;        

        public void Start(string symbol)
        {
            throw new NotImplementedException();
        }

        public void Stop(string symbol)
        {
            throw new NotImplementedException();
        }

        
    }

    
}
