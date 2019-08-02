using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Employee
{
    public class BaseEmployee : INotifyPropertyChanged
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
        public string LastName
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
            this.Name = Name;
            this.MiddleName = MiddleName;
            this.LastName = LastName;
            this.Age =Age;
            this.Sex = Sex;
        }
        public BaseEmployee(string Name, string MiddleName, string LastName, byte Age, string Sex, Department departmen)
        {
            this.Name = Name;
            this.MiddleName = MiddleName;
            this.LastName = LastName;
            this.Age = Age;
            this.Sex = Sex;
            this.Department = departmen;
        }

        public void UpdateEmployee(string name, string middleName, string lastName, byte age, string sex, Department department)
        {
            Name = name;
            MiddleName = middleName;
            LastName = lastName;
            Age = age;
            Sex = sex;
            Department = department;
        }
        public string SaveDataToBase()
        {
            string s;
            s = $"{Name},{MiddleName},{LastName},{Sex},{Age},{Department.Title}";
            return s;
        }
    }
}
