using System;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel;
using System.Text;
using System.Threading.Tasks;

namespace Employee
{
    public class BaseDepartment : INotifyPropertyChanged
    {
        private string _title;
        

        public string Title
        {
            get => _title;
            set
            {
                _title = value;
                OnPropertyChanged("Title");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public BaseDepartment()
        {
         
        }
        public BaseDepartment(string title)
        {
            Title = title;
        }
        public override string ToString()
        {
            return Title;

        }

        public void UpdateDepartment(string title)
        {
            Title = title;
        }
    }
}
