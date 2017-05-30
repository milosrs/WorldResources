using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.ComponentModel;

namespace WorldResources.Model
{
    
    [Serializable]
    public class Resource
    {
        public enum FreqType { RARE, FREQUENT, UNIVERSAL }; //Redak, Cest, Univerzalan
        public enum Units { SCOOP, BAREL, T, KG };          //Merica, Barel, Tona, Kilogram

        public string ID { get; set; }          //oznaka
        public string name { get; set; }         //ime
        public string desc { get; set; }            //opis
        public Type type { get; set; }              //tip
        public FreqType freq { get; set; }         //frekvencija
        public double price { get; set; }          //cena
        public Units unit { get; set; }            //mera
        public bool ob { get; set; }
        public bool str { get; set; }
        public bool eksp { get; set; }     //Obnovljiv, strateska vaznost, eksp
        public string ico { get; set; }             //ikonica (kopira se u poseban folder)
        public DateTime datum { get; set; }       //Datum
        public List<Etiquette> taglist { get; set; }

        private bool hasTypeImg;
      
        public Resource(string m, string n, string d, Type t, FreqType f, double p, Units u, bool o, bool s, bool e, DateTime dt, string pic)
        {
            ID = m;
            name = n;
            desc = d;
            type = t;
            freq = f;
            price = p;
            unit = u;
            ob = o; str = s; eksp = e; datum = dt;
            ico = pic;
            hasTypeImg = false;
            taglist = new List<Etiquette>();
        }

        public Resource()
        {
            taglist = new List<Etiquette>();
        }

        //GETERI <----------------------------------
        public string getMark()
        {
            return ID;
        }

        public string getIco()
        {
            return ico;
        }

        public string getName()
        {
            return name;
        }

        public string getDesc()
        {
            return desc;
        }

        public Type getType()
        {
            return type;
        }

        public FreqType getFreq()
        {
            return freq;
        }

        public double getPrice()
        {
            return price;
        }

        public Units getUnit()
        {
            return unit;
        }

        public bool getOb()
        {
            return ob;
        }

        public bool getStr()
        {
            return str;
        }

        public bool getExp()
        {
            return eksp;
        }

        public List<Etiquette> getTags()
        {
            return taglist;
        }
        //SETERI        <<<-------------------------------------
        public void setTags(List<Etiquette> t)
        {
            taglist = t;
        }
        public void setName(string n)
        {
            name = n;
        }

        public void setDesc(string D)
        {
            desc = D;
        }

        public void setMark(string n)
        {
            ID = n;
        }

        public void setFreq(FreqType n)
        {
            freq = n;
        }

        public void setType(Type t)
        {
            type = t;
        }

        public void setPrice(double p)
        {
            price = p;
        }

        public void setUnit(Units u)
        {
            unit = u;
        }

        public void setOb(bool u)
        {
            ob = u;
        }

        public void setExp(bool u)
        {
            eksp = u;
        }

        public void setStr(bool u)
        {
            str = u;
        }

        public void setDate(DateTime u)
        {
            datum = u;
        }

        public void setIcon(string pic)
        {
            ico = pic;
        }

        public override string ToString()
        {
            return name;
        }

        public void setHasTypeImg(bool state)
        {
            hasTypeImg = state;
        }

        public bool getHasTypeImg()
        {
            return hasTypeImg;
        }
    }
}
