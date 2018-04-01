using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using WencaiMonitor.ConsoleApp2;

namespace WencaiMonitor.ConsoleApp
{
    class Program
    {
        static int flagNum = 0;
        static string strategyKeyword = ConfigurationManager.AppSettings["StrategyKeyword"];
        static string unifiedQueryConditionUrl = ConfigurationManager.AppSettings["WencaiGetUnifiedQueryCondtionUrl"] + strategyKeyword;
        static string unifiedQueryResultUrl = ConfigurationManager.AppSettings["WencaiGetUnifiedQueryResultUrl"] + strategyKeyword;
        static HttpClient httpClient = new HttpClient();
        static Dictionary<string, string> dicCache = new Dictionary<string, string>();
        static int cacheLengthFlag = -1;
        static void Main(string[] args)
        {
            Console.WriteLine("Good Luck! Watch out!");

            double refreshFrq = double.Parse(ConfigurationManager.AppSettings["RefreshFrequency"]);
            httpClient.Timeout = new TimeSpan(0, 0, 600);
            Timer timer = new Timer();
            timer.Interval = refreshFrq;
            timer.AutoReset = true;
            timer.Enabled = true;
            timer.Elapsed += Timer_Elapsed;

            Console.ReadLine();
        }

        private async static void Timer_Elapsed(object sender, ElapsedEventArgs e)
        {


            //Console.WriteLine("Good Luck! Watch out!");
            //Console.WriteLine("Time:{0}", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss-fff"));

            HttpContent content = null;
            HttpResponseMessage qcResponseMs = new HttpResponseMessage();
            string queryConfition = string.Empty;
            try
            {

                HttpResponseMessage qcResponseMsg = httpClient.PostAsync(unifiedQueryConditionUrl, content).Result;
                if (qcResponseMs.IsSuccessStatusCode)
                {
                    string queryConditionResponse = string.Empty;
                    queryConditionResponse = await qcResponseMsg.Content.ReadAsStringAsync();
                    WencaiQueryConditionModel queryConditionModel = JsonConvert.DeserializeObject<WencaiQueryConditionModel>(queryConditionResponse);
                    queryConfition = queryConditionModel.data.condition;
                    // Console.WriteLine("Quern Condition:" + queryConfition);
                }
                else
                {
                    Console.WriteLine("Haven't get any qurey codition.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Haven't get any qurey codition.");
                Console.WriteLine(ex.StackTrace);
            }

            if (!string.IsNullOrEmpty(queryConfition))
            {
                try
                {
                    content = new FormUrlEncodedContent(new Dictionary<string, string>
                {
                    {"condition",queryConfition }
                });

                    HttpResponseMessage qrResponseMsg = httpClient.PostAsync(unifiedQueryResultUrl, content).Result;
                    if (qrResponseMsg.IsSuccessStatusCode)
                    {
                        string stockResultResponse = string.Empty;
                        stockResultResponse = await qrResponseMsg.Content.ReadAsStringAsync();
                        WencaiQueryResultModel resultModel = JsonConvert.DeserializeObject<WencaiQueryResultModel>(stockResultResponse);
                        var arryResult = resultModel.data.data;

                        StringBuilder sbMonitorDisplay = new StringBuilder();
                        sbMonitorDisplay.AppendLine("======================Holly Shit================================");
                        sbMonitorDisplay.AppendLine("Code \t股票简称\t最新涨跌幅 连续涨停天数 所属同花顺行业");
                        foreach (var record in arryResult)
                        {
                            sbMonitorDisplay.AppendLine(string.Format("{0}\t{1}\t{2}%\t{3}Day\t{4}", record.hqCode, record.股票简称, record.最新涨跌幅, record.连续涨停天数 == null ? "0" : record.连续涨停天数, record.所属同花顺行业));
                            sbMonitorDisplay.AppendLine();
                            if (!dicCache.ContainsKey(record.hqCode))
                            {
                                dicCache.Add(record.hqCode, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss-fff"));
                            }
                        }
                        sbMonitorDisplay.AppendLine("Total Count:" + arryResult.Length);
                        if (dicCache.Count != cacheLengthFlag)
                        {
                            Console.Clear();
                            cacheLengthFlag = dicCache.Count;
                            Console.WriteLine(sbMonitorDisplay.ToString());
                        }

                    }
                }
                catch (Exception)
                {

                    throw;
                }
            }
            else
            {
                Console.WriteLine("Haven't get valid qurey codition.");
            }
        }
    }
}
