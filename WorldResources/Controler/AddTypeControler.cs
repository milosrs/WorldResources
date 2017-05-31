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
using System.Drawing;
using System.Windows.Media.Imaging;
using System.Collections.ObjectModel;

namespace WorldResources.Controler
{
    public class AddTypeControler
    {
        private Model.Type e;
        private bool success = false;
        private View.NewType wind;

        public AddTypeControler(View.NewType w)
        {
            wind = w;
            if (contAdd())
            {
                string img = wind.icoPath.Text;
                e = new Model.Type(wind.IDbox.Text, wind.nameBox.Text.Trim(' '), wind.descBox.Text, img);
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
                GlowingEarth.getInstance().getMaster().notifyChange();
                return;
            }
            else
            {
                wind.Error.Content = "Type with this ID already exists";
                success = false;
                return;
            }
        }

        public bool contAdd()
        {
            if (wind.IDbox.Text.Equals(""))
            {
                wind.Error.Content = "Missing ID";
                return false;
            }
            if (wind.nameBox.Text.Equals(""))
            {
                wind.Error.Content = "Missing name";
                return false;
            }
            if (wind.icoPath.Text.Equals(""))
            {
                wind.Error.Content = "Missing icon";
                return false;
            }
            return true;
        }

        public bool chckTag()
        {
            foreach (Model.Type t in GlowingEarth.getInstance().getMaster().getTypes())
            {
                if (t.getMark().Equals(e.getMark()))
                {
                    success = false;
                    return false;
                }
            }
            return true;
        }

        public void addTag()
        {
            GlowingEarth.getInstance().getMaster().types.Add(e);
        }

        public bool getSuccess()
        {
            return success;
        }
    }
}