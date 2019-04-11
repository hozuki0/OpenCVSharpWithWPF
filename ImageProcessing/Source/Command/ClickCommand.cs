using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Reactive;
using System.Reactive.Subjects;

namespace ImageProcessing.Command
{
    public class ClickCommand : ICommand
    {
        public event EventHandler CanExecuteChanged;

        public IObservable<Unit> OnClick => onClick; 
        private Subject<Unit> onClick = new Subject<Unit>();

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            onClick.OnNext(Unit.Default);
        }
    }
}
