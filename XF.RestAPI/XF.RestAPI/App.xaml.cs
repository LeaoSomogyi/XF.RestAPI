using System;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using XF.RestAPI.ViewModel;

namespace XF.RestAPI
{
    public partial class App : Application
    {
        public static ProfessorVM ProfessorVM;

        public App()
        {
            InitializeComponent();

            Task task = Task.Run(() => InitializeViewModels());

            task.Wait();

            MainPage = new NavigationPage(new View.MainPage() { BindingContext = ProfessorVM });
        }

        private async Task InitializeViewModels()
        {
            if (ProfessorVM == null)
            {
                ProfessorVM = new ProfessorVM();

                await ProfessorVM.LoadProfessors();
            }
        }

        protected override void OnStart()
        {
            // Handle when your app starts
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }
    }
}
