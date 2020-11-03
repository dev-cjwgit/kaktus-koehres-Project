using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace kaktus_koehres_Project.VIewModels
{
    public class PageLabelViewModel : NotifyPropertyChanged
    {
        private int _page = 0;
        private int _maxpage = 0;
        public int Page
        {
            get
            {
                return _page;
            }

            set
            {
                if (_maxpage == 0)
                {
                    _page = 0;
                    OnPropertyChanged("Page");
                    return;
                }

                _page = value < 1 ? 1 :(value > _maxpage) ? _maxpage : value ;
                OnPropertyChanged("Page");
            }
        }

        public int MaxPage
        {
            get
            {
                return _maxpage;
            }
            set
            {
                _maxpage = value;
                OnPropertyChanged("MaxPage");
            }
        }

        private static PageLabelViewModel instance;

        public static PageLabelViewModel GetInstacne()
        {
            if (instance == null)
                instance = new PageLabelViewModel();
            return instance;
        }
    }
}
