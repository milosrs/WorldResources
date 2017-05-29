using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace WorldResources.Model
{
    [Serializable]
    public class Type
    {
        public string mark { get; set; }
        public string name { get; set; }
        public  string desc { get; set; }
        public string img { get; set; }

        public Type(string m, string n, string d, string mg)
        {
            mark = m;
            name = n;
            desc = d;
            img = mg;
        }

        public string getMark()
        {
            return mark;
        }
        public string getName()
        {
            return name;
        }
        public string getDesc()
        {
            return desc;
        }
        public string getImg()
        {
            return img;
        }

        public void setMark(string m)
        {
            mark = m;
        }
        public void setName(string n)
        {
            name = n;
        }
        public void setDesc(string m)
        {
            desc = m;
        }
        public void setImg(string mg)
        {
            img = mg;
        }
    }
}
