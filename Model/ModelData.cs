using System;
using System.Reflection;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Employee.Model
{
    public class ModelData
    {
        //Константы для сплита строки с данными о сотрудниках.
        private const int EmployeeListKeyName = 0;
        private const int EmployeeListKeyMIddleName = 1;
        private const int EmployeeListKeyLastName = 2;
        private const int EmployeeListKeySex = 3;
        private const int EmployeeListKeyAge = 4;
        private const int EmployeeListKeyDepartmentName = 5;
                
        //путь к файлу данными о департаментах
        readonly string departmentsPath = "Departments.txt";
        //Путь к файлу с данными о сотрудниках
        readonly string employeesPath = "Employees.txt";
        

        public List<BaseEmployee> Employees { get; private set; } = new List<BaseEmployee>() { };
        public List<BaseDepartment> Departments { get; private set; } = new List<BaseDepartment>() { };

        public ModelData()
        {
            LoadData();
        }

        /// <summary>
        /// Загрузка данных
        /// </summary>
        public void LoadData()
        {
            GetDepartments(departmentsPath);
            GetEmployee(employeesPath);

        }
        /// <summary>
        /// Метод чтения данных из файла, получаем список со строками.
        /// </summary>
        /// <param name="path">Путь</param>
        /// <returns></returns>
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
        /// <summary>
        /// Метод сохранения списка департаментов
        /// </summary>
        /// <param name="List">ObservableCollection департаментов</param>
        public void SaveDepartments(ObservableCollection<BaseDepartment> List)
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
        /// <summary>
        /// Метод сохранения списка департаментов
        /// </summary>
        /// <param name="List">List департаментов</param>
        public void SaveDepartments(List<BaseDepartment> List)
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
        /// <summary>
        /// Метод изменения департамента
        /// </summary>
        /// <param name="departmentToChange">Департамент выбранный для изменения</param>
        /// <param name="obj">Парамент нового названия департамента</param>
        internal void ChangeDepartment(BaseDepartment departmentToChange, string obj)
        {
            if (Departments.Contains(departmentToChange))
            {
                Departments[Departments.IndexOf(departmentToChange)].UpdateDepartment(obj);
            }
            SaveDepartments(Departments);
        }
        /// <summary>
        /// Метод записи списка сотрудников в файл
        /// </summary>
        /// <param name="List">ObservableCollection Сотрудников</param>
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
        /// <summary>
        /// Метод записи списка сотрудников в файл
        /// </summary>
        /// <param name="List">List сотрудников</param>
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
        /// <summary>
        /// Получение списка сотрудников работающих в выбранном департаменте
        /// </summary>
        /// <param name="sdep">Выбранный департамент</param>
        /// <returns></returns>
        public ObservableCollection<BaseEmployee> ChoseEmployeeByDepartment(BaseDepartment sdep)
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
        /// <summary>
        /// Метод изменения данных работника
        /// </summary>
        /// <param name="tuple">Кортеж с параметрами</param>
        /// <param name="chosenEmployee">Работник данные которого должны измениться</param>
        internal void ChangeEmployee((string name, string middleName, string lastName, byte age, string sex, BaseDepartment department) tuple, BaseEmployee chosenEmployee)
        {
            Employees[Employees.IndexOf(chosenEmployee)] = new BaseEmployee(tuple.name, tuple.middleName, tuple.lastName, tuple.age, tuple.sex, tuple.department);
            SaveEmployees(Employees);
        }
        /// <summary>
        /// Метод добавления нового сотрудника
        /// </summary>
        /// <param name="tuple">Кортеж с параметрами</param>
        public void AddEmployee((string name, string middleName, string lastName, byte age, string sex, BaseDepartment department) tuple)
        {
            Employees.Add( new BaseEmployee(tuple.name,tuple.middleName,tuple.lastName,tuple.age,tuple.sex,tuple.department));
            SaveEmployees(Employees);

        }
        /// <summary>
        /// Метод добавления нового департамента
        /// </summary>
        /// <param name="obj">Название</param>
        public void AddDepartment(string obj)
        {
            Departments.Add(new BaseDepartment(obj));
            SaveDepartments(Departments);

        }
        /// <summary>
        /// Метод получения списка департаментов из файла
        /// </summary>
        /// <param name="path">Путь к файлу с данными</param>
        private void GetDepartments(string path)
        {
            Departments.Clear();
            foreach (string s in GetDataFormFile(path))
            {                
                Departments.Add(new BaseDepartment(s));
            }
        }
        
        /// <summary>
        /// Метод получения списка сотрудников из файла
        /// </summary>
        /// <param name="path">Путь к файлу с данными</param>
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
