using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AngleSharp.Dom;
using AngleSharp.Dom.Html;
using AngleSharp.Parser.Html;
using Dal;
using Helper;
using Model;
using Model.Dto;
using Newtonsoft.Json;


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
         
            try
            {
                Rootobject obj = JsonConvert.DeserializeObject<Rootobject>(File.ReadAllText(AppDomain.CurrentDomain.BaseDirectory + "Area.txt"));
                foreach (var item in obj.GetType().GetProperties())
                {
                    Area rootArea = new Area() { Name = item.Name, Url = string.Empty, ParentId = "" };
                    object Id = areaRepository.Add(rootArea);
                    foreach (var p in ParseTool.TransFerType(item.PropertyType.FullName).GetProperties())
                    {
                        
                        var url = p.GetValue(item.GetValue(obj)).ToString().Substring(0, p.GetValue(item.GetValue(obj)).ToString().IndexOf("|", StringComparison.Ordinal));
                        Area areaa = new Area()
                        {
                            Name = p.Name,
                            Url = "http://"+url+".58.com/",
                            ParentId = Id.ToString()
                        };
                        areaRepository.Add(areaa);
                    }
                }
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
                tasks.Add(factory.StartNew(() => { task.Start(); }));
            });
            Task.WaitAll(tasks.ToArray());
            Console.WriteLine("");
        }

        //private Task CrawlData(Area area)
        //{

        //}
    }
}
