using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Linq;
namespace LinkChecker
{
    class Program
    {
        static string _linksFl = "";
        static void Main(string[] args)
        {
            ReadArgs(args);
            var resultstext = "";
            if(string.IsNullOrEmpty(_linksFl))
            {
                _linksFl = "Links.xml";
            }
            XDocument doc = XDocument.Parse(File.ReadAllText(_linksFl,Encoding.UTF8));
            foreach(var e in doc.Root.Descendants("extract"))
            {

                if(e.Attribute("value")!=null)
                {
                    var lnk = e.Attribute("value").Value;
                    resultstext += Environment.NewLine +string.Format("{0}:{1}",lnk,CheckIfLinkLive(lnk).ToString());
                    
                }
               
                
            }

            File.WriteAllText("Results.txt",resultstext);
          
        }

        internal static bool CheckIfLinkLive(string url)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            
            request.Timeout = Timeout.Infinite;
            request.KeepAlive = true;
            request.Method = "HEAD";
            try
            {
               
                using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
                {
                    return response.StatusCode == HttpStatusCode.OK;
                }
            }
            catch (WebException ex)
            {
               
                return false;
            }

        }
        static void ReadArgs(string[] args)
        {
            for(var i=0;i<args.Length;i++)
            {
                if(args[i].StartsWith("-l"))
                {
                    _linksFl = args[i].Trim().Substring(2);
                }
            }
        }
    }
}
