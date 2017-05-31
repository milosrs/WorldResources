using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorldResources.Model
{
    [Serializable]
    public class MasterClass : ObservableObject
    {
        private ObservableCollection<Model.Etiquette> _tags;        //tagovi
        private ObservableCollection<Model.Resource> _resources;    //resursi
        private ObservableCollection<Model.Type> _types;            //tipovi
        private ObservableCollection<Model.MapItem> _resOnCanvas;   //Resursi koji su na kanvasu
        private string _title;                                      //Ime projekta
        private string _serPath;                                    //Gde je projekat?
        private string lastOpenedProject;                           //Koji je projekat zadnji put bio otvoren?

        [field:NonSerialized]
        public event PropertyChangedEventHandler PropertyChanged;
        
        protected virtual void OnPropertyChanged(string name)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(name));
            }
        }
        /*--------------------BINDING--------------------*/
        public ObservableCollection<Model.Etiquette> tags
        {
            get
            {
                return _tags;
            }
            set
            {
                _tags = value;
                OnPropertyChanged("tags");
            }
        }
        public ObservableCollection<Model.Resource> resources
        {
            get
            {
                return _resources;
            }
            set
            {
                _resources = value;
                OnPropertyChanged("resources");
            }
        }
        public ObservableCollection<Model.Type> types
        {
            get
            {
                return _types;
            }
            set
            {
                _types = value;
                OnPropertyChanged("types");
            }
        }
        public ObservableCollection<Model.MapItem> resOnCanvas
        {
            get
            {
                return _resOnCanvas;
            }
            set
            {
                _resOnCanvas = value;
                OnPropertyChanged("resOnCanvas");
            }
        }
        public string title
        {
            get
            {
                return _title;
            }
            set
            {
                _title = value;
                OnPropertyChanged("title");
            }
        }
        public string serPath
        {
            get
            {
                return _serPath;
            }
            set
            {
                _serPath = value;
                OnPropertyChanged("serPath");
            }
        }

        public MasterClass()
        {
            tags = new ObservableCollection<Model.Etiquette>();
            resources = new ObservableCollection<Model.Resource>();
            types = new ObservableCollection<Model.Type>();
            resOnCanvas = new ObservableCollection<Model.MapItem>();
            title = "Glowing Earth - Default";
        }

        /*----------------------------List geteri------------------------*/
        public ObservableCollection<Model.Resource> getResources()
        {
            return _resources;
        }
        public ObservableCollection<Model.Type> getTypes()
        {
            return _types;
        }
        public ObservableCollection<Model.Etiquette> getTags()
        {
            return _tags;
        }

        public ObservableCollection<Model.MapItem> getMapItems()
        {
            return _resOnCanvas;
        }
        /*----------------------------List seteri------------------------*/
        public void setResources(ObservableCollection<Model.Resource> res)
        {
            _resources = res;
            resources = _resources;
        }
        public void setTypes(ObservableCollection<Model.Type> t)
        {
            _types = t;
            types = _types;
        }
        public void setTags(ObservableCollection<Model.Etiquette> e)
        {
            _tags = e;
            tags = _tags;
        }

        public void setMapItems(ObservableCollection<Model.MapItem> mi)
        {
            _resOnCanvas = mi;
            resOnCanvas = _resOnCanvas;
        }

        public void setTitle(string t)
        {
            _title = t;
            title = _title;
        }
        public string getTitle()
        {
            return _title;
        }

        public string getSerPath()
        {
            return serPath;
        }
        public void setSerPath(string sp)
        {
            serPath = sp;
        }

        public void notifyChange()
        {
            if(!_title.Contains("*"))
                _title = title + "*";
        }

        public string getLastOpened()
        {
            return lastOpenedProject;
        }
        public void setLastOpened(string lo)
        {
            lastOpenedProject = lo;
        }
    }
}
