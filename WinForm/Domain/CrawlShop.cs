using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using AngleSharp.Dom;
using AngleSharp.Dom.Html;
using AngleSharp.Parser.Html;
using Helper;
using Model;

namespace Domain
{
    public class CrawlShop
    {
        private LogHelper log = new LogHelper(typeof(CrawlShop));

        /// <summary>
        /// 抓取出租信息
        /// </summary>
        /// <param name="area"></param>
        public void CrawlDataCz(Area area)
        {
            try
            {
                Crawler crawler = new Crawler();
                string url = area.Url + ConstVar.出租 + "0/";
                string html = crawler.Crawl(url, Encoding.UTF8);
                var htmlParse = new HtmlParser();
                IHtmlDocument docuement = htmlParse.Parse(html);
                List<IElement> eles = docuement.QuerySelectorAll("div").ToList().Where(p => p.ClassName == "pager").ToList();
                if (eles.Count > 0)
                {
                    var htmlparseA = new HtmlParser();
                    IHtmlDocument htmlA = htmlparseA.Parse(eles[0].InnerHtml);
                    List<IElement> spanEles = htmlA.QuerySelectorAll("span").ToList();
                    IElement el = spanEles[spanEles.Count - 2];
                    int page = int.Parse(el.InnerHtml);
                    List<string> urls = new List<string>();
                    for (int i = 1; i <= page; i++)
                    {
                        string str = url + "pn" + i + "/";
                        urls.Add(str);
                    }

                    Parallel.ForEach(urls, e =>
                    {
                        Crawler crawlerA = new Crawler();
                        string htmlB = crawlerA.Crawl(e, Encoding.UTF8);
                        HtmlParser parseB = new HtmlParser();
                        IDocument docuemnt = parseB.Parse(htmlB);
                        IElement eleist = docuemnt.QuerySelectorAll("ul").Where(p => p.ClassName == "house-list-wrap").ToList().FirstOrDefault();

                        HtmlParser parseC = new HtmlParser();
                        IDocument docuementC = parseC.Parse(eleist.InnerHtml);
                        List<IElement> eliss = docuementC.QuerySelectorAll("div").Where(p => p.ClassName == "pic").ToList();
                        Thread.Sleep(10000 );
                        Parallel.ForEach(eliss, p =>
                        {
                            try
                            {
                                HtmlParser parseD = new HtmlParser();
                                IDocument documentD = parseD.Parse(p.InnerHtml);

                                IElement eloo = documentD.QuerySelector("a");
                                var htmlE = crawler.Crawl(eloo.GetAttribute("href").ToString(), Encoding.UTF8);
                                HtmlParser parseE = new HtmlParser();
                                IDocument documentE = parseE.Parse(htmlE);
                                IElement ele = documentE.QuerySelectorAll("span").Where(o => o.InnerHtml.StartsWith("更新于")).FirstOrDefault();
                                Thread.Sleep(10000);
                                Console.WriteLine(ele.InnerHtml);
                            }
                            catch (Exception exception)
                            {
                                Console.WriteLine(exception);
                             
                            }
                         
                        });

                    });


                }
            }
            catch (Exception e)
            {
                log.Error(e.ToString());
            }
        }
    }
}
