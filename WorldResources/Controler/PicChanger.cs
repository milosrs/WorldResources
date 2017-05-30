using System;
using System.Collections.Generic;
using System.ComponentModel;
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

        [field:NonSerialized]
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string name)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(name));
            }
        }
    }
}
