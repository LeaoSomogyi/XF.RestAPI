using System;
using System.Windows.Input;
using XF.RestAPI.Model;
using XF.RestAPI.ViewModel;

namespace XF.RestAPI.Commands
{
    public class OnSaveCommand : ICommand
    {
        private readonly ProfessorVM professor;

        public OnSaveCommand(ProfessorVM professor)
        {
            this.professor = professor;
        }

        public event EventHandler CanExecuteChanged;

        public void IsModelValid() => CanExecuteChanged?.Invoke(this, EventArgs.Empty);

        public bool CanExecute(object parameter) => parameter != null && 
            (!string.IsNullOrEmpty(((Professor)parameter).Name)) &&
            (!string.IsNullOrEmpty(((Professor)parameter).Title));

        public void Execute(object parameter)
        {
            professor.Save(parameter as Professor);
        }
    }
}
