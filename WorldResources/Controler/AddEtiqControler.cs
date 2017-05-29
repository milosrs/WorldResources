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
        private ObservableCollection<Model.Etiquette> taglist;
        private string path;
        private BinaryFormatter fm;
        private FileStream sm = null;
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

            fm = new BinaryFormatter();
            path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "tagList");

            if(File.Exists(path))
            {
                sm = File.Open(path, FileMode.OpenOrCreate);
                taglist = (ObservableCollection<Model.Etiquette>)fm.Deserialize(sm);
                sm.Dispose();
                sm.Close();
            }
            else
            {
                taglist = new ObservableCollection<Model.Etiquette>();
            }

            if (chckTag())
            {
                addTag();
                success = true;
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
            foreach(Etiquette t in taglist)
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
            taglist.Add(e);
            try
            {
                sm = File.OpenWrite(path);
                fm.Serialize(sm, taglist);
                sm.Dispose();
                sm.Close();
            }
            catch
            {
                success = false;
                return;
            }
            finally
            {
                if (sm != null)
                {
                    sm.Dispose();
                    sm.Close();
                }
            }
        }
        public bool getSuccess()
        {
            return success;
        }
    }
}
