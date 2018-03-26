using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using AngleSharp.Parser.Html;
using Helper;
using log4net;
 

namespace WinForm
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
           
        }
    }
}
