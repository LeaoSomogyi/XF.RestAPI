using System;
using System.Windows.Input;
using XF.RestAPI.Model;
using XF.RestAPI.ViewModel;

namespace XF.RestAPI.Commands
{
    public class OnRemoveCommand : ICommand
    {
        private readonly ProfessorVM professor;

        public OnRemoveCommand(ProfessorVM professor)
        {
            this.professor = professor;
        }

        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter) => parameter != null;

        public void Execute(object parameter)
        {
            professor.Remove(parameter as Professor);
        }
    }
}
