using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Employee
{
    class BaseEmployee : INotifyPropertyChanged
    {
        private string name;
        private string middleName;
        private string lastName;
        private byte age;
        private string sex;
        private Department department;

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        public string Sex
        {
            get => sex;
            set
            {
                sex = value;
                OnPropertyChanged("Sex");
            }
        }

        public byte Age
        {
            get => age;
            set
            {
                age = value;
                OnPropertyChanged("Sex");
            }
        }
        public string Lastname
        {
            get => lastName;
            set
            {
                lastName = value;
                OnPropertyChanged("LastName");
            }
        }
        public string MiddleName
        {
            get => middleName;
            set
            {
                middleName = value;
                OnPropertyChanged("MiddleName");
            }
        }
        public string Name
        {
            get => name;
            set
            {
                name = value;
                OnPropertyChanged("Name");
            }
        }

        public Department Department
        {
            get => department;
            set
            {
                department = value;
                OnPropertyChanged("Department");
            }
        }

        public BaseEmployee()
        {

        }
        public BaseEmployee(string Name, string MiddleName, string LastName, byte Age, string Sex)
        {

        }


    }
}
