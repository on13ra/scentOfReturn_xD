
using scentOfReturn_xD.Pages;
using System.Dynamic;

namespace scentOfReturn_xD
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();


            if (Settings.Settings.FirstRun)
            {

                MainPage = new NavigationPage(new GroupSelect());
                Settings.Settings.FirstRun = false;
            }
            else { MainPage = new NavigationPage(new MainPage()); }
            //MainPage = new NavigationPage(new GroupSelect());
        }
      
    }
}