using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;

namespace free_proxy_listProxyTool
{
    public class Proxy
    {
        private string proxyAddress { get; set; }
        private string port { get; set; }
        public Proxy(string proxyAddress, string port)
        {
            this.proxyAddress = proxyAddress;
            this.port = port;
        }
        public Proxy()
        {

        }
        public List<Proxy> freeProxyListProxy()
        {
            string url = "https://free-proxy-list.net/";

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.Method = "GET";
            request.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64; rv:81.0) Gecko/20100101 Firefox/81.0";
            request.Accept = "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,*/*;q=0.8";
            request.Headers.Add("Accept-Language", "en-GB,en;q=0.5");
            //request.Headers.Add("Accept-Encoding", "gzip, deflate, br");
            request.Host = "free-proxy-list.net";
            request.ContentType = "keep-alive";
            request.Headers.Add("DNT", "1");
            request.Headers.Add("Upgrade-Insecure-Requests", "1");

            var response = (HttpWebResponse)(request.GetResponse());
            List<Proxy> proxyAddressList = new List<Proxy>();
            List<String> allCollectedData = new List<string>();
            if (response.StatusCode == HttpStatusCode.OK)
            {
                HtmlDocument htmlDocument = new HtmlDocument();
                StreamReader streamReader = new StreamReader(response.GetResponseStream(), Encoding.UTF8);
                htmlDocument.Load(streamReader);

                string result = string.Join(" ", htmlDocument.DocumentNode.SelectNodes("//tbody//tr").Descendants()
                .Where(n => !n.HasChildNodes && !string.IsNullOrWhiteSpace(n.InnerText))
                .Select(n => n.InnerText));


                string queryForIPAddress = @"\b\d{1,3}\.\d{1,3}\.\d{1,3}\.\d{1,3} \b\d{2,5}";

                Regex regex = new Regex(queryForIPAddress);
               
                MatchCollection match = regex.Matches(/*allCollectedData[i]*/result);
                
                foreach(var ipAddressAndPort in match)
                {
                    string[] splitString = ipAddressAndPort.ToString().Split(' ');
                    Proxy proxy = new Proxy(splitString[0], splitString[1]);
                    proxyAddressList.Add(proxy);
                }
            }
            return proxyAddressList;
        }
    }
}
