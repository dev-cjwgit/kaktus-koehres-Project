using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace kaktus_koehres_Project.VIewModels
{
    public class CactusForm
    {
        public string Cactus_Name { get; set; }
        public string Page_Code { get; set; }
    }

    public class CactusModel
    {
        private static List<CactusForm> model;

        public static List<CactusForm> GetInstance()
        {
            if (model == null)
                model = new List<CactusForm>();

            return model;
        }

        /// <summary>
        /// Model to ViewModel
        /// </summary>
        /// <returns></returns>
        public static List<string> GetDataSource()
        {
            List<string> temp = new List<string>();
            foreach(var items in model)
                temp.Add(items.Cactus_Name);

            return temp;
        }
    }


    public class CactusViewModel : NotifyPropertyChanged
    {
        private static List<string> instance;
        public static List<string> Instance
        {
            get { return instance; }
            set
            {
                instance = value;
            }
        }
        public static List<string> GetInstance()
        {
            if (Instance == null)
                Instance = new List<string>();

            return Instance;
        }

        public static void SetSource(List<string> obj)
        {
            instance.Clear();
            foreach(var item in obj)
            {
                instance.Add(item);
            }
        }
    }
}