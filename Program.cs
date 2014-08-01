using System;
using System.IO;
using System.Net;
using System.Web;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;

namespace MyAmazon_API
{
    class Program
    {
        static void Main(string[] args)
        {
            CookieAwareWebClient aw = AmazonCls.login("III", "LLL", null);
            Console.WriteLine(AmazonCls.GetOrderNo(aw));
            Console.ReadLine();
        }
    }
    class AmazonCls
    {
        /// <summary>
        /// Login to Amazon.com with a Username and Password 
        /// </summary>
        /// <param name="Id">Email address</param>
        /// <param name="Pass">Password</param>
        /// <param name="Proxy">Set this to null if you don't want to use a proxy</param>
        /// <returns>A webclient logged into amazon.com</returns>
        public static CookieAwareWebClient login(string Id, string Pass, string Proxy)
        {
            CookieContainer ck = new CookieContainer();
            CookieAwareWebClient wc = new CookieAwareWebClient(ck);
            if (Proxy != null)
            { WebProxy wp = new WebProxy(Proxy); wc.Proxy = wp; }
            
            string signInPage = wc.DownloadString("https://amazon.com/gp/sign-in.html");
            NameValueCollection nmv = new NameValueCollection();
            string act = "";
            bool firstTimex = true;
            for (int j = 0; j < signInPage.Length; j++)
            {
                if (signInPage.Substring(j, "<form ".Length) == "<form ")
                {
                    string tag = signInPage.Substring(j, 500).Trim();
                    tag = tag.Substring(0, tag.IndexOf('>'));
                    if (tag.Contains("action=") && firstTimex)
                    {
                        int in1 = tag.IndexOf("action=\"");
                        int in2 = tag.IndexOf("\"", in1 + "action=\"".Length);
                        act = tag.Substring(in1 + "action=\"".Length, in2 - in1 - "action=\"".Length);
                        firstTimex = false;
                    }
                }
                try
                {
                    string l = signInPage.Substring(j, ("<input ").Length);
                }
                catch
                {
                    break;
                }
                if (signInPage.Substring(j, ("<input ").Length) == "<input ")
                {
                    string tag = signInPage.Substring(j, 800).Trim();
                    tag = tag.Substring(0, tag.IndexOf('>'));
                    int il1 = 0; int il2 = 0;
                    if (tag.Contains("name="))
                    {
                        string value = "";
                        int in1 = tag.IndexOf("name=\"");
                        int in2 = tag.IndexOf("\"", in1 + "name=\"".Length);
                        if (tag.Contains("value="))
                        {
                            il1 = tag.IndexOf("value=\"");
                            il2 = tag.IndexOf("\"", il1 + "value=\"".Length);
                        }
                        else
                        {
                            value = "!!!!!!!!!!!!!";
                        }
                        string name = tag.Substring(in1 + "name=\"".Length, in2 - in1 - "name=\"".Length);
                        if(value != "!!!!!!!!!!!!!") value = tag.Substring(il1 + "value=\"".Length, il2 - il1 - "value=\"".Length);
                        nmv.Add(name, value);
                    }
                }
            }
            nmv.Set("email", Id);
            nmv.Set("password", Pass);
            wc.UploadValues(act, null, nmv);
            for (int i = 0; i < wc.Headers.Count; i++)
            {
                string hd = wc.Headers.Get(i);
                string lk = "";
            } 
            string kkk = wc.DownloadString("https://amazon.com");
            wc.MPage = kkk;
            return wc;
        }
        /// <summary>
        /// Gets the orders of a logged in Amazon account (see AmazonCls.login(...) )
        /// </summary>
        /// <param name="cli">Logged in client made with AmazonCls.login(...)</param>
        /// <param name="orderFilter">order filter. default is 'year-2014', for default use null or ""</param>
        /// <returns>AmazonOrderInfo class containing OrderNo, OrderNames, OrderZips, OrderPhones, OrderAddresses</returns>
        public static int GetOrderNo(CookieAwareWebClient cli, string orderFilter)
        {
            if (string.IsNullOrEmpty(orderFilter)) orderFilter = "year-2014";
            AmazonOrderInfo ret = new AmazonOrderInfo();
            cli.MPage = cli.DownloadString("https://www.amazon.com/gp/css/order-history/ref=oh_menu_date?orderFilter=" + orderFilter);
            int from = cli.MPage.IndexOf("<label for='orderFilter'>") + "<label for='orderFilter'>".Length;
            string str = cli.MPage.Substring(from);
            string tot = "";
            int total = 0;
            for (int i = 0; i < str.Length; i++)
            {
                try
                {
                    int val = int.Parse(str[i].ToString());
                }
                catch
                {
                    break;
                }
                tot += str[i];
            }
            total = int.Parse(tot);
            return total;
        }
    }
}
