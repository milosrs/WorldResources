using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;

namespace WorldResources.Controler
{
    public class ThisWasLastController
    {
        private string path;
        private string getFromHere;
        public ThisWasLastController(string mode)
        {
            path = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "lastOpened");
            if (mode.Equals("save"))
            {
                save();
            }
            else if (mode.Equals("load"))
            {
                load();
            }
        }

        public void save()
        {
            BinaryFormatter fm = new BinaryFormatter();
            FileStream sm = null;

            try
            {
                sm = File.OpenWrite(path);
                fm.Serialize(sm, GlowingEarth.getInstance().getMaster().getSerPath());
            }
            catch (Exception e)
            {
                Console.WriteLine(e.StackTrace);
                
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

        public void load()
        {
            BinaryFormatter fm = new BinaryFormatter();
            FileStream sm = null;
            try
            {
                sm = File.OpenRead(path);
                getFromHere = (string)fm.Deserialize(sm);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.StackTrace);
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

        public string GetFromHere()
        {
            return getFromHere;
        }
    }
}
