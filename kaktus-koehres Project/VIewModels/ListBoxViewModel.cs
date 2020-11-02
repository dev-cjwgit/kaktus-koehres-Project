using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace kaktus_koehres_Project.VIewModels
{

    public class ListBoxViewViewModel : NotifyPropertyChanged
    {

        public string Cactus_Name { get; set; }

        private static List<string> instance;

        public static List<string> GetInstance()
        {
            if (instance == null)
                instance = new List<string>();

            return instance;
        }
    }
}