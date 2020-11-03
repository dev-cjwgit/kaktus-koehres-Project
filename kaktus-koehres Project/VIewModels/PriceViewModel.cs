using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace kaktus_koehres_Project.VIewModels
{
    public class PriceViewModel : NotifyPropertyChanged
    {
        private double[] _Price = { 0, 0, 0, 0 };


        public double[] Price
        {
            get
            {
                return _Price;
            }
            set
            {
                _Price = value;
                OnPropertyChanged("Price");
            }
        }

        private static PriceViewModel instance;

        public static PriceViewModel GetInstance()
        {
            if (instance == null)
                instance = new PriceViewModel();

            return instance;
        }
    }

}
