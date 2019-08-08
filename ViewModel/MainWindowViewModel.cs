using System;

using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Globalization;
using System.Windows.Input;
using System.Windows;
using Employee.Model;
using System.Runtime.Remoting.Contexts;
using System.Data.Entity;

namespace Employee.ViewModel
{

    public class MainWindowViewModel : INotifyPropertyChanged
    {
        Model.EmployeeEntities emDB = new Model.EmployeeEntities();
        //Model.ModelData dt = new Model.ModelData();
        private Department _chosenDepartment;
        private Department departmentToChange;
        private Model.Employee chosenEmployee;
        private ObservableCollection<Department> departments;
        private ObservableCollection<Model.Employee> selectedEmployees = new ObservableCollection<Model.Employee>();
        

        //Список команд.
        public ICommand DepAddCommand { get; private set; }
        public ICommand DepDeleteCommand { get; private set; }
        public ICommand DepChangeCommand { get; private set; }
        public ICommand EmpDeleteCommand { get; private set; }
        public ICommand EmpAddCommand { get; private set; }
        public ICommand EmpChangeCommand { get; private set; }


        public Department ChosenDepartment
        {
            get { return _chosenDepartment; }
            set
            {
                _chosenDepartment = value;
                GetSelectedEmployees();
                OnPropertyChanged("SelectedDepartment");
            }
        }
        public Department DepartmentToChange
        {
            get { return departmentToChange; }
            set
            {
                departmentToChange = value;
                OnPropertyChanged("DepartmentToChange");
            }
        }

        public Model.Employee ChosenEmployee
        {
            get { return chosenEmployee; }
            set
            {
                chosenEmployee = value;
                OnPropertyChanged("ChosenEmployee");
            }
        }

        public MainWindowViewModel()
        {
            DepAddCommand = new DelegateCommand(AddDepartment);
            EmpAddCommand = new DelegateCommand(AddEmployee);
            DepDeleteCommand = new DelegateCommand(DeleteDepartment);
            EmpDeleteCommand = new DelegateCommand(DeleteEmployee);
            DepChangeCommand = new DelegateCommand(ChangeDepartment);
            EmpChangeCommand = new DelegateCommand(ChangeEmployee);

            LoadData();
        }
        /// <summary>
        /// Метод изменения департамена, выбранного в ComboBox
        /// </summary>
        /// <param name="obj">Название нового департамента</param>
        private void ChangeDepartment(object obj)
        {
            DepartmentToChange.Title = (string)obj;
            EmDB.Entry(DepartmentToChange).State = EntityState.Modified;
            emDB.SaveChanges();
            LoadData();
            GetSelectedEmployees();
        }
        /// <summary>
        /// Метод добавления департамента.
        /// </summary>
        /// <param name="obj">Название нового департамента</param>
        private void AddDepartment(object obj)
        {
            string s = (string)obj;
            bool isContain = false;           

            //проверка на наличие департамента с таким же названием
            foreach (Department d in emDB.Departments)
            {
                if (d.Title.Equals(s))
                {
                    isContain = true;
                }
            }

            if (!isContain)
            {
                emDB.Departments.Add(new Department() { Title = s.Trim() });
                emDB.SaveChanges();
            }
            LoadData();

        }
        /// <summary>
        /// Метод удаления выбранного в ListBox департамента
        /// </summary>
        /// <param name="obj"></param>
        private void DeleteDepartment(object obj)
        {
            //Удаление всех работников выбранного департамента.
            for (int i = SelectedEmployees.Count - 1; i >= 0; i--)
            {
                emDB.Employees.Remove(SelectedEmployees[i]);
            }
            EmDB.Departments.Remove(_chosenDepartment);
            emDB.SaveChanges();
            LoadData();


        }
        /// <summary>
        /// Метод удаления работника
        /// </summary>
        /// <param name="obj"></param>
        private void DeleteEmployee(object obj)
        {
            emDB.Employees.Remove(ChosenEmployee);
            emDB.SaveChanges();
            GetSelectedEmployees();
        }

        /// <summary>
        /// Метод добавления нового сотрудника
        /// </summary>
        /// <param name="obj">Массив объектов полученного из MultiBinder</param>
        private void AddEmployee(object obj)
        {
            object[] data = (object[])obj;
            //Создание кортежа после проверки всех данных на корректность
            var (id,name, middleName, lastName, age, sex, department, isValid, message) = ValidateData(data);

            if (!isValid)
            {
                MessageBox.Show(message, "Data error", MessageBoxButton.OK);

                return;
            }

            emDB.Employees.Add(new Model.Employee() { Name = name.Trim(), MiddleName = middleName.Trim(), LastName = lastName.Trim(), Sex = sex.Trim(), Age = age, Department = department.Dep_Id });
            emDB.SaveChanges();
            GetSelectedEmployees();
            



        }
        /// <summary>
        /// Метод получения списка сотрудников выбранного департамента
        /// </summary>
        private void GetSelectedEmployees()
        {
            if (ChosenDepartment != null)
            {
                var q = emDB.Employees.Where(x => x.Department == ChosenDepartment.Dep_Id);

                SelectedEmployees?.Clear();
                foreach (var item in q)
                {
                    SelectedEmployees.Add(item);
                }
            }
            else
            {
                SelectedEmployees.Clear();
            }
        }
        /// <summary>
        /// Метод изменения данных сотрудника
        /// </summary>
        /// <param name="obj">Массив объектов полученного из MultiBinder</param>
        private void ChangeEmployee(object obj)
        {
            object[] data = (object[])obj;
            var (id,name, middleName, lastName, age, sex, department, isValid, message) = ValidateData(data);

            if (!isValid)
            {
                MessageBox.Show(message, "Data error", MessageBoxButton.OK);

                return;
            }
            if (ChosenEmployee != null)
            {
                ChosenEmployee.Name = name.Trim();
                ChosenEmployee.MiddleName = middleName.Trim();
                ChosenEmployee.LastName = lastName.Trim();
                ChosenEmployee.Age = age;
                ChosenEmployee.Sex = sex.Trim();
                ChosenEmployee.Department1 = department;

                emDB.Entry(ChosenEmployee).State = EntityState.Modified;
                emDB.SaveChanges();
                
            }
            
            GetSelectedEmployees();
        }

        /// <summary>
        /// Метод проверки введенных данных на корректность
        /// </summary>
        /// <param name="values">Массив Объектов</param>
        /// <returns></returns>
        private (int id, string name, string middleName, string lastName, byte age, string sex, Department department, bool isValid, string message) ValidateData(object[] values)
        {
            var message = new StringBuilder();
            var isValid = true;

            int.TryParse(values[0].ToString(), out int id);

            var name = values[1].ToString();
            if (name.Length == 0)
            {
                isValid = false;
                message.AppendLine("Invalid name");
            }
            var middleName = values[2].ToString();
            if (middleName.Length == 0)
            {
                isValid = false;
                message.AppendLine("Invalid middleName");
            }
            var lastName = values[3].ToString();
            if (lastName.Length == 0)
            {
                isValid = false;
                message.AppendLine("Invalid lastName");
            }

            if (!byte.TryParse(values[4].ToString(), out byte age) || age < 18)
            {
                isValid = false;
                message.AppendLine("Age cant be lower 18");
            }
            var sex = values[5].ToString();
            if (!sex.Equals("Мужской") && !sex.Equals("Женский"))
            {
                isValid = false;
                message.AppendLine("Invalid sex");
            }
            var department = values[6] as Department;
            if (department == null)
            {
                isValid = false;
                message.AppendLine("Invalid department");
            }

            return (id, name, middleName, lastName, age, sex, department, isValid, message.ToString());
        }
        /// <summary>
        /// Метод запроса данных из Модели
        /// </summary>
        private void LoadData()
        {

            ObservableCollection<Department> temp = new ObservableCollection<Department>();

            foreach (Department item in emDB.Departments)
            {               
                temp.Add(item);
            }
            Departments = temp;
            ObservableCollection<Model.Employee> temp1 = new ObservableCollection<Model.Employee>();
            foreach (var item in emDB.Employees)
            {
                temp1.Add(item);
            }
            Employees = temp1;
        }




        public ObservableCollection<Department> Departments
        {
            get { return departments; }
            private set
            {
                departments = value;
                OnPropertyChanged("Departments");
            }
        } 

        public ObservableCollection<Model.Employee> Employees { get; private set; } = new ObservableCollection<Model.Employee>() { };

        public ObservableCollection<Model.Employee> SelectedEmployees
        {
            get { return selectedEmployees; }
            set
            {
                selectedEmployees = value;
                OnPropertyChanged("SelectedEmployees");
            }
        }

        public EmployeeEntities EmDB { get => emDB; set => emDB = value; }

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

    }
}
