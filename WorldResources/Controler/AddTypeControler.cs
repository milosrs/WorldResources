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
        private ObservableCollection<Model.Type> typelist;
        private string path;
        private BinaryFormatter fm;
        private FileStream sm = null;
        private bool success = false;
        private View.NewType wind;

        public AddTypeControler(View.NewType w)
        {
            wind = w;
            if (contAdd())
            {
                string img = wind.icoPath.Text;
                e = new Model.Type(wind.IDbox.Text, wind.nameBox.Text, wind.descBox.Text, img);
            }
            else
            {
                success = false;
                return;
            }

            fm = new BinaryFormatter();
            path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "typeList");

            if (File.Exists(path))
            {
                sm = File.Open(path, FileMode.OpenOrCreate);
                typelist = (ObservableCollection<Model.Type>)fm.Deserialize(sm);
                sm.Dispose();
                sm.Close();
            }
            else
            {
                typelist = new ObservableCollection<Model.Type>();
            }

            if (chckTag())
            {
                addTag();
                success = true;
                if (sm != null)
                {
                    sm.Dispose();
                    sm.Close();
                }
                return;
            }
            else
            {
                wind.Error.Content = "Type with this ID already exists";
                success = false;
                if(sm != null)
                {
                    sm.Dispose();
                    sm.Close();
                }
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
            foreach (Model.Type t in typelist)
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
            typelist.Add(e);
            try
            {
                sm = File.OpenWrite(path);
                fm.Serialize(sm, typelist);
                sm.Dispose();
                sm.Close();
            }
            catch (Exception e)
            {
                Console.WriteLine("Bracooooo");
                sm.Dispose();
                sm.Close();
                success = false;
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