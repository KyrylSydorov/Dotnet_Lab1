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

        private string _ageDisplay;
        private string _birthdayMessage;

        public string AgeDisplay
        {
            get => _ageDisplay;
            set 
            {
                if (_ageDisplay != value)
                {
                    _ageDisplay = value;
                    OnPropertyChanged(nameof(AgeDisplay));
                }
            }
        }

        public string BirthdayMessage
        {
            get => _birthdayMessage;
            set { _birthdayMessage = value; OnPropertyChanged(nameof(BirthdayMessage)); }
        }

        public ICommand CalculateCommand { get; }
        public MainViewModel()
        {
            CalculateCommand = new RelayCommand(Calculate);
        }

        private void Calculate(object parameter)
        {
            AgeDisplay = string.Empty;
            BirthdayMessage = string.Empty;

            if (BirthDate == null)
            {
                MessageBox.Show("Введіть правильну дату", "Помилка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            DateTime birthDate = BirthDate.Value;
            Console.WriteLine($"Birth date: {birthDate}");

            DateTime today = DateTime.Today;
            
            int age = today.Year - birthDate.Year;
            if (birthDate > today.AddYears(-age))
            {
                age--;
            }

            if (age < 0)
            {
                MessageBox.Show("Ви ще не народилися!", "Помилка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (age > 135)
            {
                MessageBox.Show("Ви вже померли!", "Помилка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            AgeDisplay = $"Ваш вік: {age} років";

            if (birthDate.Day == today.Day && birthDate.Month == today.Month)
            {
                BirthdayMessage = "З днем народження!";
            }
            else
            {
                BirthdayMessage = string.Empty;
            }
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
