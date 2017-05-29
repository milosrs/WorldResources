using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;

namespace WorldResources.Controler
{
    class DeleteControler
    {
        private bool success;
        public DeleteControler(Model.Resource r, GlowingEarth ge)
        {
            foreach(Model.Resource d in ge.resources)
            {
                if (d.getMark().Equals(r.getMark()))
                {
                    ge.resources.Remove(d);
                    success = saveList(ge.resources);
                    break;
                }
            }
        }

        public DeleteControler(Model.Etiquette e, GlowingEarth ge)
        {
            foreach (Model.Etiquette d in ge.tags)
            {
                if (d.getID().Equals(e.getID()))
                {
                    ge.tags.Remove(d);
                    success = saveList(ge.tags);
                    break;
                }
            }
            foreach(Model.Resource r in ge.resources)
            {
                List<Model.Etiquette> temp = r.getTags();       //Lista etiketa
                foreach(Model.Etiquette et in temp)             //Ako se etiketa slaze sa prosledjenom
                {
                    if (et.getID().Equals(e.getID()))
                    {
                        temp.Remove(et);                        //Obrisi je iz temp liste
                    }
                }
                r.setTags(temp);                                //Nova lista etiketa
            }

        }
        public DeleteControler(Model.Type e, GlowingEarth ge)
        {
            foreach (Model.Type d in ge.types)
            {
                if (d.getMark().Equals(e.getMark()))
                {
                    ge.types.Remove(d);
                    success = saveList(ge.types);
                    break;
                }
            }
            ObservableCollection<Model.Resource> novi = new ObservableCollection<Model.Resource>();
            for(int i=0; i<ge.resources.Count; i++)
            {
                novi.Add(ge.resources[i]);
            }
            foreach(Model.Resource r in ge.resources)
            {
                Model.Type temp = r.getType();
                if (temp.getMark().Equals(e.getMark()))
                {
                    novi.Remove(r);        //Obrisi taj resurs ako obrises tip
                }
            }
            ge.resources = novi;
            saveList(ge.resources);
        }
        public bool saveList(ObservableCollection<Model.Resource> c)
        {
            BinaryFormatter fm = new BinaryFormatter();
            FileStream sm = null;
            string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "reslist");

            if (File.Exists(path))
            {
                sm = File.OpenWrite(path);
                fm.Serialize(sm, c);
                sm.Dispose();
            }
            else
            {
                return false;
            }
            return true;
        }

        public bool saveList(ObservableCollection<Model.Etiquette> c)
        {
            BinaryFormatter fm = new BinaryFormatter();
            FileStream sm = null;
            string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "tagList");

            if (File.Exists(path))
            {
                sm = File.OpenWrite(path);
                fm.Serialize(sm, c);
                sm.Dispose();
            }
            else
            {
                return false;
            }
            return true;
        }

        public bool saveList(ObservableCollection<Model.Type> c)
        {
            BinaryFormatter fm = new BinaryFormatter();
            FileStream sm = null;
            string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "typeList");

            if (File.Exists(path))
            {
                sm = File.OpenWrite(path);
                fm.Serialize(sm, c);
                sm.Dispose();
            }
            else
            {
                return false;
            }
            return true;
        }

        public bool getSucc()
        {
            return success;
        }
    }
}
