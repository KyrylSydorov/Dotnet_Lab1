using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Input;

namespace Sydorov_Lab1.ViewModels
{
    public class MainViewModel : INotifyPropertyChanged
    {
        private DateTime? _birthDate;

        public DateTime? BirthDate
        {
            get => _birthDate;
            set
            {
                if (_birthDate != value)
                {
                    _birthDate = value;
                    OnPropertyChanged(nameof(BirthDate));
                }
            }
        }

        public ICommand CalculateCommand { get; }
        public MainViewModel()
        {
            CalculateCommand = new RelayCommand(Calculate);
        }

        private void Calculate(object parameter)
        {
            if (BirthDate == null)
            {
                MessageBox.Show("Введіть правильну дату", "Помилка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            DateTime birthDate = BirthDate.Value;
            Console.WriteLine($"Birth date: {birthDate}");
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    public class RelayCommand : ICommand
    {
        private readonly Action<object> _execute;
        private readonly Predicate<object> _canExecute;

        public RelayCommand(Action<object> execute, Predicate<object> canExecute = null)
        {
            _execute = execute ?? throw new ArgumentNullException(nameof(execute));
            _canExecute = canExecute;
        }

        public bool CanExecute(object parameter) => _canExecute == null || _canExecute(parameter);

        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        public void Execute(object parameter) => _execute(parameter);
    }
}
