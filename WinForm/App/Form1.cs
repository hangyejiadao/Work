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

        private void button2_Click(object sender, EventArgs e)
        {
            try
            {

            }
            catch (Exception exception)
            {
                log.Error(exception.ToString());
            }
        }
    }
}
