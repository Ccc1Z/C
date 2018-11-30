using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SocketServer
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        Socket socketServer;//负责通信的Socket

        private void btnListen_Click(object sender, EventArgs e)
        {
            //创建新的Socket对象，并且绑定IP地址，端口号
            Socket socketStart = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            IPAddress ip = IPAddress.Any;
            IPEndPoint point = new IPEndPoint(ip, Convert.ToInt32(txtPort.Text));
            socketStart.Bind(point);
            //设置监听队列，开始监听
            socketStart.Listen(10);
            ShowMsg("开始监听");
            Thread thdListen = new Thread(Listen);
            thdListen.IsBackground = true;
            thdListen.Start(socketStart);
        }
        void ShowMsg(string str)
        {
            txtLog.AppendText(str + "\r\n");
        }
        void Listen(object o)
        {
            Socket socketStart = o as Socket;
            try
            {
                while (true)
                {
                    socketServer = socketStart.Accept();
                    ShowMsg(socketServer.RemoteEndPoint.ToString() + ":" + "连接成功");
                    Thread thdReceive = new Thread(Receive);
                    thdReceive.IsBackground = true;
                    thdReceive.Start(socketServer);
                }
            }
            catch { }
        }
        void Receive(object o)
        {
            socketServer = o as Socket;
            try
            {
                while(true)
                {
                    byte[] buffer = new byte[1024 * 1024 * 3];
                    int r = socketServer.Receive(buffer);
                    if (r == 0)
                    {
                        break;
                    }
                    string str = Encoding.UTF8.GetString(buffer);
                    ShowMsg(socketServer.RemoteEndPoint.ToString() + ":" + str);
                }
            }
            catch { }
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            Control.CheckForIllegalCrossThreadCalls = false;
        }
    }
}
