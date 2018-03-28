using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AngleSharp.Dom;
using AngleSharp.Dom.Html;
using AngleSharp.Parser.Html;
using Dal;
using Helper;
using Model;

namespace Domain
{
    /// <summary>
    /// 获取区域
    /// </summary>
    public class AreaSpider
    {

        private LogHelper log = new LogHelper(typeof(AreaSpider));
        private AreaRepository areaRepository = new AreaRepository();


        public async Task Crawl()
        {
            List<Area> area = new List<Area>();
            try
            {
                Crawler craw = new Crawler();
                string html = await craw.CrawlAsy(ConstVar.AreaUrl, Encoding.UTF8);
                var parse = new HtmlParser();
                IHtmlDocument document = await parse.ParseAsync(html);
                IEnumerable<IElement> elel = document.QuerySelectorAll("dt").Where(p => p.ClassName == null);
                List<Task> list = new List<Task>();
                TaskFactory factory = new TaskFactory();
                elel.ToList().ForEach(async p =>
               {
                   var areaa = new Area()
                   {
                       Name = p.InnerHtml,
                       ParentId = "0",
                       Url = ""
                   };
                   object Id = await areaRepository.AddAsync(areaa);

                   var task = new Task(async () =>
                   {
                       await CrawlCity(p.NextElementSibling.InnerHtml, long.Parse(Id.ToString()));
                   });
                   list.Add(factory.StartNew(() => { task.Start(); }));

               });
                Task.WaitAll(list.ToArray());
                Console.WriteLine("区域全部抓取完成");
            }
            catch (Exception e)
            {
                log.Error(e.ToString());
            }

        }

        /// <summary>
        /// 爬取城市
        /// </summary>
        /// <param name="html"></param>
        /// <returns></returns>
        public async Task CrawlCity(string html, long parentId)
        {
            var parse = new HtmlParser();
            IHtmlDocument document = await parse.ParseAsync(html);
            IEnumerable<IElement> eles = document.QuerySelectorAll("a").Where(p => true);
            List<Task> tasks = new List<Task>();
            TaskFactory factory = new TaskFactory();
            Parallel.ForEach(eles, async p =>
            {
                var url = p.Attributes["href"].Value;
                var area = new Area()
                {
                    Name = p.InnerHtml,
                    Url = p.Attributes["href"].Value,
                    ParentId = parentId.ToString(),

                };

                object Id = await areaRepository.AddAsync(area);
                area.Id = int.Parse(Id.ToString());
                var task = new Task(async () =>
                {
                  
                });
                tasks.Add(  factory.StartNew(() => { task.Start(); }));
            });
            Task.WaitAll(tasks.ToArray());
            Console.WriteLine("");
        }

        //private Task CrawlData(Area area)
        //{
           
        //}
    }
}
