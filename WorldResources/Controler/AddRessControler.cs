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
using System.Collections.ObjectModel;
using Xceed.Wpf.Toolkit;

namespace WorldResources.Controler
{
    public class AddRessControler
    {
        private Model.Resource res;
        private ObservableCollection<Model.Resource> reslist;
        private string path;
        private BinaryFormatter fm;
        private FileStream sm=null;
        private bool success = false;
        private View.NewRes wind;
        public AddRessControler(View.NewRes refer)
        {
            wind = refer;
            res = new Resource();
            fm = new BinaryFormatter();
            if (contAdd())
            {
                foreach (Model.Etiquette b in wind.tagovi)
                {
                    if (b.isPartOfRes)
                    {
                        res.taglist.Add(b);
                    }
                }
                res.setName(wind.nameBox.Text);
                res.setMark(wind.IDBox.Text);
                res.setDesc(wind.descBox.Text);
                if (wind.radioFreq.IsChecked == true)
                {
                    res.setFreq(Resource.FreqType.FREQUENT);
                }
                else if (wind.radioRare.IsChecked == true)
                {
                    res.setFreq(Resource.FreqType.RARE);
                }
                else if (wind.radioUniv.IsChecked == true)
                {
                    res.setFreq(Resource.FreqType.UNIVERSAL);
                }

                res.setType((Model.Type)wind.typeBox.SelectedItem);
                res.setPrice(Double.Parse(wind.priceBox.Text));
                if (wind.radioScoop.IsChecked == true)
                {
                    res.setUnit(Resource.Units.SCOOP);
                }
                else if (wind.radioBarrel.IsChecked == true)
                {
                    res.setUnit(Resource.Units.BAREL);
                }
                else if (wind.radioT.IsChecked == true)
                {
                    res.setUnit(Resource.Units.T);
                }
                else if (wind.radioKG.IsChecked == true)
                {
                    res.setUnit(Resource.Units.KG);
                }
                res.setOb(wind.ren.IsChecked.Value);
                res.setStr(wind.Strt.IsChecked.Value);
                res.setExp(wind.exp.IsChecked.Value);
                res.setDate(wind.Date.SelectedDate.Value);
                if (wind.icoPath.Text.Equals(""))
                {
                    res.setIcon(res.getType().getImg());
                    res.setHasTypeImg(true);
                }
                else
                {
                    res.setIcon(wind.icoPath.Text);
                    res.setHasTypeImg(false);
                }
            }
            else
            {
                success = false;
                return;
            }
            //Resource created <-----------------------------------
            fm = new BinaryFormatter();
            path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "reslist");

            if (File.Exists(path))
            {
                sm = File.Open(path, FileMode.OpenOrCreate);
                reslist = (ObservableCollection<Model.Resource>)fm.Deserialize(sm);
                sm.Dispose();
                sm.Close();
            }
            else
            {
                reslist = new ObservableCollection<Model.Resource>();
            }

            if (chckRes())
            {
                addRes();
                success = true;
            }
            else
            {
                wind.Error.Content = "Resource with this ID already exists";
                success = false;
            }
        }

        private bool contAdd()
        {
            if (wind.IDBox.Text.Equals(""))
            {
                wind.Error.Content = "Missing ID";
                return false;
            }
            if (wind.nameBox.Text.Equals(""))
            {
                wind.Error.Content = "Missing name";
                return false;
            }
            if (wind.radioFreq.IsChecked == false && wind.radioRare.IsChecked == false && wind.radioUniv.IsChecked == false)
            {
                wind.Error.Content = "No frequency selected";
                return false;
            }
            if (wind.radioBarrel.IsChecked == false && wind.radioKG.IsChecked == false && wind.radioScoop.IsChecked == false && wind.radioT.IsChecked == false)
            {
                wind.Error.Content = "No unit selected";
                return false;
            }
            if (wind.priceBox.Text.Equals(""))
            {
                wind.Error.Content = "Missing price";
                return false;
            }
            if (wind.typeBox.SelectedItem == null)
            {
                wind.Error.Content = "No type selected";
                return false;
            }
            if (wind.Date.SelectedDate==null)
            {
                wind.Error.Content = "No date selected";
                return false;
            }
            if(wind.Date.SelectedDate > DateTime.Now)
            {
                wind.Error.Content = "Invalid date";
                return false;
            }
            return true;
        }
        private void addRes()
        {
            reslist.Add(res);

            if (File.Exists(path))
            {
                try
                {
                    sm = File.OpenWrite(path);
                    fm.Serialize(sm, reslist);
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
            else
            {
                try {
                    sm = File.OpenWrite(path);
                    fm.Serialize(sm, reslist);
                    sm.Dispose();
                    sm.Close();
                }
                catch
                {
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
        }

        private bool chckRes()
        {
            foreach (Model.Resource r in reslist)
            {
                if (res.getMark().Equals(r.getMark()))
                {
                    return false;
                }
            }
            return true;
        }

        public bool getSuccess()
        {
            return success;
        }
    }
}
