using Domain;
using Helper;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Model;

namespace App
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private LogHelper log = new LogHelper(typeof(Form1));
        private async void button1_Click(object sender, EventArgs e)
        {
            try
            {
                button1.Enabled = false;
                AreaSpider spider = new AreaSpider();
                await spider.Crawl();
                button1.Enabled = true;
            }
            catch (Exception exception)
            {
                log.Error(exception.ToString());
            }
        }

        private async void button2_ClickAsync(object sender, EventArgs e)
        {
            try
            {
                List<Area> areas = await SqlHelper.GetEntity<Area>(p => p.Url != string.Empty);
                List<Task> list = new List<Task>();
                TaskFactory factory = new TaskFactory();
                button2.Enabled = false;

                CrawlShop crawlShop = new CrawlShop();
                areas.ForEach(p =>
              {
                  list.Add(factory.StartNew(() => { crawlShop.CrawlData(p); }));

              });


                Task.WaitAll(list.ToArray());

                Console.WriteLine("Over");
                button2.Enabled = true;
            }
            catch (Exception exception)
            {
                log.Error(exception.ToString());
            }
            finally
            {

            }
        }

        public void Test()
        {
            for (int i = 0; i < 1000; i++)
            {
                Console.WriteLine("adf");
            }
        }

        private async void Form1_Load(object sender, EventArgs e)
        {
            List<Area> areas = await SqlHelper.GetEntity<Area>(p => true);

            foreach (var item in areas.Where(p => p.Url == string.Empty))
            {
                var Node = new TreeNode()
                {
                    Text = item.Name,
                    Name = item.Name, 
                };
                foreach (var ChildItem in areas.Where(p => p.ParentId == item.Id.ToString()))
                {
                    var child = new TreeNode()
                    {
                        Text = ChildItem.Name,
                        Tag = ChildItem.Url
                    };
                    Node.Nodes.Add(child);
                }

                this.treeView1.Nodes.Add(Node);
            }

        }
    }
}
