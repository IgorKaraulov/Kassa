using Test_Kiosk_MVVM.ViewModels;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Test_Kiosk_MVVM.NetworkInteraction
{
  public  class OrderMessager
    {
        private string ip;
        private int port;

        public OrderMessager()
        {
            ip = ConfigurationManager.AppSettings.Get("ServerHost");
            port = int.Parse(ConfigurationManager.AppSettings.Get("ServerPort"));
        }

        public  string SendMessage(string message)
        {
            var tcpEndPoint = new IPEndPoint(IPAddress.Parse(ip), port);

            var tcpSocket = new Socket(AddressFamily.InterNetwork,SocketType.Stream,ProtocolType.Tcp);
            var buffer = new byte[256];
            var size = 0;
            var answer = new StringBuilder();

            var data = Encoding.UTF8.GetBytes(message);
            try
            {
               tcpSocket.Connect(tcpEndPoint);
               tcpSocket.Send(data);
                
                do
                {
                    size = tcpSocket.Receive(buffer);
                    answer.Append(Encoding.UTF8.GetString(buffer, 0, size));
                }
                while (tcpSocket.Available > 0);

                tcpSocket.Shutdown(SocketShutdown.Both);
                tcpSocket.Close();
            }
            catch (Exception e)
            {
                
                ApplicationViewModel.logger.Error("Ошибка при отправке сообщения на сервер. \n" + e.Message);
                return e.Message;
            }
           return answer.ToString();
        }
    }
}
