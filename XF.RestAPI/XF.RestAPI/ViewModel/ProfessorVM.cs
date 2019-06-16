using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using XF.RestAPI.Commands;
using XF.RestAPI.Model;

namespace XF.RestAPI.ViewModel
{
    public class ProfessorVM : INotifyPropertyChanged
    {
        #region "  Properties  "

        public Professor Professor { get; set; }

        public List<Professor> ProfessorsLocal { get; set; } = new List<Professor>();

        public ObservableCollection<Professor> Professors { get; set; } = new ObservableCollection<Professor>();

        private string searchName;

        public string SearchName
        {
            get { return searchName; }
            set
            {
                if (value == searchName) return;

                searchName = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(SearchName)));
                Filter();
            }
        }

        #endregion

        #region "  Constructors  "

        public ProfessorVM()
        {
            RedirectNew = new Command(SendToAdd);
            RedirectCancel = new Command(Cancel);
            OnSave = new OnSaveCommand(this);
            OnRemove = new OnRemoveCommand(this);
            OnEdit = new OnEditCommand(this);            
        }

        #endregion

        #region "  Commands  "

        public ICommand RedirectNew { get; set; }

        public ICommand RedirectCancel { get; set; }

        public OnSaveCommand OnSave { get; set; }

        public OnRemoveCommand OnRemove { get; set; }

        public OnEditCommand OnEdit { get; set; }

        #endregion

        #region "  Operations  "

        public event PropertyChangedEventHandler PropertyChanged;       

        public void Filter()
        {
            if (searchName == null)
                searchName = string.Empty;

            var result = ProfessorsLocal.Where(p => p.Name.ToLowerInvariant().Contains(SearchName.ToLowerInvariant()))
                .ToList();

            if (result != null)
            {
                var remove = Professors.Except(result).ToList();

                remove.ForEach(r =>
                {
                    Professors.Remove(r);
                });
            }

            for (int index = 0; index < result.Count; index++)
            {
                var item = result[index];
                if (index + 1 > Professors.Count || !Professors[index].Equals(item))
                    Professors.Insert(index, item);
            }
        }

        public void SendToAdd()
        {
            App.ProfessorVM.Professor = new Professor();

            App.Current.MainPage.Navigation.PushAsync(new View.Professor() { BindingContext = App.ProfessorVM });
        }

        public void Cancel()
        {
            App.Current.MainPage.Navigation.PopAsync();
        }

        public async void Save(Professor professor)
        {
            try
            {
                if (professor.Id == 0)
                {
                    await HandleSave(professor);
                }
                else
                {
                    await HandleEdit(professor);
                }
            }
            catch (Exception ex)
            {
                await App.Current.MainPage.DisplayAlert("Atenção", $"Ocorreu um erro inesperado. {ex.Message}", "Ok");
            }
        }

        public async void Remove(Professor professor)
        {
            bool action = await App.Current.MainPage.DisplayAlert("Atenção", $"Deseja remover o {professor.Name}?", "Sim", "Não");

            if (action)
            {
                await ProfessorRepository.DeleteProfessorAsync(professor.Id);
                await App.Current.MainPage.DisplayAlert("Info", $"{professor.Name} removido com sucesso!", "Ok");
                await LoadProfessors();
            }
        }

        public async Task LoadProfessors()
        {
            ProfessorsLocal = await ProfessorRepository.GetTeachersAsync();

            Filter();
        }

        public async void Edit(Professor professor)
        {
            App.ProfessorVM.Professor = professor;

            await App.Current.MainPage.Navigation.PushAsync(new View.Professor() { BindingContext = App.ProfessorVM });
        }

        #endregion

        #region "  Private Methods  "

        private async Task HandleSave(Professor professor)
        {
            bool result = await ProfessorRepository.PostProfessorAsync(professor);

            if (result)
            {
                await App.Current.MainPage.Navigation.PopAsync();

                await App.Current.MainPage.DisplayAlert("Info", $"{professor.Name} salvo com sucesso!", "Ok");

                await LoadProfessors();
            }
            else
            {
                await App.Current.MainPage.DisplayAlert("Atenção", $"Ocorreu um problema ao salvar o {professor.Name}, " +
                    $"tente novamente mais tarde.", "Ok");
            }
        }

        private async Task HandleEdit(Professor professor)
        {
            bool result = await ProfessorRepository.PutProfessorAsync(professor);

            if (result)
            {
                await App.Current.MainPage.Navigation.PopAsync();

                await App.Current.MainPage.DisplayAlert("Info", $"{professor.Name} editado com sucesso!", "Ok");

                await LoadProfessors();
            }
            else
            {
                await App.Current.MainPage.DisplayAlert("Atenção", $"Ocorreu um problema ao editar o {professor.Name}, " +
                    $"tente novamente mais tarde.", "Ok");
            }
        }

        private void EventPropertyChanged([CallerMemberName] string propertyName = null)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion
    }
}
