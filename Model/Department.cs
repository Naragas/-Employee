using System;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel;
using System.Text;
using System.Threading.Tasks;

namespace Employee
{
    class Department : INotifyPropertyChanged
    {
        private string title;
        private ushort numberOfEmployees;

        public ushort NumberOfEmployees { get => numberOfEmployees; set => numberOfEmployees = value; }
        public string Title
        {
            get => title;
            set
            {
                title = value;
                OnPropertyChanged("Title");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public Department(string Title)
        {

        }
    }
}
