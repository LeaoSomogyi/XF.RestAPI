using System;
using System.Windows.Input;
using XF.RestAPI.Model;
using XF.RestAPI.ViewModel;

namespace XF.RestAPI.Commands
{
    public class OnDetailCommand : ICommand
    {
        private readonly ProfessorVM professor;

        public OnDetailCommand(ProfessorVM professor)
        {
            this.professor = professor;
        }

        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter) => parameter != null;

        public void Execute(object parameter)
        {
            professor.SendToDetails(parameter as Professor);
        }
    }
}
