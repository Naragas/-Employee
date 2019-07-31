using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Employee.ViewModel
{
    
    public class ViewModel : INotifyPropertyChanged
    {
        
        private ObservableCollection<Department> departments;
        Department selectedDepartment;
        internal ObservableCollection<Department> Departments { get => departments; private set => departments = value; }


        public ViewModel()
        {
            
           
        }

        

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
