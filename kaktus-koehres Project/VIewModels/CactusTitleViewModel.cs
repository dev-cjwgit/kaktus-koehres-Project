using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace kaktus_koehres_Project.VIewModels
{
    public class CactusTitleViewModel : NotifyPropertyChanged
    {
        private string _title = "";

        public string Title
        {
            get
            {
                return _title;
            }
            set
            {
                _title = value;
                OnPropertyChanged("Title");
            }
        }

        private static CactusTitleViewModel instance;
        public static CactusTitleViewModel GetInstance()
        {
            if (instance == null)
                instance = new CactusTitleViewModel();
            return instance;
        }
    }
}
