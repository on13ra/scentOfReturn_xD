using scentOfReturn_xD.Pages;
using System.Windows.Input;

namespace scentOfReturn_xD

{

    public partial class MainPage : ContentPage
    {


        //static string tempPath = System.IO.Path.GetTempPath();
        
        public MainPage()
        {
            InitializeComponent();
            string gdText = GroupDisplay.Text.ToString();
            if (gdText.Contains("CurrentGroup"))
            {
                GroupDisplay.Text = "Выберите группу";
            }
            else GroupDisplay.Text = "hawaii";
            //ICommand TappedToChoose = new Command(async (Features) => await Navigation.PushAsync(new Features()));
            //BindingContext = this;
        }

        async private void ToNews(object sender, TappedEventArgs e)
        {
            await Navigation.PushAsync(new Site());
        }

        async private void ToFeatures(object sender, TappedEventArgs e)
        {
            await Navigation.PushAsync(new Features());
        }

       

         async private void SelectGroup(object sender, TappedEventArgs e)
        {
            await Navigation.PushAsync(new GroupSelect());
        }

        private void updateRasp(object sender, EventArgs e)
        {

        }

        //private void HAJKATuE(object sender, EventArgs e)//
        //{

        //    //string path = @"C:\VpmtTracker\temp\";
        //    string path = tempPath; //темп 1
        //    string path1 = "";

        //    Microsoft.Office.Interop.Word.Application word = new Microsoft.Office.Interop.Word.Application();
        //    string a = "fdfd";
        //    string b = a;
        //    DownloadWord();
        //    Microsoft.Office.Interop.Word.Document file = word.Documents.Open(path + @"rasp.doc");

        //    List<string> list = new List<string> { "1 СП", "2 СП", "3 СП", "4 СП", "1 ТО", "2 ТО", "3 ТО", "4 АТ", "1 ТМ", "2 ТМ", "3 ТМ", "4 ТМ", "1 ИС", "2 ИС", "3 ИС", "4 ИС", "1 ЭЛ", "2 ЭЛ", "3 ЭЛ", "4 ЭЛ", "1 МО", "2 ОС", "3 ТП", "1 СПП", "1 ЭМ", "2 СВ", "3 МО" };
        //    char[] ra = new char[] { '\r', '\a' };

        //    for (int numtable = 1; numtable <= 2; numtable++)
        //    {
        //        Table table = file.Tables[numtable];//выбор таблицы 

        //        for (int row = 1; row < 14; row += 2)
        //        {
        //            for (int column = 1; column < table.Rows.Count; column++)
        //            {
        //                try
        //                {
        //                    if (list.Contains(table.Cell(column, row / 2 + row % 2).Range.Text.Trim(ra)))//проверка названия подгруппы
        //                    {
        //                        path1 = path + numtable + "_" + table.Cell(column, row / 2 + row % 2).Range.Text.Trim(ra) + ".txt";
        //                        using (StreamWriter writer = new StreamWriter(path1, false))
        //                        {
        //                            writer.WriteLine(table.Cell(column, row / 2 + row % 2).Range.Text.Trim(ra));//название подгруппы
        //                            column++;
        //                            if (table.Cell(column, row + 1).Range.Text.Trim(ra) == "")
        //                                column += 2;
        //                        }
        //                    }
        //                    using (StreamWriter writer1 = new StreamWriter(path1, true))
        //                    {
        //                        writer1.WriteLine(table.Cell(column, row).Range.Text.Trim(ra));//время
        //                        writer1.Write(table.Cell(column, row + 1).Range.Text.Trim(ra) + " ");//наименование пары
        //                        column++;
        //                        writer1.WriteLine(table.Cell(column, row + 1).Range.Text.Trim(ra));//преподаватель пары
        //                    }
        //                }
        //                catch { column++; }
        //            }
        //        }
        //    }
        //    word.Quit();
        //    //Console.ReadLine();

        //    string dirtemp = tempPath; //

        //    Directory.Delete(dirtemp, true); //true - если директория не пуста (удалит и файлы и папки)
        //    Directory.CreateDirectory(dirtemp);
        //}
        //public static void DownloadWord()//
        //{
        //    WebClient webClient = new WebClient();
        //    webClient.DownloadFile("http://www.vpmt.ru/docs/rasp.doc", tempPath); //скачать
        //}

    }
}