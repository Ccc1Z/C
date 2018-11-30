using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Client
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        Socket socketClient;
        private void btnConnect_Click(object sender, EventArgs e)
        {
            socketClient = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            IPAddress ip = IPAddress.Parse(txtIP.Text);
            IPEndPoint point = new IPEndPoint(ip, Convert.ToInt32(txtPort.Text));
            socketClient.Connect(point);
            ShowMsg("连接成功");
            
        }
        void ShowMsg(string str)
        {
            txtLog.AppendText(str + "\r\n");
        }

        private void btnSend_Click(object sender, EventArgs e)
        {
            string str = txtSend.Text;
            byte[] buffer = Encoding.UTF8.GetBytes(str);
            socketClient.Send(buffer);
            txtSend.Clear();
        }
    }
}
