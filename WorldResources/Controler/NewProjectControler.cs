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
    class NewProjectControler
    {
        private string projName;
        public NewProjectControler(string name)
        {
            if (GlowingEarth.getInstance().getMaster().getTitle().Contains("*") && !GlowingEarth.getInstance().getMaster().getSerPath().Equals(""))
            {
                if (MessageBox.Show("You have some unsaved changes on current map. Do you want to save those changes?", "Save changes", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    SaveFileDialog fd = new SaveFileDialog();
                    fd.DefaultExt = ".gemap";
                    fd.Filter = "Glowing Earth Map (*.gemap)|*.gemap";

                    DialogResult dr = fd.ShowDialog();
                    if (dr == System.Windows.Forms.DialogResult.OK)
                    {
                        GlowingEarth.getInstance().getMaster().setSerPath(fd.FileName);
                        saveProject(fd.FileName);
                    }
                }
            }
            projName ="Glowing Earth - "+ name;
            GlowingEarth.getInstance().getMaster().getResources().Clear();
            GlowingEarth.getInstance().getMaster().getTypes().Clear();
            GlowingEarth.getInstance().getMaster().getTags().Clear();
            GlowingEarth.getInstance().getMaster().getMapItems().Clear();
            GlowingEarth.getInstance().getMaster().setTitle(projName);

            if (MessageBox.Show("To see this project next time you open Glowing Earth, you should save this project.\nDo you want to save this project?", "Save new project", MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.Yes)
            {
                SaveFileDialog fd = new SaveFileDialog();
                fd.DefaultExt = ".gemap";
                fd.Filter = "Glowing Earth Map (*.gemap)|*.gemap";

                DialogResult dr = fd.ShowDialog();
                if (dr == System.Windows.Forms.DialogResult.OK)
                {
                    GlowingEarth.getInstance().getMaster().setSerPath(fd.FileName);
                    saveProject(fd.FileName);
                }
            }
        }

        public bool saveProject(string path)
        {
            BinaryFormatter fm = new BinaryFormatter();
            FileStream sm = null;

            try
            {
                sm = File.OpenWrite(path);
                fm.Serialize(sm, GlowingEarth.getInstance().getMaster());
                GlowingEarth.getInstance().getMaster().setTitle(GlowingEarth.getInstance().getMaster().getTitle().Replace("*", string.Empty));
                sm.Dispose();
                sm.Close();
                return true;
            }
            catch (Exception e)
            {
                MessageBox.Show("Saving went wrong. Please, try again. If the problem persists, contact your administrator.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Console.WriteLine(e.StackTrace);
                sm.Dispose();
                sm.Close();
                return false;
            }
        }
    }
}
