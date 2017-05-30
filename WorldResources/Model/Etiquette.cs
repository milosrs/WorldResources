using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using WorldResources.View;

namespace WorldResources.Model
{
    [Serializable]
    public class Etiquette
    {
        public string mark { get; set; }
        public string colCod { get; set; }
        public string desc { get; set; }

        public bool isPartOfRes { get; set; }
        public Etiquette(string m, string c, string d)
        {
            mark = m;
            colCod = c;
            desc = d;
        }
        

        public string getID()
        {
            return mark;
        }
        public string getColor()
        {
            return colCod;
        }
        public string getDesc()
        {
            return desc;
        }
        public bool getPartOfRes()
        {
            return isPartOfRes;
        }
        public void setID(string id)
        {
            mark = id;
        }
        public void setCol(string c)
        {
            colCod = c;
        }
        public void setDesc(string d)
        {
            desc = d;
        }
    }
}
