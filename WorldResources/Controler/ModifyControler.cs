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
    class ModifyControler
    {
        private bool success;

        private Model.Resource res;
        private Model.Etiquette tag;
        private Model.Type type;

        private View.ResEditor re;
        private View.TypeEditor te;
        private View.TagEditor tage;

        public ModifyControler(View.ResEditor resed)
        {
            re = resed;
            res = makeRes();
            if (res != null)
            {
                for (int i = 0; i < GlowingEarth.getInstance().getMaster().getResources().Count; i++)
                {
                    if (res.getMark().Equals(GlowingEarth.getInstance().getMaster().getResources()[i].getMark()))
                    {
                        GlowingEarth.getInstance().getMaster().getResources()[i] = res;
                        break;
                    }
                    
                }
            }
            else
            {
                success = false;
                return;
            }

            foreach(Model.MapItem mi in GlowingEarth.getInstance().getMaster().getMapItems())
            {
                if (mi.getID().Equals(res.getMark()))
                {
                    mi.setName(res.getName());
                    mi.setPath(res.getIco());
                }
            }
            success = true;
            GlowingEarth.getInstance().getMaster().setTitle(GlowingEarth.getInstance().getMaster().getTitle() + "*");
        }
        public ModifyControler(View.TypeEditor resed)
        {
            te = resed;

            if ((type = makeType()) != null)
            {
                for (int i = 0; i < GlowingEarth.getInstance().getMaster().getTypes().Count; i++)
                {
                    if (type.getMark().Equals(GlowingEarth.getInstance().getMaster().getTypes()[i].getMark()))
                    {
                        GlowingEarth.getInstance().getMaster().getTypes()[i] = type;
                        break;
                    }
                }
            }
            else
            {
                success = false;
                return;
            }

            GlowingEarth.getInstance().itemList.Items.Refresh();

            ObservableCollection<Model.Resource> temp = GlowingEarth.getInstance().getMaster().getResources();
            for(int i=0; i<temp.Count; i++)
            {
                if (GlowingEarth.getInstance().getMaster().getResources()[i].getType().getMark().Equals(type.getMark()))
                {
                    Model.Resource r = temp[i];
                    r.setType(type);
                    if (r.getHasTypeImg())
                        r.setIcon(type.getImg());
                    ObservableCollection<Model.MapItem> mapitems = GlowingEarth.getInstance().getMaster().getMapItems();
                    for(int j=0; j<mapitems.Count; j++)
                    {
                        if (mapitems[j].getID().Equals(r.getMark()))
                        {
                            mapitems[j].setPath(r.getIco());
                        }
                    }
                    GlowingEarth.getInstance().getMaster().setMapItems(mapitems);
                }
            }
            GlowingEarth.getInstance().getMaster().setResources(temp);
            success = true;
            GlowingEarth.getInstance().map.Items.Refresh();
            GlowingEarth.getInstance().getMaster().setTitle(GlowingEarth.getInstance().getMaster().getTitle() + "*");
        }
        public ModifyControler(View.TagEditor resed)
        {
            tage = resed;
            if ((tag = makeTag()) != null){
                for (int i = 0; i < GlowingEarth.getInstance().getMaster().getTags().Count; i++)
                {
                    if (tag.getID().Equals(GlowingEarth.getInstance().getMaster().getTags()[i].getID()))
                    {
                        GlowingEarth.getInstance().getMaster().getTags()[i] = tag;
                        break;
                    }
                }
            }
            else
            {
                success = false;
                return;
            }
            foreach(Model.Resource r in GlowingEarth.getInstance().getMaster().getResources())
            {
                List<Model.Etiquette> temp = r.getTags();
                int j = 0;
                foreach (Model.Etiquette et in temp)
                {
                    if (et.getID().Equals(tag.getID()))
                    {
                        temp[j] = tag;
                        break;
                    }
                    j++;
                }
                r.setTags(temp);
            }
            success = true;
            GlowingEarth.getInstance().getMaster().setTitle(GlowingEarth.getInstance().getMaster().getTitle() + "*");
        }

        public bool saveList(ObservableCollection<Model.Resource> c)
        {
            if (c == null)
            {
                return false;
            }
            BinaryFormatter fm = new BinaryFormatter();
            FileStream sm = null;
            string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "reslist");

            if (File.Exists(path))
            {
                sm = File.OpenWrite(path);
                fm.Serialize(sm, c);
                sm.Dispose();
            }
            else
            {
                return false;
            }
            return true;
        }

        public bool saveList(ObservableCollection<Model.Etiquette> c)
        {
            if (c == null)
            {
                return false;
            }
            BinaryFormatter fm = new BinaryFormatter();
            FileStream sm = null;
            string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "tagList");

            if (File.Exists(path))
            {
                sm = File.OpenWrite(path);
                fm.Serialize(sm, c);
                sm.Dispose();
            }
            else
            {
                return false;
            }
            return true;
        }

        public bool saveList(ObservableCollection<Model.Type> c)
        {
            if (c == null)
            {
                return false;
            }
            BinaryFormatter fm = new BinaryFormatter();
            FileStream sm = null;
            string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "typeList");

            if (File.Exists(path))
            {
                sm = File.OpenWrite(path);
                fm.Serialize(sm, c);
                sm.Dispose();
            }
            else
            {
                return false;
            }
            return true;
        }

        public bool getSucc()
        {
            return success;
        }

        public Model.Resource makeRes()
        {
            res = new Model.Resource();
            if (contAddRes())
            {
                res.setName(re.nameBox.Text);
                res.setMark(re.IDBox.Text);
                res.setDesc(re.descBox.Text);
                if (re.radioFreq.IsChecked == true)
                {
                    res.setFreq(Model.Resource.FreqType.FREQUENT);
                }
                else if (re.radioRare.IsChecked == true)
                {
                    res.setFreq(Model.Resource.FreqType.RARE);
                }
                else if (re.radioUniv.IsChecked == true)
                {
                    res.setFreq(Model.Resource.FreqType.UNIVERSAL);
                }

                res.setType((Model.Type)re.typeBox.SelectedItem);
                res.setPrice(Double.Parse(re.priceBox.Text));
                if (re.radioScoop.IsChecked == true)
                {
                    res.setUnit(Model.Resource.Units.SCOOP);
                }
                else if (re.radioBarrel.IsChecked == true)
                {
                    res.setUnit(Model.Resource.Units.BAREL);
                }
                else if (re.radioT.IsChecked == true)
                {
                    res.setUnit(Model.Resource.Units.T);
                }
                else if (re.radioKG.IsChecked == true)
                {
                    res.setUnit(Model.Resource.Units.KG);
                }
                res.setOb(re.ren.IsChecked.Value);
                res.setStr(re.Strt.IsChecked.Value);
                res.setExp(re.exp.IsChecked.Value);
                res.setDate(re.Date.SelectedDate.Value);
                res.setHasTypeImg(re.selectedResource.getHasTypeImg());

                bool this_is_type = false;
                foreach(Model.Type t in re.types)
                {
                    if (t.getImg().Equals(re.picpath))
                    {
                        this_is_type = true;
                        break;
                    }
                }

                if (re.icoPath.Text.Equals("") || (res.getHasTypeImg()))
                {
                    bool novo = false;
                    foreach (Model.Type t in GlowingEarth.getInstance().getMaster().types)
                    {
                        String path = t.getImg();
                        if ((path.Equals(((Model.Type)re.typeBox.SelectedItem).getImg()) || path.Equals("")) && this_is_type)
                        {
                            res.setIcon(res.getType().getImg());
                            res.setHasTypeImg(true);
                            novo = true;
                            break;
                        }
                    }
                    if (!novo)
                    {
                        res.setIcon(re.icoPath.Text);
                        res.setHasTypeImg(false);
                    }
                }
                else
                {
                    res.setIcon(re.icoPath.Text);
                    res.setHasTypeImg(false);
                }

                res.getTags().Clear();

                bool dontModify=true;
                List<Model.Etiquette> et = new List<Model.Etiquette>();
                foreach (Model.Etiquette e in re.selectedResource.getTags())
                {
                    if (e.isPartOfRes)
                    {
                        dontModify = false;
                        et.Add(e);
                    }
                }
                res.setTags(et);
                if (!dontModify)
                {
                    foreach (Model.Etiquette e in GlowingEarth.getInstance().getMaster().getTags())
                    {
                        if (e.isPartOfRes)
                        {
                            dontModify = false;
                            et.Add(e);
                        }
                    }
                    res.setTags(et);
                }
            }
            else
            {
                success = false;
                return null;
            }
            return res;
        }

        private bool contAddRes()
        {
            if (re.IDBox.Text.Equals(""))
            {
                re.Error.Content = "Missing ID";
                return false;
            }
            if (re.nameBox.Text.Equals(""))
            {
                re.Error.Content = "Missing name";
                return false;
            }
            if (re.radioFreq.IsChecked == false && re.radioRare.IsChecked == false && re.radioUniv.IsChecked == false)
            {
                re.Error.Content = "No frequency selected";
                return false;
            }
            if (re.radioBarrel.IsChecked == false && re.radioKG.IsChecked == false && re.radioScoop.IsChecked == false && re.radioT.IsChecked == false)
            {
                re.Error.Content = "No unit selected";
                return false;
            }
            if (re.priceBox.Text.Equals(""))
            {
                re.Error.Content = "Missing price";
                return false;
            }
            if (re.typeBox.SelectedItem == null)
            {
                re.Error.Content = "No type selected";
                return false;
            }
            if (re.Date.SelectedDate == null)
            {
                re.Error.Content = "No date selected";
                return false;
            }
            if (re.Date.SelectedDate > DateTime.Now)
            {
                re.Error.Content = "Invalid date";
                return false;
            }
            return true;
        }

        public Model.Type makeType()
        {
            if (contAddType())
            {
                Model.Type t = new Model.Type(te.IDbox.Text, te.nameBox.Text, te.descBox.Text, te.icoPath.Text);
                return t;
            }
            success = false;
            return null;
        }

        public bool contAddType()
        {
            if (te.IDbox.Text.Equals(""))
            {
                te.Error.Content = "Missing ID";
                return false;
            }
            if (te.nameBox.Text.Equals(""))
            {
                te.Error.Content = "Missing name";
                return false;
            }
            if (te.icoPath.Text.Equals(""))
            {
                te.Error.Content = "Missing icon";
                return false;
            }
            return true;
        }

        public Model.Etiquette makeTag()
        {
            if (contAddTag())
            {
                Model.Etiquette t = new Model.Etiquette(tage.IDbox.Text, tage.colorPicker.SelectedColor.ToString(), tage.descBox.Text);
                success = true;
                return t;
            }
            return null;
        }

        public bool contAddTag()
        {
            if (tage.IDbox.Text.Equals(""))
            {
                tage.Error.Content = "Missing ID";
                success = false;
                return false;
            }
            return true;
        }
    }
}
