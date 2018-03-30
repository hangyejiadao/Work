using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;


namespace Helper
{
    public class Crawler
    {
        private LogHelper log = new LogHelper(typeof(Crawler));

        private string[] user_agents = ConfigHelper.Ua().Replace("\r\n","\n").Split('\n');


        private const string ipApiA =
            "http://api.ip.data5u.com/dynamic/get.html?order=03ea626781c3f0076f79b96ecfeb3644";

        private int index = 0;
        public string Crawl(string url, Encoding encoding)
        {
          
            try
            {

                Uri uri = new Uri(ipApiA);
                System.Net.HttpWebRequest requestA = (System.Net.HttpWebRequest)System.Net.WebRequest.Create(ipApiA);
                System.Net.HttpWebResponse responseA = (System.Net.HttpWebResponse)requestA.GetResponse();
                System.IO.Stream stream = responseA.GetResponseStream();
                System.IO.StreamReader readerA = new System.IO.StreamReader(stream);
                String iptxt = readerA.ReadToEnd();
                readerA.Dispose();
                readerA.Close();
                string[] ipports = iptxt.Split(new String[] { "\n" }, StringSplitOptions.None);
                System.Threading.Thread.Sleep(1000);


                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                request.Method = "Get";
                request.UserAgent = user_agents[(++index) % user_agents.Count()];
                if (index == 99999)
                {
                    index = 0;
                }

                string ipport = ipports[(int)(ipports.Length * new Random().Next(0, 1))];
                System.Net.WebClient client = new System.Net.WebClient();
                client.Encoding = System.Text.Encoding.GetEncoding("GB2312");
                // 设置代理
                System.Net.WebProxy proxy = new System.Net.WebProxy();
                proxy.Address = new Uri("http://" + ipports[0] + "/");

                // 设置代理服务器
              



                request.Timeout = 5000;
                request.Proxy = proxy;
                request.AllowAutoRedirect = true;
                request.KeepAlive = true;
               
                Thread.Sleep(1000);
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


        public string CrawlByProxy(string url, string address, Encoding encoding)
        {
            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                request.Method = "Get";
                request.UserAgent = "Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/57.0.2987.133 Safari/537.36";
                request.Timeout = 5000;
                WebProxy prox = new WebProxy(address);
                string proxyUser = "hanghang";
                string proxyPass = "hangyejiadao";

                request.Proxy = prox;
                request.Proxy.Credentials = new System.Net.NetworkCredential(proxyUser, proxyPass);
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


        public bool VerIP(string address)
        {
            try
            {
                HttpWebRequest Req;
                HttpWebResponse Resp;
                WebProxy proxyObject = new WebProxy(address);// port为端口号 整数型
                Req = WebRequest.Create("https://www.baidu.com") as HttpWebRequest;
                Req.Proxy = proxyObject; //设置代理
                string proxyUser = "hanghang";
                string proxyPass = "hangyejiadao";

            
                Req.Proxy.Credentials = new System.Net.NetworkCredential(proxyUser, proxyPass);
                Req.Timeout = 500;   //超时

                using (Resp = (HttpWebResponse)Req.GetResponse())
                {
                    if (Resp.StatusCode == HttpStatusCode.OK)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }

            }
            catch (Exception ex)
            {
                return false;
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

        /// <summary>
        /// 抓取图片
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public Bitmap CrawlPic(string url)
        {
            try
            {

                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                request.Method = "Get";
                request.UserAgent = user_agents[(++index) % user_agents.Count()];
                if (index == 99999)
                {
                    index = 0;
                }

                // 代理服务器
                string proxyHost = "61.132.93.14";
                string proxyPort = "6445";
                Thread.Sleep(1000);

                // 代理隧道验证信息
                string proxyUser = "16FSTHOX";
                string proxyPass = "422650";

                // 设置代理服务器
                WebProxy proxy = new WebProxy(string.Format("{0}:{1}", proxyHost, proxyPort), true);



                request.Timeout = 5000;
                request.Proxy = proxy;
                request.AllowAutoRedirect = true;
                request.KeepAlive = true;
                request.Proxy.Credentials = new System.Net.NetworkCredential(proxyUser, proxyPass);
                using (HttpWebResponse response = request.GetResponse() as HttpWebResponse)
                {
                    if (response.StatusCode == HttpStatusCode.OK)
                    {

                        using (Stream reader = (response.GetResponseStream()))
                        {
                            return new Bitmap(reader);
                        }
                    }
                    else
                    {
                        return null;
                    }

                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                log.Error(e.ToString());
                return null;
            }
        }


        /// <summary>
        /// 将IP写进ip池
        /// </summary>
        public void EquenIps()
        {
            if (RedisHelper.GetInstance().QueueCount(ConstVar.IpPool) < 800)
            {
                WebClient webClient = new WebClient();
                string IPs = webClient.DownloadString(ConfigHelper.GetStringConfig(ConstVar.IPUrl));
                string[] IPS = IPs.Replace("\r\n", "\n").Split('\n');
                foreach (var VARIABLE in IPS)
                {
                    RedisHelper.GetInstance().Enque(ConstVar.IpPool, VARIABLE);
                }
            }

        }


        public string DequenIps()
        {
            string Ip = RedisHelper.GetInstance().Deque(ConstVar.IpPool);
            if (VerIP(Ip))
            {
                return Ip;
            }
            else
            {
                return DequenIps();
            }
        }


    }
}
