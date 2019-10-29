using Reactive.Bindings.Extensions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Reactive.Disposables;
using System.Runtime.CompilerServices;
using System.Text;
using System.Windows;

namespace ReactivePropertyDataGridSample
{
    public class MainWindowViewModel : BindableBase
    {
        // 今回はしてないけど Window が閉じられるタイミングなどで Dispose しないと永遠に People の変更を監視し続ける
        private readonly CompositeDisposable _disposable = new CompositeDisposable();
        public ObservableCollection<People> Member { get; }

        public MainWindowViewModel()
        {
            Member = new ObservableCollection<People>(new[]
            {
                new People { Name = "田中一郎", Age = 30, Height = 170 },
                new People { Name = "佐藤次郎", Age = 35, Height = 165},
            });

            Member.ObserveElementPropertyChanged()
                .Subscribe(Changed)
                .AddTo(_disposable);
        }

        private void Changed(SenderEventArgsPair<People, PropertyChangedEventArgs> pair)
        {
            MessageBox.Show("Changed");
        }
    }

    public class BindableBase : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        protected bool SetProperty<T>(ref T field, T value, [CallerMemberName]string propertyName = null)
        {
            if (EqualityComparer<T>.Default.Equals(field, value)) { return false; }

            field = value;
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
            return true;
        }
    }

    public class People : BindableBase
    {
        private string _name;
        public string Name
        {
            get { return _name; }
            set { SetProperty(ref _name, value); }
        }

        private int _age;
        public int Age
        {
            get { return _age; }
            set { SetProperty(ref _age, value); }
        }

        private int _height;
        public int Height
        {
            get { return _height; }
            set { SetProperty(ref _height, value); }
        }
    }
}
