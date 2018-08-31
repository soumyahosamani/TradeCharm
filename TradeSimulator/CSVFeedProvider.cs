using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Threading;
using Utility;

namespace TradeSimulator
{
    public class CSVFeedProvider : AbstractFeedProvider
    {
        private IDictionary<string, string> filePaths = new Dictionary<string, string>();
        public CSVFeedProvider()
        {
            LoadFilePaths();
        }

        protected override void CreateProcesThreads()
        {
            foreach (var symbol in SubscribedSymbols)
            {
                var file = filePaths.ContainsKey(symbol) ? filePaths[symbol] : null;
                if (file != null)
                {
                    var processThread = new Thread(() => { ProcessCsv(file); });
                    processThread.Name = "csv_" +symbol;
                    processThread.IsBackground = false;
                    processThreads.Add(processThread);                    
                }
            }
        }

        private void ProcessCsv(string file)
        {
            var randomNumber = Randomizer.GetRandomNumber(1,5);
            // handle concern of file being held open for such a long time?? or keeping entire csv data in memory which  is better?
            var quotes = File.ReadAllLines(file);
            int tickId = 0;           

            foreach (var quote in quotes)
            {
                // to skip header of csv file
                if(tickId == 0)
                {
                    tickId++;
                    continue;
                }

                Console.WriteLine("Thread {0} {1} Sleeping for {2} s", Thread.CurrentThread.ManagedThreadId, Thread.CurrentThread.Name, randomNumber/1000);
                Thread.Sleep(randomNumber);
                var temp = quote.Split(',');                
                var tick = new Tick(tickId, temp[0].Trim('"'), double.Parse(temp[8].Trim('"')), DateTime.Parse(temp[2].Trim('\"')));
                RaiseNewTickEvent(tick);
                tickId++;
            }
        }

        private void LoadFilePaths()
        {
            var baseFolder = ConfigurationSettings.AppSettings["CsvProviderFolder"];
            var symbols = ConfigurationSettings.AppSettings["Symbols"].Split(',');
            foreach (var symbol in symbols)
            {
                filePaths[symbol] = (Path.Combine(baseFolder, symbol + ".csv"));
            }
        }
    }

}
