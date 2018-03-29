using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using AngleSharp.Dom;
using AngleSharp.Dom.Html;
using AngleSharp.Parser.Html;
using Dal;
using Helper;
using Model;
using Model.BaseModel;

namespace Domain
{
    public class CrawlShop
    {
        private LogHelper log = new LogHelper(typeof(CrawlShop));
        private ShopRentOrTransferRepository shoprepo = new ShopRentOrTransferRepository();
        private ShopBegRentRepository shopbegrepo = new ShopBegRentRepository();
        private ImageRepository imgrepo = new ImageRepository();
        private ErrorUrlRepository errorUrlrepsitory = new ErrorUrlRepository();



        public void CrawlAll(Area area)
        {
            CrawlDataCz(area);

            CrawlDataZr(area);

            CrawlDataBegRent(area);

        }




        /// <summary>
        /// 抓取出租信息
        /// </summary>
        /// <param name="area"></param>
        public void CrawlDataCz(Area area)
        {
            string url = string.Empty;
            try
            {
                Crawler crawler = new Crawler();
                url = area.Url + ConstVar.出租 + "0/";
                string html = crawler.Crawl(url, Encoding.UTF8);
                var htmlParse = new HtmlParser();
                IHtmlDocument docuement = htmlParse.Parse(html);
                List<IElement> eles = docuement.QuerySelectorAll("div").ToList().Where(p => p.ClassName == "pager").ToList();
                if (eles.Count > 0)
                {

                    IHtmlDocument htmlA = htmlParse.Parse(eles[0].InnerHtml);
                    List<IElement> spanEles = htmlA.QuerySelectorAll("span").ToList();


                    int page = 0;
                    if (spanEles.Count > 2)
                    {
                        IElement el = spanEles[spanEles.Count - 2];
                        page = int.Parse(el.InnerHtml);
                    }
                    else
                    {
                        page = 1;
                    }



                    
                    for (int i = 1; i <= page; i++)
                    {
                        string str = string.Empty;
                        try
                        {
                            str = url + "pn" + i + "/";
                            Crawler crawlerA = new Crawler();
                            string htmlB = crawlerA.Crawl(str, Encoding.UTF8);

                            IDocument docuemnt = htmlParse.Parse(htmlB);
                            IElement eleist = docuemnt.QuerySelectorAll("ul").Where(p => p.ClassName == "house-list-wrap").ToList().FirstOrDefault();


                            IDocument docuementC = htmlParse.Parse(eleist.InnerHtml);
                            List<IElement> eliss = docuementC.QuerySelectorAll("div").Where(p => p.ClassName == "pic").ToList();
                            foreach (var p in eliss)
                            {
                                string urlA = string.Empty;
                                try
                                {

                                    IDocument documentD = htmlParse.Parse(p.InnerHtml);

                                    IElement eloo = documentD.QuerySelector("a");
                                    urlA = eloo.GetAttribute("href").ToString();
                                    var htmlE = crawler.Crawl(eloo.GetAttribute("href").ToString(), Encoding.UTF8);


                                    IDocument documentE = htmlParse.Parse(htmlE);
                                    IElement ele = documentE.QuerySelectorAll("span").Where(o => o.InnerHtml.StartsWith("更新于")).FirstOrDefault();
                                    DateTime time = DateTime.Parse(ele.InnerHtml.Replace("更新于", ""));
                                    if (time > DateTime.Now.AddMonths(-2))
                                    {
                                        IElement InfoTitleElee = documentE.QuerySelectorAll("h1")
                                            .FirstOrDefault(o => o.ClassName == "c_000 f20");

                                        IElement money = documentE.QuerySelectorAll("span").FirstOrDefault(o => o.ClassName == "house_basic_title_money_num");
                                        var InfoContent = documentE.QuerySelectorAll("div")
                                            .Where(o => o.ClassName == "general-item-wrap").ToList()[2];
                                        var Customer = documentE.QuerySelectorAll("span")
                                            .FirstOrDefault(o => o.ClassName == "f14 c_333 jjrsay");
                                        var phone = documentE.QuerySelectorAll("p").FirstOrDefault(o => o.ClassName == "phone-num");


                                        var InfoEles = htmlParse.Parse(documentE.QuerySelectorAll("ul")
                                            .FirstOrDefault(o => o.ClassName == "house_basic_title_content")
                                            ?.InnerHtml).QuerySelectorAll("li").ToList();
                                        //面积
                                        IElement areasize = htmlParse.Parse(InfoEles[0].InnerHtml).QuerySelectorAll("span")
                                            .FirstOrDefault(o => o.ClassName == "house_basic_title_content_item2");
                                        //行业名字    
                                        IElement IndustryName = htmlParse.Parse(InfoEles[2].InnerHtml).QuerySelectorAll("span")
                                            .FirstOrDefault(o => o.ClassName == "house_basic_title_content_item3");
                                        IElement address = htmlParse.Parse(InfoEles[5].InnerHtml).QuerySelectorAll("a")
                                            .FirstOrDefault(o => o.ClassName == "house_basic_title_content_item3 blue-link");
                                        IElement addressDetail = htmlParse.Parse(InfoEles[5].InnerHtml).QuerySelectorAll("span")
                                            .FirstOrDefault(o => o.ClassName == "house_basic_title_content_item3 xxdz-des");

                                        var shoptransfer = new ShopRentOrTransfer()
                                        {
                                            Id = Guid.NewGuid(),
                                            ShopArea = areasize.InnerHtml,
                                            InfoTitle = InfoTitleElee.InnerHtml,
                                            TransFerMoney = money.InnerHtml,
                                            Address = string.Join("", address.InnerHtml),
                                            DetailAddress = addressDetail.InnerHtml,
                                            InfoContent = InfoContent.InnerHtml,
                                            InfoType = Model.BaseModel.InfoType.出租,
                                            IndustryName = IndustryName.InnerHtml,
                                            Customer = Customer.InnerHtml,
                                            Phone = phone.InnerHtml,
                                            AreaId = area.Id.ToString(),
                                            UpdateTime = time
                                        };
                                        var imgUl = documentE.QuerySelectorAll("ul")
                                               .FirstOrDefault(o => o.ClassName == "general-pic-list");
                                        IDocument documentf = htmlParse.Parse(imgUl.InnerHtml);
                                        shoprepo.Add(shoptransfer);
                                        Console.WriteLine(area.Name + "添加一条出租信息");
                                        var tem = documentf.QuerySelectorAll("img").Select(o => o.GetAttribute("data-src"));
                                        if (tem != null && tem.Count() > 0)
                                        {
                                            foreach (var o in tem)
                                            {
                                                if (o != null)
                                                {


                                                    Bitmap img = crawler.CrawlPic(o);
                                                    if (img != null)
                                                    {
                                                        string path = AppDomain.CurrentDomain.BaseDirectory + "Imgs/" + shoptransfer.Id + "/";
                                                        if (!Directory.Exists(path))
                                                        {
                                                            Directory.CreateDirectory(path);
                                                        }

                                                        string fullPath =
                                                            path + Guid.NewGuid().ToString().Replace("-", "") + ".png";
                                                        img.Save(fullPath);
                                                        string savePath = fullPath.Replace(AppDomain.CurrentDomain.BaseDirectory,
                                                            "");
                                                        imgrepo.Add(new Model.Image()
                                                        {
                                                            FkId = shoptransfer.Id,
                                                            ImageUrl = savePath,
                                                            InfoType = TableType.ShopRentOrTransfer,

                                                        });
                                                
                                                    }
                                                }
                                            }

                                        }

                                    }

                                }
                                catch (Exception e)
                                {
                                    errorUrlrepsitory.Add(new ErrorUrl() { UrlType = UrlType.Item, Url = urlA });
                                    log.Error(e.ToString());

                                }
                            }

                        }
                        catch (Exception e)
                        {
                            errorUrlrepsitory.Add(new ErrorUrl() { UrlType = UrlType.Page, Url = str });
                            log.Error(e.ToString());

                        }



                    }

                }

                Console.WriteLine(area.Name + "出租信息抓取完成");
            }
            catch (Exception e)
            {
                errorUrlrepsitory.Add(new ErrorUrl() { UrlType = UrlType.All, Url = url });
                log.Error(e.ToString());
            }
        }


        /// <summary>
        /// 抓取转让信息
        /// </summary>
        /// <param name="area"></param>
        public void CrawlDataZr(Area area)
        {
            string url = string.Empty;
            try
            {
                Crawler crawler = new Crawler();
                url = area.Url + ConstVar.转让 + "0/";
                string html = crawler.Crawl(url, Encoding.UTF8);
                var htmlParse = new HtmlParser();
                IHtmlDocument docuement = htmlParse.Parse(html);
                List<IElement> eles = docuement.QuerySelectorAll("div").ToList().Where(p => p.ClassName == "pager").ToList();
                if (eles.Count > 0)
                {

                    IHtmlDocument htmlA = htmlParse.Parse(eles[0].InnerHtml);
                    List<IElement> spanEles = htmlA.QuerySelectorAll("span").ToList();
                    int page = 0;
                    if (spanEles.Count > 2)
                    {
                        IElement el = spanEles[spanEles.Count - 2];
                        page = int.Parse(el.InnerHtml);
                    }
                    else
                    {
                        page = 1;
                    }

                   
                    for (int i = 1; i <= page; i++)
                    {
                        string str = url + "pn" + i + "/";
                        try
                        {
                            //抓取每页
                            Crawler crawlerA = new Crawler();
                            string htmlB = crawlerA.Crawl(str, Encoding.UTF8);

                            IDocument docuemnt = htmlParse.Parse(htmlB);
                            IElement eleist = docuemnt.QuerySelectorAll("ul").Where(p => p.ClassName == "house-list-wrap").ToList().FirstOrDefault();


                            IDocument docuementC = htmlParse.Parse(eleist.InnerHtml);
                            List<IElement> eliss = docuementC.QuerySelectorAll("div").Where(p => p.ClassName == "pic").ToList();
                            //抓取每条
                            eliss.ForEach(p =>
                            {
                                string ItemUrl = string.Empty;
                                try
                                {
                                    IDocument documentD = htmlParse.Parse(p.InnerHtml);
                                    IElement eloo = documentD.QuerySelector("a");
                                    ItemUrl = eloo.GetAttribute("href").ToString();
                                    var htmlE = crawler.Crawl(ItemUrl, Encoding.UTF8);

                                    //开始抓取每条
                                    IDocument documentE = htmlParse.Parse(htmlE);
                                    IElement ele = documentE.QuerySelectorAll("span").Where(o => o.InnerHtml.StartsWith("更新于")).FirstOrDefault();
                                    DateTime time = DateTime.Parse(ele.InnerHtml.Replace("更新于", ""));
                                    if (time > DateTime.Now.AddMonths(-2))
                                    {
                                        IElement InfoTitleElee = documentE.QuerySelectorAll("h1")
                                          .FirstOrDefault(o => o.ClassName == "c_000 f20");

                                        IElement money = documentE.QuerySelectorAll("span").FirstOrDefault(o => o.ClassName == "house_basic_title_money_num");
                                        var InfoContent = documentE.QuerySelectorAll("div")
                                            .Where(o => o.ClassName == "general-item-wrap").FirstOrDefault(u => u.ParentElement.ClassName == "general-item general-miaoshu");




                                        var Customer = documentE.QuerySelectorAll("span")
                                            .FirstOrDefault(o => o.ClassName == "f14 c_333 jjrsay");
                                        var phone = documentE.QuerySelectorAll("p").FirstOrDefault(o => o.ClassName == "phone-num");


                                        var InfoEles = htmlParse.Parse(documentE.QuerySelectorAll("ul")
                                            .FirstOrDefault(o => o.ClassName == "house_basic_title_content")
                                            ?.InnerHtml).QuerySelectorAll("li").ToList();
                                        //面积
                                        IElement areasize = htmlParse.Parse(InfoEles[0].InnerHtml).QuerySelectorAll("span")
                                .FirstOrDefault(o => o.ClassName == "house_basic_title_content_item2");
                                        //行业名字    
                                        IElement IndustryName = htmlParse.Parse(InfoEles[2].InnerHtml).QuerySelectorAll("span")
                                .FirstOrDefault(o => o.ClassName == "house_basic_title_content_item3");
                                        IElement address = htmlParse.Parse(InfoEles[5].InnerHtml).QuerySelectorAll("a")
                                            .FirstOrDefault(o => o.ClassName == "house_basic_title_content_item3 blue-link");
                                        IElement addressDetail = htmlParse.Parse(InfoEles[5].InnerHtml).QuerySelectorAll("span")
                                            .FirstOrDefault(o => o.ClassName == "house_basic_title_content_item3 xxdz-des");

                                        var shoptransfer = new ShopRentOrTransfer()
                                        {
                                            Id = Guid.NewGuid(),
                                            ShopArea = areasize.InnerHtml.Trim(),
                                            InfoTitle = InfoTitleElee.InnerHtml.Trim(),
                                            TransFerMoney = money.InnerHtml.Trim(),
                                            Address = string.Join("", address.InnerHtml.Trim()),
                                            DetailAddress = addressDetail.InnerHtml.Trim(),
                                            InfoContent = InfoContent.InnerHtml.Trim(),
                                            InfoType = Model.BaseModel.InfoType.出租,
                                            IndustryName = IndustryName.InnerHtml.Trim(),
                                            Customer = Customer.InnerHtml.Trim(),
                                            Phone = phone.InnerHtml.Trim(),
                                            AreaId = area.Id.ToString(),
                                            UpdateTime = time
                                        };
                                        var imgUl = documentE.QuerySelectorAll("ul")
                                               .FirstOrDefault(o => o.ClassName == "general-pic-list");
                                        IDocument documentf = htmlParse.Parse(imgUl.InnerHtml);
                                        shoprepo.Add(shoptransfer);
                                        Console.WriteLine(area.Name + "添加了一条转让信息");
                                        documentf.QuerySelectorAll("img").Select(o => o.GetAttribute("data-src")).ToList().ForEach(
                                            o =>
                                            {
                                                Bitmap img = crawler.CrawlPic(o);
                                                if (img != null)
                                                {
                                                    string path = AppDomain.CurrentDomain.BaseDirectory + "Imgs/" + shoptransfer.Id + "/";
                                                    if (!Directory.Exists(path))
                                                    {
                                                        Directory.CreateDirectory(path);
                                                    }

                                                    string fullPath =
                                                        path + Guid.NewGuid().ToString().Replace("-", "") + ".png";
                                                    img.Save(fullPath);
                                                    string savePath = fullPath.Replace(AppDomain.CurrentDomain.BaseDirectory,
                                                        "");
                                                    imgrepo.Add(new Model.Image()
                                                    {
                                                        FkId = shoptransfer.Id,
                                                        ImageUrl = savePath,
                                                        InfoType = TableType.ShopRentOrTransfer,

                                                    });
                                                 

                                                }
                                            });

                                       
                                    }

                                }
                                catch (Exception exception)
                                {
                                    errorUrlrepsitory.Add(new ErrorUrl() { Url = ItemUrl, UrlType = UrlType.Page });
                                    Console.WriteLine(exception);
                                }
                            });
                        }
                        catch (Exception e)
                        {
                            errorUrlrepsitory.Add(new ErrorUrl() { Url = str, UrlType = UrlType.Page });
                            Console.WriteLine(e.ToString());
                        }

                    }


                    Console.WriteLine(area.Name + "转让信息抓完");
                }

            }
            catch (Exception e)
            {
                errorUrlrepsitory.Add(new ErrorUrl() { Url = url, UrlType = UrlType.All });
                log.Error(e.ToString());
            }
        }



        /// <summary>
        /// 抓取求租
        /// </summary>
        /// <param name="area"></param>
        public void CrawlDataBegRent(Area area)
        {
            try
            {
                Crawler crawler = new Crawler();
                string url = area.Url + ConstVar.求租 + "0/";
                string html = crawler.Crawl(url, Encoding.UTF8);
                var htmlParse = new HtmlParser();
                IHtmlDocument docuement = htmlParse.Parse(html);
                List<IElement> eles = docuement.QuerySelectorAll("div").ToList().Where(p => p.ClassName == "pager")
                    .ToList();
                if (eles.Count > 0)
                {

                    IHtmlDocument htmlA = htmlParse.Parse(eles[0].InnerHtml);
                    List<IElement> spanEles = htmlA.QuerySelectorAll("span").ToList();
                   

                    int page = 0;
                    if (spanEles.Count > 2)
                    {
                        IElement el = spanEles[spanEles.Count - 2];
                        page = int.Parse(el.InnerHtml);
                    }
                    else
                    {
                        page = 1;
                    }


                    List<string> urls = new List<string>();//分页
                    for (int i = 1; i <= page; i++)
                    {
                        string str = url + "pn" + i + "/";
                        urls.Add(str);
                    }

                    foreach (var e in urls)
                    {
                        try
                        {
                            //抓取每页
                            string htmlB = crawler.Crawl(e, Encoding.UTF8);

                            IDocument docuemnt = htmlParse.Parse(htmlB);
                            IElement eleist = docuemnt.QuerySelectorAll("ul").Where(p => p.ClassName == "house-list-wrap")
                                .ToList().FirstOrDefault();
                            IDocument docuementC = htmlParse.Parse(eleist.InnerHtml);
                            List<IElement> eliss = docuementC.QuerySelectorAll("div").Where(p => p.ClassName == "list-info")
                                .ToList();
                            //抓取每条
                            for (int i = 0; i < eliss.Count; i++)
                            {
                                string itemUrl = string.Empty;
                                try
                                {
                                    IDocument documentD = htmlParse.Parse(eliss[i].InnerHtml);

                                    IElement eloo = documentD.QuerySelector("a");
                                    itemUrl = eloo.GetAttribute("href");
                                    var htmlE = crawler.Crawl(itemUrl, Encoding.UTF8);

                                    //开始解析
                                    IDocument documentE = htmlParse.Parse(htmlE);
                                    IElement time = documentE.QuerySelectorAll("div").FirstOrDefault(o => o.ClassName == "other");
                                    string update = time.InnerHtml.Substring(0, time.InnerHtml.IndexOf("<"))
                                        .Replace("发布时间：", "").Trim();
                                    DateTime updateime = DateTime.Parse(DateTime.Parse(update).ToShortDateString());
                                    if (updateime > DateTime.Now.AddMonths(-2))
                                    {
                                        //标题
                                        string InfoTitle = documentE.QuerySelectorAll("h1").FirstOrDefault().InnerHtml.Trim();
                                        //详细内容
                                        string InfoContent = documentE.QuerySelectorAll("div")
                                            .FirstOrDefault(u => u.ClassName == "maincon").InnerHtml.Trim();

                                        //电话
                                        string Phone = documentE.QuerySelectorAll("span")
                                            .FirstOrDefault(u => u.ClassName == "phone").InnerHtml.Trim();
                                        //租金
                                        string rentMoney = documentE.QuerySelectorAll("em")
                                            .FirstOrDefault(u => u.ClassName == "redfont").InnerHtml.Trim();
                                        //面积
                                        string areasize = htmlParse.Parse(documentE.QuerySelectorAll("ul").FirstOrDefault(u => u.ClassName == "info").InnerHtml).QuerySelectorAll("li").ToList()[2].InnerHtml.Replace("面积：", "").Replace("㎡", "").Trim();
                                        //客户名
                                        string customerName = documentE.QuerySelectorAll("a")
                                            .Where(u => u.ClassName == "tx").ToList()[1].InnerHtml.Trim();
                                        var infolilist = htmlParse
                                            .Parse(documentE.QuerySelectorAll("ul").FirstOrDefault(u => u.ClassName == "info")
                                                .InnerHtml).QuerySelectorAll("li");
                                        //区域名字
                                        string AreaName = string.Join(",", htmlParse.Parse(infolilist[0].InnerHtml).QuerySelectorAll("a").Select(p => p.InnerHtml.Trim()).ToList()).Trim();
                                        ShopBegRent shop = new ShopBegRent();
                                        shop.AreaName = AreaName;
                                        shop.AreaId = area.Id.ToString();
                                        shop.InfoContent = InfoContent;
                                        shop.InfoTitle = InfoTitle;
                                        shop.Phone = Phone;
                                        shop.MaxRentMoney = ParseTool.StringToDouble(rentMoney) + 1000;
                                        shop.MinRentMoney = (ParseTool.StringToDouble(rentMoney) - 1000) > 0
                                            ? (ParseTool.StringToDouble(rentMoney) - 1000)
                                            : 0;
                                        shop.Customer = customerName;
                                        shop.UpdateTime = updateime;
                                        if (areasize.Contains("-"))
                                        {
                                            string[] areasizes = areasize.Split('-');
                                            shop.MinArea = ParseTool.StringToDouble(areasizes[0]);
                                            shop.MaxArea = ParseTool.StringToDouble(areasizes[1]);
                                        }
                                        else
                                        {
                                            shop.MinArea = ParseTool.StringToDouble(areasize) - 10 > 0 ? double.Parse(areasize) - 10 : 0;
                                            shop.MaxArea = ParseTool.StringToDouble(areasize) + 10;
                                        }

                                        shop.UpdateTime = updateime;
                                        shop.Id = Guid.NewGuid();
                                        shopbegrepo.Add(shop);
                                        Console.WriteLine(area.Name+"添加了一条商铺求租");
                                        Thread.Sleep(2000);
                                    }
                                }
                                catch (Exception exception)
                                {
                                    errorUrlrepsitory.Add(new ErrorUrl() { Url = itemUrl, UrlType = UrlType.Item });
                                    log.Error(exception.ToString());
                                }
                            }
                        }
                        catch (Exception exception)
                        {
                            errorUrlrepsitory.Add(new ErrorUrl() { Url = e, UrlType = UrlType.Page });
                            log.Error(exception.ToString());
                        }
                    }

                }

                Console.WriteLine("抓取" + area.Name + "求租信息完成");
            }
            catch (Exception e)
            {
                log.Error(e.ToString());
                Console.WriteLine(e);

            }
        }
    }
}
