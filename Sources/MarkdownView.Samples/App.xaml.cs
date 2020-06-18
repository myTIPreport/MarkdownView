using Xamarin.Forms;

namespace MarkdownView.Samples
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            MainPage = new MasterDetailPage()
            {
                Master = new MenuPage() { Title = "Menu" },
                Detail = new MarkdownView_SamplesPage()
            };
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}