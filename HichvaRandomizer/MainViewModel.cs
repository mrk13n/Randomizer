using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows;

namespace HichvaRandomizer
{
    internal class MainViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private int min;
        private int max;
        private RelayCommand<object> generateCommand;
        private ObservableCollection<int> resultList;

        public RelayCommand<object> GenerateCommand =>
            generateCommand ?? (generateCommand = new RelayCommand<object>(Generate, o => CanExecuteCommand()));

        public bool CanExecuteCommand() => max >= min;

        public ObservableCollection<int> ResultList
        {
            get => resultList;
            set
            {
                resultList = value;
                OnPropertyChanged();
            }
        }

        private async void Generate(object o)
        {
            string str = (string)o;

            await Task.Run(() =>
            {
                try
                {
                    ResultList = new ObservableCollection<int>();
                    int size = max - min + 1;
                    int[] array = new int[size];
                    for (int i = 0; i < array.Length; i++)
                    {
                        array[i] = i + min;
                    }

                    var rng = new Random();

                    int n = array.Length;
                    while (n > 1)
                    {
                        int k = rng.Next(n--);
                        int temp = array[n];
                        array[n] = array[k];
                        array[k] = temp;
                    }

                    foreach (var item in array)
                    {
                        ResultList.Add(item);
                    }
                }
                catch (Exception e)
                {
                    MessageBox.Show("У мене не вийшло опрацювати такий об'єм чисел :( \n Спробуй інший діапазон");
                }
            });

        }

        public int Minimum
        {
            get => min;
            set
            {
                min = value;
                OnPropertyChanged();
            }
        }

        public int Maximum
        {
            get => max;
            set
            {
                max = value;
                OnPropertyChanged();
            }
        }

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
