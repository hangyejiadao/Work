using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;


namespace Helper
{
    public class Crawler
    {
        private LogHelper log = new LogHelper(typeof(Crawler));


        public string Crawl(string url, Encoding encoding)
        {
            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                request.Method = "Get";
                request.UserAgent = "Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/57.0.2987.133 Safari/537.36";
                request.Timeout = 5000;
                using (HttpWebResponse response = request.GetResponse() as HttpWebResponse)
                {
                    if (response.StatusCode == HttpStatusCode.OK)
                    {
                        using (StreamReader reader = new StreamReader(response.GetResponseStream(), encoding))
                        {
                            return reader.ReadToEnd();
                        }
                    }
                    else
                    {
                        return "失败";
                    }

                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                log.Error(e.ToString());
                return e.ToString();
            }
        }


        public async Task<string> CrawlAsy(string url, Encoding encoding)
        {
            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                request.Method = "Get";
                request.UserAgent = "Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/57.0.2987.133 Safari/537.36";
                request.Timeout = 5000;
                using (HttpWebResponse response = await request.GetResponseAsync() as HttpWebResponse)
                {
                    if (response.StatusCode == HttpStatusCode.OK)
                    {
                        using (StreamReader reader = new StreamReader(response.GetResponseStream(), encoding))
                        {
                            return await reader.ReadToEndAsync();
                        }
                    }
                    else
                    {
                        return "失败";
                    }

                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                log.Error(e.ToString());
                return e.ToString();
            }
        }



    }
}
