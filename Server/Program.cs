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

        async static System.Threading.Tasks.Task Main()
        {
            await ParsTime();
            UdpClient udpServer = new UdpClient(11000);
            while (true)
            {
                var remoteEP = new IPEndPoint(IPAddress.Any, 11000);
                if (remoteEP == null)
                {
                    var data = udpServer.Receive(ref remoteEP); ; // listen on port 11000
                    string line = Encoding.UTF8.GetString(data);
                    Console.WriteLine(line);
                    string fileName = GetFileNameForMessage(line);
                    string currentFile = Path.Combine(path, fileName);
                    string lines = File.ReadAllText(currentFile, Encoding.UTF8);
                    byte[] responseBytes = Encoding.UTF8.GetBytes(lines);
                    udpServer.Send(responseBytes, responseBytes.Length,remoteEP); // reply back
                }
            }
        }

        // Определяем имя файла на основе сообщения от клиента
        static string GetFileNameForMessage(string message)
        {
            switch (message.Trim())
            {
                case "4EL":
                    return "1_4_ЭЛ.txt";
                case "4TM":
                    return "1_4_ТМ.txt";
                case "4SP":
                    return "1_4_СП.txt";
                case "4IS":
                    return "1_4_ИС.txt";
                case "3EL":
                    return "1_3_ЭЛ.txt";
                case "3TO":
                    return "1_3_ТО.txt";
                case "3TM":
                    return "1_3_ТМ.txt";
                case "3SP":
                    return "1_3_СП.txt";
                case "3IS":
                    return "1_3_ИС.txt";
                case "2EL":
                    return "1_2_ЭЛ.txt";
                case "2TO":
                    return "1_2_ТО.txt";
                case "2TM":
                    return "1_2_ТМ.txt";
                case "2SP":
                    return "1_2_СП.txt";
                case "2IS":
                    return "1_2_ИС.txt";
                case "1EL":
                    return "1_1_ЭЛ.txt";
                case "1TO":
                    return "1_1_ТО.txt";
                case "1TM":
                    return "1_1_ТМ.txt";
                case "1SP":
                    return "1_1_СП.txt";
                case "1IS":
                    return "1_1_ИС.txt";
                default:
                    return "cock"; // Если сообщение не соответствует файлу, вернуть null
            }
        }
        async static System.Threading.Tasks.Task ParsTime()
        {
            Pars();
            List<string> time = new List<string>() { "17:05:00", "19:05:00", "6:00:00" };//время обновления расписания
            while (true)
            {
                if (time.Contains(DateTime.Now.ToLongTimeString()))
                    await System.Threading.Tasks.Task.Run(() => Pars());
            }
        }
        static void Pars()
        {
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

                List<string> list = new List<string> 
                { 
                    "1 СП", "2 СП", "3 СП", "4 СП",
                    "1 ТО", "2 ТО", "3 ТО", "4 ТО",
                    "4 АТ", 
                    "1 ТМ", "2 ТМ", "3 ТМ", "4 ТМ",
                    "1 ИС", "2 ИС", "3 ИС", "4 ИС",
                    "1 ЭЛ", "2 ЭЛ", "3 ЭЛ", "4 ЭЛ", 
                    "1 ТП","2 ТП","3 ТП", "4 ТП",
                    "1 СПП",
                    "1 ЭМ", "2 ЭМ", "3 ЭМ", "4 ЭМ",
                    "1 СВ","2 СВ", "3 СВ", "4 СВ",
                    "1 МО","2 МО", "3 МО", "4 МО",
                    "1 ПКД", "2 ПКД", "3 ПКД", "4 ПКД",
                    "1 ОС", "2 ОС", "3 ОС", "4 ОС",
                    "1 НС", "2 НС", "3 НС", "4 НС"
                };
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
                                        path1 = path + numtable + "_" + table.Cell(column, row / 2 + row % 2).Range.Text.Trim(ra).Replace(" ", "") + ".txt";
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
