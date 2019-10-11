using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApplication网口通讯服务器
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }
        Socket Soc;
        Socket SerClienSock;
        Thread th;
        Thread rec;
        byte[] buffer = new byte[1024];
        private void button1_Click(object sender, EventArgs e)
        {













            IPAddress ip = IPAddress.Parse(textBox1.Text);
            Soc = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            Soc.Bind(new IPEndPoint(ip, 3000));
            Soc.Listen(10);
            th = new Thread(ListenThread);
            th.Start();
            Control.CheckForIllegalCrossThreadCalls = false;
        }

        public void ListenThread()
        {
            while (true)
            {
                SerClienSock = Soc.Accept();
                rec = new Thread(RecData);
                rec.Start(SerClienSock);
            }
        }
        public void RecData(object a)
        {
            Socket Clien = (Socket)a;
            int num = SerClienSock.Receive(buffer);
            if(num>0)
            {
                textBox3.Text += string.Format("客户端{0}，{1}",Clien.RemoteEndPoint.ToString(), Encoding.UTF8.GetString(buffer, 0, buffer.Length));
                textBox3.Text += "\n\t";
            }
            else
            {
                MessageBox.Show("数据接收异常");
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if(textBox2.Text!="")
            {
                byte[] buffer = Encoding.UTF8.GetBytes(textBox2.Text);
                SerClienSock.Send(buffer);
            }
            else
            {
                MessageBox.Show("发送数据为空");
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            th.Abort();
            rec.Abort();
            SerClienSock.Close();
            Process.GetCurrentProcess().Kill();
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            SerClienSock.Close();
            Process.GetCurrentProcess().Kill();
        }


    }
}
