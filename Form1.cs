using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Diagnostics;

namespace hideForm
{
    public partial class Form1 : Form
    {
        private int[] keyID;
        private KeySetting keyst;
        private List<HwndInfo> HwndList;
        private List<HwndInfo> CurrentList;
        public Form1()
        {
            this.keyID = new int[]{1,2};
            this.keyst = new KeySetting { sfsModifiers = 0, svkey = "F8", hfsModifiers = 0, hvkey = "F6" };
            this.HwndList = new List<HwndInfo>();
            this.CurrentList = new List<HwndInfo>();
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            
        }

        private void 退出EToolStripMenuItem_Click(object sender, EventArgs e)
        {
            System.Environment.Exit(0);
        }

        private void notifyIcon1_DoubleClick(object sender, EventArgs e)
        {
            if (WindowState == FormWindowState.Minimized)
            {
                WindowState = FormWindowState.Normal;
                this.Activate();
                this.ShowInTaskbar = true;
                this.notifyIcon1.Visible = false;

            }
            else if (this.Visible == false)
            {
                this.Visible = true;
            }
        }

        private void 后台运行ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            WindowState = FormWindowState.Minimized;
            this.notifyIcon1.Visible = true;
            this.ShowInTaskbar = false;
        }

        private void Form1_SizeChanged(object sender, EventArgs e)
        {
            if (WindowState == FormWindowState.Minimized)
            {
                this.notifyIcon1.Visible = true;
                this.ShowInTaskbar = false;
            }
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = true;
            this.Visible = false;
            this.notifyIcon1.Visible = true;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            this.comboBox3.SelectedIndex = 0;
            this.comboBox4.SelectedIndex = 0;
            this.comboBox5.SelectedIndex = 0;
            this.listView1.Items.Clear();
            timer1.Start();
        }

        private void comboBox3_TextChanged(object sender, EventArgs e)
        {
            this.listView1.Items.Clear();
            IEnumerable<HwndInfo> infos = from h in this.HwndList where h.HWndName.ToLower().IndexOf(this.comboBox3.Text) >= 0 select h;
            foreach (var i in infos)
            {
                ListViewItem item = new ListViewItem();
                item.Text = i.HWnd.ToString();
                item.SubItems.Add(i.HWndName);
                this.listView1.Items.Add(item);
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            this.HwndList.Clear();
            W32api.EnumWindows(delegate(IntPtr hWnd, int LParam)
            {
                StringBuilder sb = new StringBuilder();
                W32api.GetWindowTextW(hWnd, sb, sb.Capacity);
                //下面的内容是用ArrayList来存放当前窗口信息的代码
                HwndInfo info = new HwndInfo { HWnd = hWnd, HWndName = sb.ToString()};
                this.HwndList.Add(info);
                return true;
            }, 0);
        }

        protected override void WndProc(ref Message m)
        {
            switch (m.Msg)
            {
                case 0x0312:
                    if (this.KeyRadio.Checked)
                    {
                        if (m.WParam.ToString().Equals("1"))
                        {
                            IEnumerable<HwndInfo> infos = from h in this.HwndList where h.HWndName.ToLower().IndexOf(this.comboBox3.Text) >= 0 select h;
                            foreach (var i in infos)
                            {
                                bool show1 = W32api.ShowWindow(i.HWnd, 0);
                            }
                        }
                        else if (m.WParam.ToString().Equals("2"))
                        {
                            IEnumerable<HwndInfo> infos = from h in this.HwndList where h.HWndName.ToLower().IndexOf(this.comboBox3.Text) >= 0 select h;
                            foreach (var i in infos)
                            {
                                bool show1 = W32api.ShowWindow(i.HWnd, 5);
                            }
                        }
                    }
                    else
                    {
                        if (m.WParam.ToString().Equals("1"))
                        {
                            HwndInfo info = new HwndInfo();
                            info.HWnd = W32api.GetForegroundWindow();
                            StringBuilder sb = new StringBuilder();
                            W32api.GetWindowTextW(info.HWnd, sb, sb.Capacity);
                            CurrentList.Add(info);
                            bool show1 = W32api.ShowWindow(info.HWnd, 0);
                        }
                        else if (m.WParam.ToString().Equals("2"))
                        {
                            foreach(var i in CurrentList){
                                bool show1 = W32api.ShowWindow(i.HWnd, 5);
                            }
                        }
                    }
                    break;
                default:
                    break;
            }
            base.WndProc(ref m);
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            this.comboBox3.Enabled = false;
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            this.comboBox3.Enabled = true;
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            //下面两行是注销已经注册的快捷键
            bool ret = W32api.UnregisterHotKey(this.Handle, this.keyID[0]); 
            ret = W32api.UnregisterHotKey(this.Handle, this.keyID[1]);
            ret = W32api.RegisterHotKey(this.Handle, this.keyID[0], keyst.hfsModifiers, (Keys)Enum.Parse(typeof(Keys), this.keyst.hvkey.ToUpper()));   //注册隐藏快捷键
            ret = W32api.RegisterHotKey(this.Handle, this.keyID[1], keyst.sfsModifiers, (Keys)Enum.Parse(typeof(Keys), this.keyst.svkey.ToUpper()));   //注册还原快捷键
    }

        private void comboBox4_SelectedIndexChanged(object sender, EventArgs e)
        {
            uint baseVar = 1;
            this.keyst.hfsModifiers = this.comboBox4.SelectedIndex == 0 ? 0 : baseVar << this.comboBox4.SelectedIndex - 1;
            this.toolStripStatusLabel2.Text = "选择/输入功能键 [" + this.comboBox4.Text + "]";
        }

        private void comboBox5_SelectedIndexChanged(object sender, EventArgs e)
        {
            uint baseVar = 1;
            this.keyst.sfsModifiers = this.comboBox5.SelectedIndex == 0 ? 0 : baseVar << this.comboBox5.SelectedIndex - 1;
            this.toolStripStatusLabel2.Text = "选择/输入功能键 [" + this.comboBox5.Text + "]";
        }

        private void comboBox1_TextChanged(object sender, EventArgs e)
        {
            this.keyst.hvkey = this.comboBox1.Text;
            this.toolStripStatusLabel2.Text = "选择/输入普通键 [" + this.keyst.hvkey.ToUpper() + "]";
        }

        private void comboBox2_TextChanged(object sender, EventArgs e)
        {
            this.keyst.svkey = this.comboBox2.Text;
            this.toolStripStatusLabel2.Text = "选择/输入普通键 [" + this.keyst.svkey.ToUpper() + "]";
        }

    }
}
