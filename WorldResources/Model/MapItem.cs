using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
namespace WorldResources.Model
{
    [Serializable]
    public class MapItem : INotifyPropertyChanged
    {
        private string ID;
        private string _name;
        private string _path;
        private Point _position;

        [field:NonSerialized]
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string name)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(name));
            }
        }
        public string name
        {
            get
            {
                return _name;
            }
            set
            {
                _name = value;
                OnPropertyChanged("name");
            }
        }
        public string path
        {
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
        public Point position
        {
            get
            {
                return _position;
            }
            set
            {
                _position = value;
                OnPropertyChanged("position");
            }
        }
        public MapItem(string id, string name, string path)
        {
            ID = id;
            this.name = name;
            this.path = path;
        }

        public Point getPosition()
        {
            return position;
        }
        public void setPosition(Point pos)
        {
            position = pos;
        }
        public string getID()
        {
            return ID;
        }
        public void setID(string id)
        {
            ID = id;
        }
        public string getName()
        {
            return _name;
        }
        public string getPath()
        {
            return _path;
        }
        public void setName(string n)
        {
            _name=n;
        }
        public void setPath(string p)
        {
            _path=p;
        }
    }
}
