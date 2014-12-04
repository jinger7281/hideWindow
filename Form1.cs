using System;
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
        public Form1()
        {
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
            foreach (ListViewItem i in this.listView1.Items)
            {
                i.BackColor = Color.White;
                if (i.SubItems[1].Text.IndexOf(this.comboBox3.Text) >= 0 && this.comboBox3.Text.Length > 0)
                {
                    i.BackColor = Color.Red;
                }
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            this.listView1.Items.Clear();
            W32api.EnumWindows(delegate(IntPtr hWnd, int LParam)
            {
                StringBuilder sb = new StringBuilder();
                ListViewItem item = new ListViewItem();
                item.Text = hWnd.ToString();
                W32api.GetWindowTextW(hWnd, sb, sb.Capacity);
                item.SubItems.Add(sb.ToString());
                this.listView1.Items.Add(item);
                return true;
            }, 0);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            bool ret = W32api.RegisterHotKey(this.Handle, 1, 0, Keys.F6);
            bool ret1 = W32api.RegisterHotKey(this.Handle, 2, 0, Keys.F8);
        }

        protected override void WndProc(ref Message m)
        {
            switch (m.Msg)
            {
                case 0x0312:
                    Console.WriteLine("事件响应成功----[{0}]",m.WParam.ToString());
                    if (m.WParam.ToString().Equals("1"))
                    {
                        foreach (ListViewItem i in this.listView1.Items)
                        {
                            if (i.SubItems[1].Text.IndexOf(this.comboBox3.Text) >= 0 && this.comboBox3.Text.Length > 0)
                            {
                                bool show1 = W32api.ShowWindow(new IntPtr(Convert.ToUInt32(i.Text)), 0);
                                Console.WriteLine("句柄{0}--隐藏窗口返回{1}", new IntPtr(Convert.ToUInt32(i.Text)), show1);
                            }
                        }
                    }
                    else if (m.WParam.ToString().Equals("2"))
                    {
                        foreach (ListViewItem i in this.listView1.Items)
                        {
                            if (i.SubItems[1].Text.IndexOf(this.comboBox3.Text) >= 0 && this.comboBox3.Text.Length > 0)
                            {
                                bool show2 = W32api.ShowWindow(new IntPtr(Convert.ToUInt32(i.Text)), 5);
                                Console.WriteLine("句柄{0}--隐藏窗口返回{1}", new IntPtr(Convert.ToUInt32(i.Text)), show2);
                            }
                        }
                    }
                    break;
                default:
                    Console.WriteLine("收到系统消息---0x{0:x4}",m.WParam.ToInt32());
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

    }
}
