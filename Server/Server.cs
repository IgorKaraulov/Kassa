using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using Newtonsoft.Json;
using System.Windows;
using NLog;

namespace TCP_IP_Server
{
    class Server
    {
        const string ip = "127.0.0.1";
        const int port = 8080;

        private static Logger logger = LogManager.GetCurrentClassLogger();

        static void Main(string[] args)
        {
            logger.Info("Старт программы сервера");
            try
            {
                logger.Info($"Инициализируем сервер с ip {ip} и портом {port} ");
                var tcpEndpoint = new IPEndPoint(IPAddress.Parse(ip), port);
                var tcpSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

                try
                {

                    tcpSocket.Bind(tcpEndpoint);
                    tcpSocket.Listen(5);
                }
                catch (Exception e)
                {
                    logger.Error("Ошибка бинда сокета \n" + e.Message);
                    return;
                }

                while (true)
                {
                    var listener = tcpSocket.Accept();

                    var buffer = new byte[256];
                    var size = 0;
                    var data = new StringBuilder();
                    try
                    {
                        logger.Info("Пытаемся принять сообщение");
                        do
                        {
                            size = listener.Receive(buffer);
                            data.Append(Encoding.UTF8.GetString(buffer, 0, size));
                        }
                        while (listener.Available > 0);
                    }
                    catch (Exception e)
                    {
                        logger.Error("Ошибка принятия сообщения \n" + e.Message);
                        return;
                    }
                    try
                    {
                        logger.Info("Deserialize message");
                        var message = data.ToString();

                        Console.WriteLine(message);

                        var deserializedMessage = JsonConvert.DeserializeObject<Order>(message);
                        List<string> names = new List<string>();
                        foreach (Product p in deserializedMessage.Products)
                        {
                            Console.WriteLine($"{p.Name} \n");
                            names.Add(p.Name);
                        }

                        string notifMessage = "";
                        foreach (string name in names)
                        {
                            notifMessage += $"{name} \n";
                        }
                        MessageBox.Show($"Заказ получен: \n {notifMessage}");
                        logger.Info($"Заказ получен: \n {notifMessage}");
                    }
                    catch (Exception e)
                    {
                        logger.Error("Не смогли десериализовать сообщение \n" + e.Message);
                    }

                    try
                    {
                        logger.Info("Пытаемся отправить ответ");
                        listener.Send(Encoding.UTF8.GetBytes("Заказ доставлен успешно!"));
                    }
                    catch (Exception e)
                    {
                        logger.Error("Не получилось отправить ответ \n" + e.Message);
                    }

                    try
                    {
                        logger.Info("Пытаемся закрыть сокет");
                        listener.Shutdown(SocketShutdown.Both);
                        listener.Close();
                    }
                    catch (Exception e)
                    {
                        logger.Error("Закрыть сокет не получилось \n" + e.Message);
                    }
                }
            }
            catch (Exception e)
            {
                logger.Error("Ошибка инициализации сервера. \n" + e.Message);
            }
        }
    }
}
