using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using System.IO;

namespace TradeSimulator
{
    public class CSVFeedProvider : IFeedProvider
    {
        private string baseFolder;
        private IList<string> symbols = new List<string> {"INFY", "WIPRO", "MARUTI" };
        private IList<string> filePaths = new List<string>();
        
        public CSVFeedProvider()
        {
            LoadFilePaths();            
        }

        private void LoadFilePaths()
        {
            baseFolder = ConfigurationSettings.AppSettings["CsvProviderFolder"];
            foreach (var symbol in symbols)
            {
                filePaths.Add(Path.Combine(baseFolder, symbol + ".csv"));
                              
            }
        }

        public event NewTickEventHandler NewTickEvent;        

        public void Start()
        {
            throw new NotImplementedException();
        }

        public void Stop()
        {
            throw new NotImplementedException();
        }

        
    }

    
}
