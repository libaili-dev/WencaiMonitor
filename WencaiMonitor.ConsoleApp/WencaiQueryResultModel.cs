using System;
using System.Collections.Generic;
using System.Text;

namespace WencaiMonitor.ConsoleApp2
{
   

    public class WencaiQueryResultModel
    {
        public bool success { get; set; }
        public string message { get; set; }
        public Data data { get; set; }
    }

    public class Data
    {
        public string status_code { get; set; }
        public string status_msg { get; set; }
        public string show_type { get; set; }
        public string domain { get; set; }
       
        public Datum1[] data { get; set; }
       
        public string token { get; set; }
    }


    public class Datum1
    {
        public string 股票代码 { get; set; }
        public string 股票简称 { get; set; }
        public float 最新价 { get; set; }
        public float 最新涨跌幅 { get; set; }
        public string hqCode { get; set; }
        public string marketId { get; set; }        
        public string 连续涨停天数 { get; set; }      
        public string 所属同花顺行业 { get; set; }           
        
    }


}
