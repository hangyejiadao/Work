
using Dal;
using Helper;
using Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Model.BaseModel;
using Newtonsoft.Json;

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
            ErrorUrlRepository errorUrlRepository = new ErrorUrlRepository();
            //  errorUrlRepository.Add(new ErrorUrl() { Url = "http://www.baidu.com", UrlType = UrlType.Item });
            string str = File.ReadAllText(AppDomain.CurrentDomain.BaseDirectory + "Json.txt");
            var obj = JsonConvert.DeserializeObject<Rootobject>(str);

        }

        public enum Test
        {
            A = 0
        }
    }
}
