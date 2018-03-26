using Bll;
using Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Test
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private async void button1_Click(object sender, EventArgs e)
        {
            DemoA demo = new DemoA() {Id = ""};
        }

        public class Demo
        {
            public int Id { get; set; }
        }

        public class DemoA : Demo
        {
            public new string Id { get; set; }
        }

    }
}
