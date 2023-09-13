using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.IO;
using Microsoft.Office.Interop.Word;
using System.Text;
using System.Threading;
using System.Net.Sockets;

namespace Server
{
    internal class Program
    {
        private static string path = @"C:\VpmtTracker\temp\";
        async static System.Threading.Tasks.Task Main(string[] args)
        {
            // Устанавливаем для сокета локальную конечную точку
            IPHostEntry ipHost = Dns.GetHostEntry("localhost");
            IPAddress ipAddr = ipHost.AddressList[0];
            IPEndPoint ipEndPoint = new IPEndPoint(ipAddr, 11000);

            // Создаем сокет Tcp/Ip
            Socket sListener = new Socket(ipAddr.AddressFamily, SocketType.Stream, ProtocolType.Tcp);

            // Назначаем сокет локальной конечной точке и слушаем входящие сокеты
            try
            {
                sListener.Bind(ipEndPoint);
                sListener.Listen(10);

                // Начинаем слушать соединения
                while (true)
                {
                    Console.WriteLine("Ожидаем соединение через порт {0}", ipEndPoint);

                    // Программа приостанавливается, ожидая входящее соединение
                    Socket handler = sListener.Accept();
                    string data = null;

                    // Мы дождались клиента, пытающегося с нами соединиться

                    byte[] bytes = new byte[1024];
                    int bytesRec = handler.Receive(bytes);

                    data += Encoding.UTF8.GetString(bytes, 0, bytesRec);

                    // Показываем данные на консоли
                    Console.Write("Полученный текст: " + data + "\n\n");

                    // Отправляем ответ клиенту\
                    string reply = "Спасибо за запрос в " + data.Length.ToString()
                            + " символов";
                    byte[] msg = Encoding.UTF8.GetBytes(reply);
                    handler.Send(msg);

                    if (data.IndexOf("<TheEnd>") > -1)
                    {
                        Console.WriteLine("Сервер завершил соединение с клиентом.");
                        break;
                    }

                    handler.Shutdown(SocketShutdown.Both);
                    handler.Close();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            finally
            {
                Console.ReadLine();
            }
            await ParsTime();

        }
        async static System.Threading.Tasks.Task ParsTime()
        {
            while (true) 
            {
                await System.Threading.Tasks.Task.Run(()=> Pars());
                Thread.Sleep(10000);
            }
        }
        static void Pars()
        {
            Directory.Delete(path, true); //true - если директория не пуста (удалит и файлы и папки)
            Directory.CreateDirectory(path);
            string path1 = "";

            Application word = new Application();
            try
            {
                DownloadWord();
                Document file = word.Documents.Open(path + @"rasp.doc");

                List<string> list = new List<string> { "1 СП", "2 СП", "3 СП", "4 СП", "1 ТО", "2 ТО", "3 ТО", "4 АТ", "1 ТМ", "2 ТМ", "3 ТМ", "4 ТМ", "1 ИС", "2 ИС", "3 ИС", "4 ИС", "1 ЭЛ", "2 ЭЛ", "3 ЭЛ", "4 ЭЛ", "1 МО", "2 ОС", "3 ТП", "1 СПП", "1 ЭМ", "2 СВ", "3 МО" };
                char[] ra = new char[] { '\r', '\a' };

                for (int numtable = 1; numtable <= 1; numtable++)
                {
                    try
                    {
                        Table table = file.Tables[numtable];//выбор таблицы 

                        for (int row = 1; row < 14; row += 2)
                        {
                            for (int column = 1; column < table.Rows.Count; column++)
                            {
                                try
                                {
                                    if (list.Contains(table.Cell(column, row / 2 + row % 2).Range.Text.Trim(ra)))//проверка названия подгруппы
                                    {
                                        path1 = path + numtable + "_" + table.Cell(column, row / 2 + row % 2).Range.Text.Trim(ra).Replace(' ', '_') + ".txt";
                                        using (StreamWriter writer = new StreamWriter(path1, false))
                                        {
                                            writer.WriteLine(table.Cell(column, row / 2 + row % 2).Range.Text.Trim(ra));//название подгруппы
                                            column++;
                                            if (table.Cell(column, row + 1).Range.Text.Trim(ra) == "")
                                                column += 2;
                                        }
                                    }
                                    using (StreamWriter writer1 = new StreamWriter(path1, true))
                                    {
                                        writer1.WriteLine(table.Cell(column, row).Range.Text.Trim(ra));//время
                                        writer1.Write(table.Cell(column, row + 1).Range.Text.Trim(ra) + " ");//наименование пары
                                        column++;
                                        writer1.WriteLine(table.Cell(column, row + 1).Range.Text.Trim(ra));//преподаватель пары
                                    }
                                }
                                catch { column++; }
                            }
                        }
                    }
                    catch { numtable++; }
                }
                word.Quit();
            }
            catch (Exception ex) 
            { 
                word.Quit();
                string dirtemp = path;
                Directory.Delete(dirtemp, true); //true - если директория не пуста (удалит и файлы и папки)
                Directory.CreateDirectory(dirtemp);
                Console.WriteLine(ex);
            }
        }
        public static void DownloadWord()//
        {
            WebClient webClient = new WebClient();
            webClient.DownloadFile("http://www.vpmt.ru/docs/rasp.doc", @"C:\VpmtTracker\temp\rasp.doc"); //скачать
        }
    }
}
