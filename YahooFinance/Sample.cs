using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YahooFinance;
using YahooFinance.NET;

namespace YahooFinance
{
  public  class Sample
    {
        public void GetTick()
        {
            string cookie = "31km9s9dm7p94&b=3&s=3d";
            string crumb = "gFXT6LioH24";
            string exchange = "NSE"; //"ASX";
            string symbol = "INFY";// "AFI";

            YahooFinanceClient yahooFinance = new YahooFinanceClient(cookie, crumb);
            string yahooStockCode = yahooFinance.GetYahooStockCode(exchange, symbol);
            List<YahooHistoricalPriceData> yahooPriceHistory = yahooFinance.GetDailyHistoricalPriceData(yahooStockCode);
            List<YahooHistoricalDividendData> yahooDividendHistory = yahooFinance.GetHistoricalDividendData(yahooStockCode);
            YahooRealTimeData yahooRealTimeData = yahooFinance.GetRealTimeData(yahooStockCode);

        }
    }
}
