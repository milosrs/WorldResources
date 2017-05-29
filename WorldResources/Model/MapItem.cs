using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
namespace WorldResources.Model
{
    public class MapItem
    {
        private string _name;
        private string _path;
        private Point _position;
        public string name
        {
            get;
            set;
        }
        public string path
        {
            get;
            set;
        }
        public Point position
        {
            get;
            set;
        }
        public MapItem(string name, string path)
        {
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

    }
}
