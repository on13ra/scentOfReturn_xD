using Microsoft.Office.Interop.Word;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace Server
{
    internal class Program
    {
        private static string currentDirectory = Directory.GetCurrentDirectory();  // сюда загрузятся все файлы (Server/bin/Debug)

        // Задайте имя папки, которую вы хотите создать
        private static string folderName = "rasptemp\\";

        // Полный путь к новой папке
        private static string path = Path.Combine(currentDirectory, folderName);

        //async static System.Threading.Tasks.Task Maincraft()
         static void Maincraft()
        {
            //await ParsTime();
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
        }
        //async static System.Threading.Tasks.Task ParsTime()
        //{
        //    List<string> time = new List<string>() {"17:05:00", "19:05:00", "6:00:00" };//время обновления расписания
        //    while (true) 
        //    {
        //        if (time.Contains(DateTime.Now.ToLongTimeString()))
        //            await System.Threading.Tasks.Task.Run(()=> Pars());
        //    }
        //}
        static void Main()
        {
           Maincraft();
            try
            {
                // Проверьте, существует ли папка, прежде чем создавать её
                if (!Directory.Exists(path))
                {
                    // Создайте папку
                    Directory.CreateDirectory(path);
                    Console.WriteLine("Папка успешно создана.");
                }
                else
                {
                    Console.WriteLine("Папка уже существует.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Произошла ошибка: " + ex.Message);
            }

            Directory.Delete(path, true); //true - если директория не пуста (удалит и файлы и папки)
            Directory.CreateDirectory(path);
            string path1 = "";

            Application word = new Application();
            try
            {
                DownloadWord();
                Document file = word.Documents.Open(Path.Combine(path,"rasp.doc")); //тут откроется расписание файл 

                List<string> list = new List<string> { "1 СП", "2 СП", "3 СП", "4 СП", "1 ТО", "2 ТО", "3 ТО", "4 АТ", "1 ТМ", "2 ТМ", "3 ТМ", "4 ТМ", "1 ИС", "2 ИС", "3 ИС", "4 ИС", "1 ЭЛ", "2 ЭЛ", "3 ЭЛ", "4 ЭЛ", "1 МО", "2 ОС", "3 ТП", "1 СПП", "1 ЭМ", "2 СВ", "3 МО" };
                char[] ra = new char[] { '\r', '\a' };

                for (int numtable = 1; numtable <= 1; numtable++)
                {
                    try
                    {
                        Table table = file.Tables[numtable]; //выбор таблицы 

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
            webClient.DownloadFile("http://www.vpmt.ru/docs/rasp.doc", Path.Combine(path, "rasp.doc")); //сюда скачается расписание с именем rasp.doc
        }
    }
}
