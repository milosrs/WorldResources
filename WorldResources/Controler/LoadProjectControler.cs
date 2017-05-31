using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WorldResources.Controler
{
    public class LoadProjectControler
    {
        private string path;

        public LoadProjectControler()
        {
            if (GlowingEarth.getInstance().getMaster().getTitle().Contains("*"))
            {
                DialogResult dr = MessageBox.Show("Do you want to save changes?", "Save changes", MessageBoxButtons.YesNo);
                if (dr == DialogResult.Yes)
                {
                    OpenFileDialog fid = new OpenFileDialog();
                    fid.DefaultExt = ".gemap";
                    fid.Filter = "Glowing Earth Map (*.gemap)|*.gemap";

                    DialogResult diir = fid.ShowDialog();
                    if (diir == System.Windows.Forms.DialogResult.OK)
                    {
                        SaveProjectControler spc = new SaveProjectControler();
                    }
                }
            }
            OpenFileDialog fd = new OpenFileDialog();
            fd.DefaultExt = ".gemap";
            fd.Filter = "Glowing Earth Map (*.gemap)|*.gemap";

            DialogResult dir = fd.ShowDialog();
            if (dir == System.Windows.Forms.DialogResult.OK)
            {
                path = fd.FileName;
                BinaryFormatter fm = new BinaryFormatter();
                FileStream sm = null;
                try {
                    sm = File.OpenRead(path);
                    Model.MasterClass x = (Model.MasterClass)fm.Deserialize(sm);
                    GlowingEarth.getInstance().getMaster().setTags(x.getTags());
                    GlowingEarth.getInstance().getMaster().setTypes(x.getTypes());
                    GlowingEarth.getInstance().getMaster().setResources(x.getResources());
                    GlowingEarth.getInstance().getMaster().setTitle(x.getTitle());
                    GlowingEarth.getInstance().getMaster().setSerPath(x.getSerPath());
                    GlowingEarth.getInstance().getMaster().setMapItems(x.getMapItems());
                    ThisWasLastController tc = new ThisWasLastController("save");
                }
                catch(Exception e)
                {
                    MessageBox.Show("Loading went wrong. Please, try again. If the problem persists, contact your administrator.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
        }
    }
}
