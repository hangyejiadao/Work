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
        private ImageRepository imgrepo = new ImageRepository();
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

                    IHtmlDocument htmlA = htmlParse.Parse(eles[0].InnerHtml);
                    List<IElement> spanEles = htmlA.QuerySelectorAll("span").ToList();
                    IElement el = spanEles[spanEles.Count - 2];
                    int page = int.Parse(el.InnerHtml);
                    List<string> urls = new List<string>();
                    for (int i = 1; i <= page; i++)
                    {
                        string str = url + "pn" + i + "/";
                        urls.Add(str);
                    }

                    urls.ForEach(e =>
                 {
                     Crawler crawlerA = new Crawler();
                     string htmlB = crawlerA.Crawl(e, Encoding.UTF8);

                     IDocument docuemnt = htmlParse.Parse(htmlB);
                     IElement eleist = docuemnt.QuerySelectorAll("ul").Where(p => p.ClassName == "house-list-wrap").ToList().FirstOrDefault();


                     IDocument docuementC = htmlParse.Parse(eleist.InnerHtml);
                     List<IElement> eliss = docuementC.QuerySelectorAll("div").Where(p => p.ClassName == "pic").ToList();

                     eliss.ForEach(p =>
                      {
                          try
                          {

                              IDocument documentD = htmlParse.Parse(p.InnerHtml);

                              IElement eloo = documentD.QuerySelector("a");
                              var htmlE = crawler.Crawl(eloo.GetAttribute("href").ToString(), Encoding.UTF8);


                              IDocument documentE = htmlParse.Parse(htmlE);
                              IElement ele = documentE.QuerySelectorAll("span").Where(o => o.InnerHtml.StartsWith("更新于")).FirstOrDefault();
                              DateTime time = DateTime.Parse(ele.InnerHtml.Replace("更新于", ""));
                              if (time > DateTime.Now.AddMonths(-2))
                              {
                                  IElement InfoTitleElee = documentE.QuerySelectorAll("h1")
                                      .FirstOrDefault(o => o.ClassName == "c_000 f20");

                                  IElement money = documentE.QuerySelectorAll("span").FirstOrDefault(o => o.ClassName == "house_basic_title_money_num");




                                  //var address = documentE.QuerySelectorAll("a").Where(o =>
                                  //    o.ClassName == "house_basic_title_content_item3 blue-link").ToList()[2];
 


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
                                      UpdateTime= time
                                  };
                                  var imgUl = documentE.QuerySelectorAll("ul")
                                         .FirstOrDefault(o => o.ClassName == "general-pic-list");
                                  IDocument documentf = htmlParse.Parse(imgUl.InnerHtml);
                                  shoprepo.Add(shoptransfer);
                                  documentf.QuerySelectorAll("img").Select(o => o.GetAttribute("src")).ToList().ForEach(
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
                                              Thread.Sleep(2000);

                                          }
                                      });

                                  Thread.Sleep(5000);

                              }

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


        /// <summary>
        /// 抓取转让信息
        /// </summary>
        /// <param name="area"></param>
        public void CrawlDataZr(Area area)
        {
            try
            {
                Crawler crawler = new Crawler();
                string url = area.Url + ConstVar.转让 + "0/";
                string html = crawler.Crawl(url, Encoding.UTF8);
                var htmlParse = new HtmlParser();
                IHtmlDocument docuement = htmlParse.Parse(html);
                List<IElement> eles = docuement.QuerySelectorAll("div").ToList().Where(p => p.ClassName == "pager").ToList();
                if (eles.Count > 0)
                {

                    IHtmlDocument htmlA = htmlParse.Parse(eles[0].InnerHtml);
                    List<IElement> spanEles = htmlA.QuerySelectorAll("span").ToList();
                    IElement el = spanEles[spanEles.Count - 2];
                    int page = int.Parse(el.InnerHtml);
                    List<string> urls = new List<string>();
                    for (int i = 1; i <= page; i++)
                    {
                        string str = url + "pn" + i + "/";
                        urls.Add(str);
                    }

                    urls.ForEach(e =>
                    {
                        Crawler crawlerA = new Crawler();
                        string htmlB = crawlerA.Crawl(e, Encoding.UTF8);

                        IDocument docuemnt = htmlParse.Parse(htmlB);
                        IElement eleist = docuemnt.QuerySelectorAll("ul").Where(p => p.ClassName == "house-list-wrap").ToList().FirstOrDefault();


                        IDocument docuementC = htmlParse.Parse(eleist.InnerHtml);
                        List<IElement> eliss = docuementC.QuerySelectorAll("div").Where(p => p.ClassName == "pic").ToList();

                        eliss.ForEach(p =>
                        {
                            try
                            {

                                IDocument documentD = htmlParse.Parse(p.InnerHtml);

                                IElement eloo = documentD.QuerySelector("a");
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
                                    documentf.QuerySelectorAll("img").Select(o => o.GetAttribute("src")).ToList().ForEach(
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
                                                Thread.Sleep(2000);

                                            }
                                        });

                                    Thread.Sleep(5000);
                                }

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



        /// <summary>
        /// 抓取求租
        /// </summary>
        /// <param name="area"></param>
        public void CrawlDataBegRent(Area area)
        {
            
        }
    }
}
