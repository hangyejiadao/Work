using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
        public void CrawlData(Area area)
        {
            try
            {
                Crawler crawler = new Crawler();
                string url = area.Url + ConstVar.出租 + "0/";
                string html = crawler.Crawl("http://dz.58.com/shangpucz/0/", Encoding.UTF8);
                var htmlParse = new HtmlParser();
                IHtmlDocument docuement = htmlParse.Parse(html);
                List<IElement> eles = docuement.QuerySelectorAll("a").ToList();
                //if (!eles.Any())
                //{
                //    string str = "抓取" + url + "出错";
                //    Console.WriteLine(str);
                //    log.Error(url);
                //}
                

            }
            catch (Exception e)
            {
                log.Error(e.ToString());
            }
        }
    }
}
