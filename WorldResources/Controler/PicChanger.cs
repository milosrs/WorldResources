using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorldResources.Controler
{
    public class PicChanger : Model.ObservableObject
    {
        private string _path = "/r/picgohere.png";
        public string path {
            get
            {
                return _path;
            }
            set
            {
                 _path = value;
                 OnPropertyChanged("path");
            }
        }
    }
}
