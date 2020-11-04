using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace kaktus_koehres_Project.VIewModels
{
    public class ImageForm
    {
        public string url { get; set; }
        public string title { get; set; }
    }

    public class ImageFlipModel
    {
        private static List<ImageForm> model;

        public static List<ImageForm> GetInstance()
        {
            if (model == null)
                model = new List<ImageForm>();
            return model;
        }
        /// <summary>
        /// idx 0 : url
        /// idx 1 : title
        /// </summary>
        /// <param name="idx"></param>
        /// <returns></returns>
        public static List<string> GetDataSource(int idx)
        {
            List<string> temp = new List<string>();
            foreach (var items in model)
                temp.Add(idx==0?items.url:items.title);

            return temp;
        }
    }

    public class ImageFlipViewModel : NotifyPropertyChanged
    {
        private string _url;
        public string Url
        {
            get
            {
                return _url;
            }

            set
            {
                _url = value;
                OnPropertyChanged("Url");
            }
        }
        private static List<string> instance;

        public static List<string> GetInstance()
        {
            if (instance == null)
                instance = new List<string>();
            return instance;
        }

        public static void SetSource(List<string> obj)
        {
            instance.Clear();
            foreach (var item in obj)
            {
                instance.Add(item);
            }
        }
    }
}
