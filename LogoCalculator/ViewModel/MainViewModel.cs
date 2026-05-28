using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using LogoCalculator.Commands;
using LogoCalculator.Core.Ldpa;

namespace LogoCalculator.ViewModel
{
    public class MainViewModel : INotifyPropertyChanged
    {
        private readonly LdpaCalculator _calculator = new();

        private double _minFrequencyMHz = 1080;
        private double _maxFrequencyMHz = 6000;
        private int _elementCount = 16;
        private double _tau = 0.90;
        private double _sigma = 0.07;
        private double _velocityFactor = 0.92;
        private double _totalLengthMm;
        private double _totalWidthMm;
        private string _errorMessage = string.Empty;


        private void OnPropertyChanged(
            [CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(
                this,
                new PropertyChangedEventArgs(propertyName));
        }

        private void Calculate()
        {
            try
            {
                ErrorMessage = string.Empty;
                Elements.Clear();
                Warnings.Clear();

                var input = new LdpaInputParameters
                {
                    MinFrequencyMHz = this.MinFrequencyMHz,
                    MaxFrequencyMHz = this.MaxFrequencyMHz,
                    ElementCount = this.ElementCount,
                    Tau = this.Tau,
                    Sigma = this.Sigma,
                    VelocityFactor = this.VelocityFactor
                };
                LdpaResult results = _calculator.Calculate(input);
                foreach (LdpaElement element in results.Elements)
                {
                    Elements.Add(element);
                }
                foreach (string warning in results.Warnings)
                {
                    Warnings.Add(warning);
                }

                TotalLengthMm = results.TotalLengthMm;
                TotalWidthMm = results.TotalWidthMm;
            }
            catch(ArgumentException ex) 
            { 
                ErrorMessage = ex.Message;
            }
        }
        public ObservableCollection<string> Warnings { get; } = new();
        public ObservableCollection<LdpaElement> Elements { get; } = new();
        public ICommand CalculateCommand { get; }

        public event PropertyChangedEventHandler? PropertyChanged;
        public MainViewModel()
        {
            CalculateCommand = new RelayCommand(Calculate);
        }
        public string ErrorMessage
        { get => _errorMessage;
            set
            {
                _errorMessage = value;
                OnPropertyChanged();
            }

        }
        public double MinFrequencyMHz
        {
            get => _minFrequencyMHz;
            set
            {
                _minFrequencyMHz = value;
                OnPropertyChanged();
            }
        }
        public double MaxFrequencyMHz
        {
            get => _maxFrequencyMHz;
            set
            {
                _maxFrequencyMHz = value;
                OnPropertyChanged();
            }
        }
        public double ElementCount
        {
            get => _elementCount;
            set
            {
                _elementCount = (int)value;
                OnPropertyChanged();
            }
        }
        public double Tau
        {
            get => _tau;
            set
            {
                _tau = value;
                OnPropertyChanged();
            }
        }
        public double Sigma
        {
            get => _sigma;
            set
            {
                _sigma = value;
                OnPropertyChanged();
            }
        }
        public double VelocityFactor
        {
            get => _velocityFactor;
            set
            {
                _velocityFactor = value;
                OnPropertyChanged();
            }
        }
        public double TotalLengthMm
        {
            get => _totalLengthMm;
            set
            {
                _totalLengthMm = value;
                OnPropertyChanged();
            }
        }
        public double TotalWidthMm
        {
            get => _totalWidthMm;
            set
            {
                _totalWidthMm = value;
                OnPropertyChanged();
            }

        }
    }
}
