using System;
using System.Collections.Generic;
using System.Text;

namespace WencaiMonitor.ConsoleApp
{
    public class WencaiQueryConditionModel
    {

        public bool success { get; set; }
        public string message { get; set; }
        public Data data { get; set; }
    }



    public class Data
    {
        public bool isStock { get; set; }
        public string domain { get; set; }
        public string queryType { get; set; }
        public bool diagSuccess { get; set; }
        public string condition { get; set; }
        public object[] conceptBlockData { get; set; }
        public Interactive interactive { get; set; }
    }

    public class Interactive
    {
        public string text { get; set; }
        public string originalText { get; set; }
        public string type { get; set; }
        public string[] synonyms { get; set; }
        public string category { get; set; }
        public string _class { get; set; }
        public Newqueryre[] newQueryRes { get; set; }
        public bool signal { get; set; }
    }

    public class Newqueryre
    {
        public string originalIndex { get; set; }
        public string query { get; set; }
    }
}
