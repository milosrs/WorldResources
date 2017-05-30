using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Data;
using System.Resources;
using System.Runtime.Serialization.Formatters.Binary;
using WorldResources.Model;
using System.Collections.ObjectModel;

namespace WorldResources.Controler
{
    public class AddEtiqControler
    {
        private Model.Etiquette e;
        private bool success = false;
        private View.NewTag wind;

        public AddEtiqControler(View.NewTag w)
        {
            wind = w;
            if (contAdd())
            {
                e = new Etiquette(wind.IDbox.Text, wind.colorp.SelectedColor.ToString(), wind.descBox.Text);
            }
            else
            {
                success = false;
                return;
            }

            

            if (chckTag())
            {
                addTag();
                success = true;
                GlowingEarth.getInstance().getMaster().setTitle(GlowingEarth.getInstance().getMaster().getTitle() + "*");
                return;
            }
            else
            {
                wind.Errl.Content = "Tag with this ID already exists";
                success = false;
                return;
            }
        }

        public bool contAdd()
        {
            if (wind.IDbox.Text.Equals(""))
            {
                wind.Errl.Content = "Missing ID";
                return false;
            }
            return true;
        }

        public bool chckTag()
        {
            foreach(Etiquette t in GlowingEarth.getInstance().getMaster().getTags())
            {
                if (t.getID().Equals(e.getID()))
                {
                    success = false;
                    return false;
                }
            }
            return true;
        }

        public void addTag()
        {
            GlowingEarth.getInstance().getMaster().getTags().Add(e);
            success = true;
        }
        public bool getSuccess()
        {
            return success;
        }
    }
}
