using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using Newtonsoft.Json;
using Microsoft.Toolkit.Uwp.Notifications;
using System.Windows;
namespace TCP_IP_Server
{
    class Program
    {
        const string ip = "127.0.0.1";
        const int port = 8080;

        static void Main(string[] args)
        {
            var tcpEndpoint = new IPEndPoint(IPAddress.Parse(ip),port);

            var tcpSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
           
            
            tcpSocket.Bind(tcpEndpoint);
            tcpSocket.Listen(5);

            
            while (true)
            {
                var listener = tcpSocket.Accept();

                var buffer = new byte[256];
                var size = 0;
                var data = new StringBuilder();

                do
                {
                    size = listener.Receive(buffer);
                    data.Append(Encoding.UTF8.GetString(buffer,0,size));
                }
                while (listener.Available > 0);

                var message = data.ToString();

                Console.WriteLine(message);
                //TODO: Дописать вывод сообщения в windows-уведомления

                var deserializedMessage = JsonConvert.DeserializeObject<Order>(message);
                List<string> names = new List<string>();
                foreach (Product p in deserializedMessage.Products)
                {
                    Console.WriteLine($"{p.Name} \n");
                    names.Add(p.Name);
                }
                var notificator = new ToastContentBuilder();
                notificator.AddArgument("action", "viewConversation")
                .AddArgument("conversationId", 9813);
                string notifMessage = "";
                foreach (string name in names)
                {
                    notifMessage += $"{name} \n";
                }

                

                notificator.AddText(notifMessage);
                notificator.Show();

                
                listener.Send(Encoding.UTF8.GetBytes("Заказ доставлен успешно!"));
                listener.Shutdown(SocketShutdown.Both);
                listener.Close();
            }
        }
    }
}
