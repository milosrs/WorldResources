﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WorldResources.Controler
{
    class SaveProjectAsControler
    {
        public SaveProjectAsControler()
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
