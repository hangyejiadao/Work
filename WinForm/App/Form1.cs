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
using Dal;

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
            this.contextMenuStrip1.Show(new Point(123, 456));
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
                        Tag = ChildItem.Url,
                        Name = ChildItem.Id.ToString()
                    };
                    Node.Nodes.Add(child);
                }

                this.treeView1.Nodes.Add(Node);
            }

        }

        private string SelectId = string.Empty;

        private void treeView1_MouseDown(object sender, MouseEventArgs e)
        {

            if (e.Button == MouseButtons.Right)
            {
                Point ClickPoint = new Point(e.X, e.Y);
                TreeNode CurrentNode = treeView1.GetNodeAt(ClickPoint);
                if (CurrentNode.Tag != null)//判断你点的是不是一个节点
                {
                    SelectId = CurrentNode.Name;
                    CurrentNode.ContextMenuStrip = contextMenuStrip1;
                }
                treeView1.SelectedNode = CurrentNode;//选中这个节点
            }
        }

        private AreaRepository arearepsository = new AreaRepository();
        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {
            Area area = arearepsository.GetEntity(p => p.Id == int.Parse(SelectId)).Result.FirstOrDefault();
            try
            {
                CrawlShop shop = new CrawlShop();
                shop.CrawlAll(area);
            }
            catch (Exception exception)
            {
                log.Error(exception.ToString());
            }
        }

        /// <summary>
        /// 抓出租
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void toolStripMenuItem2_Click(object sender, EventArgs e)
        {
            Area area = arearepsository.GetEntity(p => p.Id == int.Parse(SelectId)).Result.FirstOrDefault();
            try
            {
                CrawlShop shop = new CrawlShop();
                shop.CrawlDataCz(area);
            }
            catch (Exception exception)
            {
                log.Error(exception.ToString());
            }
        }

        /// <summary>
        /// 转让
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void toolStripMenuItem3_Click(object sender, EventArgs e)
        {
            Area area = arearepsository.GetEntity(p => p.Id == int.Parse(SelectId)).Result.FirstOrDefault();
            try
            {
                CrawlShop shop = new CrawlShop();
                shop.CrawlDataZr(area);
            }
            catch (Exception exception)
            {
                log.Error(exception.ToString());
            }
        }


        /// <summary>
        /// 求租
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void toolStripMenuItem4_Click(object sender, EventArgs e)
        {
            Area area = arearepsository.GetEntity(p => p.Id == int.Parse(SelectId)).Result.FirstOrDefault();
            try
            {
                CrawlShop shop = new CrawlShop();
                shop.CrawlDataBegRent(area);
            }
            catch (Exception exception)
            {
                log.Error(exception.ToString());
            }
        }
    }
}

