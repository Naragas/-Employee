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
    public class ModelData : INotifyPropertyChanged
    {
        private const int EmployeeListKeyName = 0;
        private const int EmployeeListKeyMIddleName = 1;
        private const int EmployeeListKeyLastName = 2;
        private const int EmployeeListKeySex = 3;
        private const int EmployeeListKeyAge = 4;
        private const int EmployeeListKeyDepartmentName = 5;

        public event PropertyChangedEventHandler PropertyChanged;

        readonly string departmentsPath = @"D:\GitHub\Employee\Properties\Departments.txt";
        //readonly string writePath = @"D:\GitHub\Employee\Properties\Departments_test.txt";
        readonly string employeesPath = @"D:\GitHub\Employee\Properties\Employees.txt";

        public List<BaseEmployee> Employees { get; private set; } = new List<BaseEmployee>() { };
        public List<Department> Departments { get; private set; } = new List<Department>() { };

        public ModelData()
        {
            LoadData();
        }
        public void LoadData()
        {
            GetDepartments(departmentsPath);
            GetEmployee(employeesPath);

        }
        private List<string> GetDataFormFile(string path)
        {
            List<String> tempString = new List<string>();
            using (StreamReader fs = new StreamReader(path, System.Text.Encoding.Default))
            {
                while (true)
                {                    
                    string temp = fs.ReadLine();
                    if (temp == null) break;
                    tempString.Add(temp);                    
                }
            }
            return tempString;
        }
        public void SaveDepartments(ObservableCollection<Department> List)
        {            
            using (StreamWriter sw = new StreamWriter(departmentsPath, false, System.Text.Encoding.Default))
            {
                for (int i = 0; i < List.Count; i++)
                {
                    sw.WriteLine(List[i].Title);
                }
            }
            LoadData();
        }
        public void SaveEmployees(ObservableCollection<BaseEmployee> List)
        {

            using (StreamWriter sw = new StreamWriter(employeesPath, false, System.Text.Encoding.Default))
            {
                for (int i = 0; i < List.Count; i++)
                {
                    sw.WriteLine(List[i].SaveDataToBase());
                }
            }
            LoadData();
        }
        public void SaveEmployees(List<BaseEmployee> List)
        {

            using (StreamWriter sw = new StreamWriter(employeesPath, false, System.Text.Encoding.Default))
            {
                for (int i = 0; i < List.Count; i++)
                {
                    sw.WriteLine(List[i].SaveDataToBase());
                }
            }
            LoadData();
        }

        public ObservableCollection<BaseEmployee> ChoseEmployeeByDepartment(Department sdep)
        {
            ObservableCollection<BaseEmployee> s = new ObservableCollection<BaseEmployee>();
            if (sdep != null)
            {
                var selectedEmployees = from user in Employees where user.Department.Title.Equals(sdep.Title) select user;
                foreach (var item in selectedEmployees)
                {
                    s.Add(item);
                }
            }
            return s;
        }

        internal void ChangeEmployee((string name, string middleName, string lastName, byte age, string sex, Department department) tuple, BaseEmployee chosenEmployee)
        {
            Employees[Employees.IndexOf(chosenEmployee)] = new BaseEmployee(tuple.name, tuple.middleName, tuple.lastName, tuple.age, tuple.sex, tuple.department);
            SaveEmployees(Employees);
        }

        public void AddEmployee((string name, string middleName, string lastName, byte age, string sex, Department department) tuple)
        {
            Employees.Add( new BaseEmployee(tuple.name,tuple.middleName,tuple.lastName,tuple.age,tuple.sex,tuple.department));
            SaveEmployees(Employees);

        }

        private void GetDepartments(string path)
        {
            Departments.Clear();
            foreach (string s in GetDataFormFile(path))
            {                
                Departments.Add(new Department(s));
            }
        }
        

        private void GetEmployee(string path)
        {
            Employees.Clear();
            foreach (string s in GetDataFormFile(path))
            {
                BaseEmployee temp = new BaseEmployee();
                String[] tempStrings = s.Split(',');
                temp.Name = tempStrings[EmployeeListKeyName];
                temp.MiddleName = tempStrings[EmployeeListKeyMIddleName];
                temp.LastName = tempStrings[EmployeeListKeyLastName];
                temp.Sex = tempStrings[EmployeeListKeySex];
                temp.Age = Convert.ToByte(tempStrings[EmployeeListKeyAge]);
                temp.Department = Departments.FirstOrDefault(e => e.Title == tempStrings[EmployeeListKeyDepartmentName]);
                Employees.Add(temp);
            }
        }
    }



}
