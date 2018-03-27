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
                using (Crawler craw = new Crawler())
                {
                    string sstr = await craw.Crawl(ConstVar.AreaUrl, Encoding.UTF8);
                }
            }
            catch (Exception exception)
            {
                log.Error(exception.ToString());
            }
        }
 
    }
}
