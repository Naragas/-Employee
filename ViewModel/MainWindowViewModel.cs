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

namespace Employee.ViewModel
{

    public class MainWindowViewModel : INotifyPropertyChanged
    {

        Model.ModelData dt = new Model.ModelData();
        private BaseDepartment _chosenDepartment;
        private BaseDepartment departmentToChange;
        private BaseEmployee chosenEmployee;
        private ObservableCollection<BaseDepartment> departments;
        private ObservableCollection<BaseEmployee> selectedEmployees;

        //Список команд.
        public ICommand DepAddCommand { get; private set; }
        public ICommand DepDeleteCommand { get; private set; }
        public ICommand DepChangeCommand { get; private set; }
        public ICommand EmpDeleteCommand { get; private set; }
        public ICommand EmpAddCommand { get; private set; }
        public ICommand EmpChangeCommand { get; private set; }


        public BaseDepartment ChosenDepartment
        {
            get { return _chosenDepartment; }
            set
            {
                _chosenDepartment = value;
                GetSelectedEmployees();
                OnPropertyChanged("SelectedDepartment");
            }
        }
        public BaseDepartment DepartmentToChange
        {
            get { return departmentToChange; }
            set
            {
                departmentToChange = value;
                OnPropertyChanged("DepartmentToChange");
            }
        }

        public BaseEmployee ChosenEmployee
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
            dt.ChangeDepartment(DepartmentToChange, (string)obj);
            LoadData();
        }
        /// <summary>
        /// Метод добавления департамента.
        /// </summary>
        /// <param name="obj">Название нового департамента</param>
        private void AddDepartment(object obj)
        {
            string s = (string)obj;
            bool isContain = false;

            //Проверка на наличие департамента с таким же названием
            foreach (BaseDepartment d in Departments)
            {
                if (d.Title.Equals(s))
                {
                    isContain = true;
                }
            }

            if (!isContain) dt.AddDepartment(s);
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
                Employees.Remove(SelectedEmployees[i]);
            }
            Departments.Remove(_chosenDepartment);

            SaveData();
        }
        /// <summary>
        /// Метод удаления работника
        /// </summary>
        /// <param name="obj"></param>
        private void DeleteEmployee(object obj)
        {
            Employees.Remove(ChosenEmployee);
            SaveData();
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
            var (name, middleName, lastName, age, sex, department, isValid, message) = ValidateData(data);

            if (!isValid)
            {
                MessageBox.Show(message, "Data error", MessageBoxButton.OK);

                return;
            }
            dt.AddEmployee((name, middleName, lastName, age, sex, department));
            GetSelectedEmployees();

        }
        /// <summary>
        /// Метод получения списка сотрудников выбранного департамента
        /// </summary>
        private void GetSelectedEmployees()
        {
            SelectedEmployees = dt.ChoseEmployeeByDepartment(_chosenDepartment);
        }
        /// <summary>
        /// Метод изменения данных сотрудника
        /// </summary>
        /// <param name="obj">Массив объектов полученного из MultiBinder</param>
        private void ChangeEmployee(object obj)
        {
            object[] data = (object[])obj;
            var (name, middleName, lastName, age, sex, department, isValid, message) = ValidateData(data);

            if (!isValid)
            {
                MessageBox.Show(message, "Data error", MessageBoxButton.OK);

                return;
            }
            dt.ChangeEmployee((name, middleName, lastName, age, sex, department), ChosenEmployee);
            GetSelectedEmployees();
        }

        /// <summary>
        /// Метод проверки введенных данных на корректность
        /// </summary>
        /// <param name="values">Массив Объектов</param>
        /// <returns></returns>
        private (string name, string middleName, string lastName, byte age, string sex, BaseDepartment department, bool isValid, string message) ValidateData(object[] values)
        {
            var message = new StringBuilder();
            var isValid = true;

            var name = values[0].ToString();
            if (name.Length == 0)
            {
                isValid = false;
                message.AppendLine("Invalid name");
            }
            var middleName = values[1].ToString();
            if (middleName.Length == 0)
            {
                isValid = false;
                message.AppendLine("Invalid middleName");
            }
            var lastName = values[2].ToString();
            if (lastName.Length == 0)
            {
                isValid = false;
                message.AppendLine("Invalid lastName");
            }

            if (!byte.TryParse(values[3].ToString(), out byte age) || age < 18)
            {
                isValid = false;
                message.AppendLine("Age cant be lower 18");
            }
            var sex = values[4].ToString();
            if (!sex.Equals("Мужской") && !sex.Equals("Женский"))
            {
                isValid = false;
                message.AppendLine("Invalid sex");
            }
            var department = values[5] as BaseDepartment;
            if (department == null)
            {
                isValid = false;
                message.AppendLine("Invalid department");
            }

            return (name, middleName, lastName, age, sex, department, isValid, message.ToString());
        }
        /// <summary>
        /// Метод запроса данных из Модели
        /// </summary>
        private void LoadData()
        {
            Departments = new ObservableCollection<BaseDepartment>(dt.Departments);
            Employees = new ObservableCollection<BaseEmployee>(dt.Employees);
        }
        /// <summary>
        /// Метод записи данных в Модель
        /// </summary>
        private void SaveData()
        {
            dt.SaveDepartments(Departments);
            dt.SaveEmployees(Employees);
            LoadData();
        }



        public ObservableCollection<BaseDepartment> Departments
        {
            get { return departments; }
            private set
            {
                departments = value;
                OnPropertyChanged("Departments");
            }
        } 

        public ObservableCollection<BaseEmployee> Employees { get; private set; } = new ObservableCollection<BaseEmployee>() { };

        public ObservableCollection<BaseEmployee> SelectedEmployees
        {
            get { return selectedEmployees; }
            set
            {
                selectedEmployees = value;
                OnPropertyChanged("SelectedEmployees");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

    }
}
