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
        public DeleteControler(Model.Resource r)
        {
            foreach(Model.Resource d in GlowingEarth.getInstance().getMaster().getResources())
            {
                if (d.getMark().Equals(r.getMark()))
                {
                    GlowingEarth.getInstance().getMaster().getResources().Remove(d);
                    break;
                }
            }
            ObservableCollection<Model.MapItem> mati = new ObservableCollection<Model.MapItem>();

            foreach(Model.MapItem mi in GlowingEarth.getInstance().getMaster().getMapItems())
            {
                if (!mi.getID().Equals(r.getMark()))
                {
                    mati.Add(mi);
                }
            }
            GlowingEarth.getInstance().getMaster().setMapItems(mati);
            success = true;
        }

        public DeleteControler(Model.Etiquette e)
        {
            foreach (Model.Etiquette d in GlowingEarth.getInstance().getMaster().getTags())
            {
                if (d.getID().Equals(e.getID()))
                {
                    GlowingEarth.getInstance().getMaster().getTags().Remove(d);
                    break;
                }
            }
            foreach(Model.Resource r in GlowingEarth.getInstance().getMaster().getResources())
            {
                foreach(Model.Etiquette et in r.getTags())
                {
                    if (et.getID().Equals(e.getID()))
                    {
                        r.getTags().Remove(et);
                        break;
                    }
                }
                r.setTags(r.getTags());                                //Cudna greska bez ovoga -.-
            }
            
            success = true;
        }
        public DeleteControler(Model.Type e)
        {
            foreach (Model.Type d in GlowingEarth.getInstance().getMaster().getTypes())
            {
                if (d.getMark().Equals(e.getMark()))
                {
                    GlowingEarth.getInstance().getMaster().getTypes().Remove(d);
                    break;
                }
            }
            ObservableCollection<Model.Resource> temp = new ObservableCollection<Model.Resource>();
            foreach(Model.Resource r in GlowingEarth.getInstance().getMaster().resources)
            {
                temp.Add(r);
            }
            for(int i=0; i<temp.Count; i++)
            {
                if (e.getMark().Equals(temp[i].getType().getMark()))
                {
                    Model.Resource rz = temp[i];
                    ObservableCollection<Model.MapItem> mati = new ObservableCollection<Model.MapItem>();

                    foreach (Model.MapItem mi in GlowingEarth.getInstance().getMaster().getMapItems())
                    {
                        if (!mi.getID().Equals(rz.getMark()))
                        {
                            mati.Add(mi);
                        }
                    }
                    GlowingEarth.getInstance().getMaster().setMapItems(mati);
                    GlowingEarth.getInstance().getMaster().getResources().Remove(temp[i]);
                }
            }
            
            success = true;
        }

        public bool getSucc()
        {
            return success;
        }
    }
}
