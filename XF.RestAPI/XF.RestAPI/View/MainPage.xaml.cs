using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace XF.RestAPI.View
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
        }

        private async void lstProfessors_Refreshing(object sender, EventArgs e)
        {
            lstProfessors.IsRefreshing = true;
            await App.ProfessorVM.LoadProfessors();
            lstProfessors.IsRefreshing = false;
        }
    }
}