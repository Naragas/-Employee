using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Employee.Model
{
    public class Data : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        internal ObservableCollection<Department> Departments { get; set; }
        internal ObservableCollection<BaseEmployee> Employees { get; set; }

        readonly string departmentsPath = "Departments.txt";
        readonly string employeesPath = "Employees.txt";



        private List<string> GetDataFormFile(string path)
        {
            List<String> tempString = new List<string>();
            using (StreamReader fs = new StreamReader(path))
            {
                while (true)
                {
                    // Читаем строку из файла во временную переменную.
                    string temp = fs.ReadLine();
                    tempString.Add(temp);
                    // Если достигнут конец файла, прерываем считывание.
                    if (temp == null) break;
                }
            }

            return tempString;
        }

        private void GetDepartments(string path)
        {
            foreach (string s  in GetDataFormFile(path))
            {
                Departments.Add(new Department(s));
            }
        }

        private void GetEmployee(string path)
        {
              
            foreach(string s in GetDataFormFile(path))
            {
                BaseEmployee temp = new BaseEmployee();
                String[] tempStrings = s.Split(',');
                for (int i = 0; i < tempStrings.Length; i++)
                {
                    switch (i)
                    {
                        case 0:
                            temp.Name = tempStrings[i];
                            break;
                        case 1:
                            temp.MiddleName = tempStrings[i];
                            break;
                        case 2:
                            temp.Lastname = tempStrings[i];
                            break;
                        case 3:
                            temp.Sex = tempStrings[i];
                            break;
                        case 4:
                            temp.Age = Convert.ToByte(tempStrings[i]);
                            break;
                        case 5:
                            foreach (var item in Departments)
                            {
                                if (item.Title.Equals(tempStrings[i]))
                                {
                                    temp.Department = item;
                                }
                            }                            
                            break;
                        default:
                            break;
                    }
                }
                Employees.Add(temp);
            }
        }
    }


    
}
