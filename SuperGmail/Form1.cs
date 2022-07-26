using OpenQA.Selenium.Chrome;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using OpenQA.Selenium;
using System.Threading;

namespace SuperGmail
{
    public partial class Form1 : Form
    {
        bool isStop = false;
        public static List<Thread> threads = new List<Thread>();
        private int getWidthChrome;
        private int getHeightChrome;
        public int getWidthScreen;
        public int getHeightScreen;
        List<int> listPossitionApp = new List<int>();
        public Form1()
        {
            InitializeComponent();
            Control.CheckForIllegalCrossThreadCalls = false;
            Helper.getHeightScreen = Screen.PrimaryScreen.Bounds.Height;
            Helper.getWidthScreen = Screen.PrimaryScreen.Bounds.Width;
        }
        private int GetIndexOfPossitionApp()
        {
            int indexPos = 0;
            lock (listPossitionApp)
            {
                for (int i = 0; i < listPossitionApp.Count; i++)
                {
                    if (listPossitionApp[i] == 0)
                    {
                        indexPos = i;
                        listPossitionApp[i] = 1;
                        break;
                    }
                }
            }
            return indexPos;
        }

        private void FillIndexPossition(int indexPos)
        {
            lock (listPossitionApp)
            {
                listPossitionApp[indexPos] = 0;
            }
        }
        void LoginGmail(int indexPos)
        {
            Point loca = Helper.GetPointFromIndexPosition(indexPos, 10);
            var driverExecutablePath = $"{Directory.GetCurrentDirectory()}\\chromedriver.exe";
            var options = new ChromeOptions();
            options.AddArgument("--mute-audio");
            options.AddArgument("--disable-gpu");
            options.AddArgument("--disable-dev-shm-usage");
            options.AddArgument("--window-size=" + getWidthChrome + "," + getHeightChrome);
            options.AddArgument("--window-position=" + loca.X + "," + loca.Y);
           
            var driver = Chrome.Create(
                options: options,
                driverExecutablePath: driverExecutablePath,
                browserExecutablePath: "C:\\Program Files\\Google\\Chrome\\Application\\chrome.exe");
            driver.Manage().Timeouts().PageLoad = TimeSpan.FromSeconds(60);
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(1.0);
            //driver.GoToUrl("https://accounts.google.com/signin");
            //driver.FindElement(By.Name("identifier")).SendKeys("#VoThanhMinhHien");
            //Thread.Sleep(1000);
            //driver.FindElement(By.XPath("//span[text()=\"Tiếp theo\"]")).Click();
        }
        void Test(int index,string value)
        {
            dataGridView1.Rows[index].Cells[0].Value = value;
            Thread.Sleep(5000);
        }
        private void button1_Click(object sender, EventArgs e)
        {
            isStop = false;
            int maxThread = Convert.ToInt32(numThread.Value);
            getWidthChrome = (2 * Helper.getWidthScreen) / 10;
            getHeightChrome = Helper.getHeightScreen / 2;
            for (int i = 0; i < maxThread; i++)
                listPossitionApp.Add(0);
            new Thread(() =>
            {
                if (isStop)
                    return;
                foreach (var line in dataGridView1.Rows.Cast<DataGridViewRow>())
                {
                    if (isStop)
                        return;
                    Thread xyz = new Thread(() =>
                    {
                        var index = dataGridView1.Rows.Add();
                        int indexPos = GetIndexOfPossitionApp();
                        LoginGmail(indexPos);
                        FillIndexPossition(indexPos);
                    });
                    threads.Add(xyz);
                    xyz.Start();
                    xyz.IsBackground = true;
                    Thread.Sleep(700);
                    maxThread--;
                    if (maxThread < 1)
                    {
                        foreach (Thread thread in threads)
                            thread.Join();
                        threads.Clear();
                        maxThread = Convert.ToInt32(numThread.Value);
                    }
                    //foreach (var lines in richTextBox1.Lines)
                    //{
                    //    if (isStop)
                    //        return;
                    //    Thread xyz = new Thread(() =>
                    //    {
                    //        var index = dataGridView1.Rows.Add();
                    //        int indexPos = GetIndexOfPossitionApp();
                    //        LoginGmail(indexPos);
                    //        FillIndexPossition(indexPos);
                    //    });
                    //    threads.Add(xyz);
                    //    xyz.Start();
                    //    xyz.IsBackground = true;
                    //    Thread.Sleep(700);
                    //    maxThread--;
                    //    if (maxThread < 1)
                    //    {
                    //        foreach (Thread thread in threads)
                    //            thread.Join();
                    //        threads.Clear();
                    //        maxThread = Convert.ToInt32(numThread.Value);
                    //    }
                    //}

                }



            }).Start();
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            isStop = true;
        }

        private void numThread_ValueChanged(object sender, EventArgs e)
        {
            if(numThread.Value > richTextBox1.Lines.Count())
            {
                MessageBox.Show("Số luồng không thể lớn hơn số xproxy", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                numThread.Value = 1;
            }
        }
    }
}
