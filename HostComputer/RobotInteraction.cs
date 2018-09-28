using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using System.Windows;


namespace HostComputer
{
    public class RobotInteraction
    {  
        //public static float[] Target_JPos = new float[6];  //运动的目标位置
        //public static float[] Target_JVel = new float[6];  //以目标速度运动

        public static string[] JPos = new string[10] { "wz0.0", "bz0.0", "mz0.0", "hz0.0", "sz0.0", "cz0.0", "Az0.0", "Bz0.0", "Cz0.0", "Dz0.0" };
       // public static string[] JVel = new string[6] { "w10", "b12.3", "m10", "s10", "r10", "c10" };
        UdpClient udpClient;
        IPAddress remoteIpAdress;
        IPEndPoint remoteIpEndPoint;
        #region UDP通信设置
        //自动取得本机IP
        private string GetLocalIp()
        {
            string hostname = Dns.GetHostName();
            IPHostEntry localhost = Dns.GetHostByName(hostname);
            IPAddress localaddr = localhost.AddressList[0];
            return localaddr.ToString();
        }
        public void UdpClient()
        {
            udpClient = new UdpClient();
            remoteIpAdress = IPAddress.Parse(GetLocalIp());
            //remoteIpAdress = IPAddress.Parse(IP);
            remoteIpEndPoint = new IPEndPoint(remoteIpAdress, 10000);
        }
        #endregion
        #region 对下位机发回来的数据处理
        //处理下位机发回来的速度信息
        public static string Volvalue_Decide(double value)
        {
            double val;
            string stringval;
            val = 6 * value;     //将转速转化为角速度
            if (!val.ToString().Contains("."))
            {
                stringval = val.ToString() + ".0";

            }
            else
            {
                int index = val.ToString().IndexOf(".");
                stringval = val.ToString().Substring(0, index + 3);
            }
            return stringval;
        }
       //处理下位机发回来的位置信息
        public static string Posvalue_Decide(double value)
        {
            string val;
            string stringval;
            if (value >= 0)
            {
                val = "z";
                if (!value.ToString().Contains("."))
                {
                    stringval = value.ToString() + ".0";

                }
                else
                {
                    int index = value.ToString().IndexOf(".");
                    stringval = value.ToString().Substring(0, index + 2);
                }
                val += stringval;

            }
            else
            {
                val = "f";
                value = System.Math.Abs(value);
                if (!value.ToString().Contains("."))
                {
                    stringval = value.ToString() + ".0";

                }
                else
                {
                    int index = value.ToString().IndexOf(".");
                    stringval = value.ToString().Substring(0, index + 2);
                }
                val += stringval;

            }
            return val;
        }
        #endregion
        #region 给U3D发送信息函数
        public void Udp_SendMessage(string[] JPos)
        {
            string sendMessageBytes = null;
            //string[] sendMessage = new string[2] { "", "" };
            //sendMessage[0] = string.Join("|", JPos);
            //sendMessage[1] = string.Join("|", JVel);
            sendMessageBytes = string.Join("|",JPos);
            //MessageBox.Show(sendMessageBytes);
            //commandAnalyse(sendMessageBytes);
            //analyseMessage(Target_JPos, Target_JVel);
            byte[] sendBytes = System.Text.Encoding.Unicode.GetBytes(sendMessageBytes);
            udpClient.Send(sendBytes, sendBytes.Length, remoteIpEndPoint);
        }
        #endregion
        #region 给U3D发送指令函数（寻零）
        public void Udp_SendMessageInt(string str)
        {
            byte[] sendBytes = System.Text.Encoding.Unicode.GetBytes(str);
            udpClient.Send(sendBytes, sendBytes.Length, remoteIpEndPoint);
        }
        #endregion

    }
}
