using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace WorldResources.View
{
    /// <summary>
    /// Interaction logic for ResEditor.xaml
    /// </summary>
    public partial class ResEditor : Window, INotifyPropertyChanged
    {
        public ObservableCollection<Model.Resource> res { get; set; }
        public ObservableCollection<Model.Type> types { get; set; }
        public ObservableCollection<Model.Etiquette> tags
        {
            get; set;
        }

        private GlowingEarth refer=null;
        private Model.Resource _selectedResource;
        private Model.Type filt;
        public Model.Resource selectedResource
        {
            get { return _selectedResource; }
            set
            {
                _selectedResource = value;
                OnPropertyChanged("selectedResource");
            }
        }
        private string _picpath;
        public string picpath
        {
            get
            {
                return _picpath;
            }
            set
            {
                _picpath = value;
                OnPropertyChanged("picpath");
            }
        }
        private ObservableCollection<Model.Resource> copy;
        public ResEditor(GlowingEarth ge)
        {
            refer = ge;
            InitializeComponent();
            res = new ObservableCollection<Model.Resource>();
            types = ge.getMaster().types;
            tags = ge.getMaster().tags;

            filter.Items.Add(null);
            if (types.Count > 0)
            {
                foreach(Model.Type t in types)
                {
                    typeBox.Items.Add(t);
                    filter.Items.Add(t);
                }
                
            }
            copy = new ObservableCollection<Model.Resource>();
            foreach(Model.Resource r in ge.getMaster().resources)
            {
                copy.Add(r);
                res.Add(r);
            }
            ShowDialog();
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string name)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(name));
            }
        }

        private void DataGrid_Loaded(object sender, RoutedEventArgs e)
        {
            this.DataContext = this;
        }

        private void DataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Error.Content = "";
            if (_selectedResource != null)
            {
                this.modify.IsEnabled = true;
                this.delete.IsEnabled = true;
                if (_selectedResource.getFreq() == Model.Resource.FreqType.RARE)
                {
                    radioRare.IsChecked = true;
                    radioUniv.IsChecked = false;
                    radioFreq.IsChecked = false;
                }
                else if (_selectedResource.getFreq() == Model.Resource.FreqType.FREQUENT)
                {
                    radioRare.IsChecked = false;
                    radioUniv.IsChecked = false;
                    radioFreq.IsChecked = true;
                }
                else if (_selectedResource.getFreq() == Model.Resource.FreqType.UNIVERSAL)
                {
                    radioRare.IsChecked = false;
                    radioUniv.IsChecked = true;
                    radioFreq.IsChecked = false;
                }
                if (_selectedResource.getUnit() == Model.Resource.Units.BAREL)
                {
                    radioBarrel.IsChecked = true;
                    radioKG.IsChecked = false;
                    radioScoop.IsChecked = false;
                    radioT.IsChecked = false;
                }
                if (_selectedResource.getUnit() == Model.Resource.Units.KG)
                {
                    radioBarrel.IsChecked = false;
                    radioKG.IsChecked = true;
                    radioScoop.IsChecked = false;
                    radioT.IsChecked = false;
                }
                else if (_selectedResource.getUnit() == Model.Resource.Units.SCOOP)
                {
                    radioBarrel.IsChecked = false;
                    radioKG.IsChecked = false;
                    radioScoop.IsChecked = true;
                    radioT.IsChecked = false;
                }
                else if (_selectedResource.getUnit() == Model.Resource.Units.T)
                {
                    radioBarrel.IsChecked = false;
                    radioKG.IsChecked = false;
                    radioScoop.IsChecked = false;
                    radioT.IsChecked = true;
                }
                typeBox.SelectedItem = _selectedResource.getType();
                picpath = _selectedResource.getIco();
                
            }
            else
            {
                this.modify.IsEnabled = false;
                this.delete.IsEnabled = false;
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog fd = new OpenFileDialog();
            fd.DefaultExt = ".png";
            fd.Filter = "JPG files (*.jpg)|*.jpg|PNG files (*.png)|*.png|BMP files (*.bmp)|*.bmp|GIF files (*.gif)|*.gif|ICO files (*.ico)|*.ico";

            DialogResult dr = fd.ShowDialog();
            if (dr == System.Windows.Forms.DialogResult.OK)
            {
                picpath = fd.FileName;
            }
        }

        private void delete_Click(object sender, RoutedEventArgs e)
        {
            Error.Content = "";
            MessageBoxResult mbr = System.Windows.MessageBox.Show("Are you sure?", "Confirm Deletion", MessageBoxButton.YesNo);
            if (mbr == MessageBoxResult.Yes)
            {
                copy.Remove(selectedResource);
                Controler.DeleteControler dc = new Controler.DeleteControler(_selectedResource);
                res.Remove(selectedResource);
                GlowingEarth.getInstance().getMaster().notifyChange();
            }
        }

        private void cancel_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void modify_Click(object sender, RoutedEventArgs e)
        {
            Controler.ModifyControler mc = new Controler.ModifyControler(this);
            if (mc.getSucc())
            {
                for(int i=0; i<copy.Count; i++)
                {
                    if (copy[i].getMark().Equals(mc.getRes().getMark()))
                    {
                        copy[i] = mc.getRes();
                        res[i] = mc.getRes();
                        break;
                    }
                }
                System.Windows.MessageBox.Show("Resource modified successfully!", "Success!", MessageBoxButton.OK);
                Error.Content ="";
            }
            GlowingEarth.getInstance().getMaster().notifyChange();
        }
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            modify.IsEnabled = false;
            delete.IsEnabled = false;
            _selectedResource = null;
        }

        private void nameBox_PreviewKeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            String txt = nameBox.Text;
            if (e.Key == Key.Space && (txt.Equals("") || txt[txt.Length - 1].Equals(' ')))
            {
                e.Handled = true;
            }
            else if ((System.Windows.Forms.Control.ModifierKeys == Keys.Shift) && char.IsDigit((char)KeyInterop.VirtualKeyFromKey(e.Key)))
            {
                Error.Content = "Invalid name character";
                nameBox.BorderBrush = System.Windows.Media.Brushes.Red;
                e.Handled = true;
            }
            else
            {
                BrushConverter bc = new BrushConverter();
                nameBox.BorderBrush = (System.Windows.Media.Brush)bc.ConvertFrom("#C7DFFC");
                if (Error.Content.Equals("Invalid name character"))
                {
                    Error.Content = "";
                }
            }
        }

        private void Date_CalendarClosed(object sender, RoutedEventArgs e)
        {
            if (Date.SelectedDate > DateTime.Now)
            {
                Error.Content = "Invalid date";
            }
            else
            {
                if (Error.Content.Equals("Invalid date"))
                {
                    Error.Content = "";
                }
            }
        }

        /*ODAVDE JE VALIDACIJA PRICEBOXA*/
        private bool IsValidTextNumber(string p)
        {
            try
            {
                bool a = System.Text.RegularExpressions.Regex.Match(p, @"^[0-9]*\.?$").Success;
                return a;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        private void priceBox_PreviewKeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            try
            {
                if (e.Key == Key.Space)
                {
                    e.Handled = true;
                }
            }
            catch (Exception ex)
            {

            }
        }

        private void priceBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            try
            {
                e.Handled = !IsValidTextNumber(e.Text);
                if (e.Text == "." && priceBox.Text.Contains("."))
                {
                    e.Handled = true;
                }
            }
            catch (Exception ex)
            {

            }
        }
        /*KRAJ VALIDACIJE PRICEBOXA*/

        private void typeBox_Copy_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ObservableCollection<Model.Resource> resez = new ObservableCollection<Model.Resource>();
            _selectedResource = null;
            picpath = "";
            filt = (Model.Type)filter.SelectedItem;
            if (filt != null && (searchBox.Text.Equals("") || searchBox.Text.Equals("Search for resources...")))
            {
                
                foreach (Model.Resource r in GlowingEarth.getInstance().getMaster().getResources())
                {
                    if (r.getType().getMark().Equals(filt.getMark()))
                    {
                        resez.Add(r);
                    }
                }
                res.Clear();
                foreach (Model.Resource r in resez)
                {
                    res.Add(r);
                }
            }
            else if(filt != null && !(searchBox.Text.Equals("") || searchBox.Text.Equals("Search for resources...")))
            {
                foreach (Model.Resource r in GlowingEarth.getInstance().getMaster().getResources())
                {
                    if (r.getType().getMark().Equals(filt.getMark()) && r.getMark().Equals(searchBox.Text))
                    {
                        resez.Add(r);
                    }
                }
                res.Clear();
                foreach (Model.Resource r in resez)
                {
                    res.Add(r);
                }
            }
            else
            {
                restartResez();
            }
        }

        private void searchBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            ObservableCollection<Model.Resource> resez = new ObservableCollection<Model.Resource>();
            _selectedResource = null;
            picpath = "";
            if ((searchBox.Text.Equals("")||searchBox.Text.Equals("Search for resources...")) && filt==null && res!=null && copy!=null)
            {
                foreach (Model.Resource r in copy)
                {
                    res.Add(r);
                }
                return;
            }
            else if (filt != null && (searchBox.Text.Equals("") || searchBox.Text.Equals("Search for resources...")))
            {
                
                foreach (Model.Resource r in GlowingEarth.getInstance().getMaster().getResources())
                {
                    if (r.getMark().Contains(searchBox.Text) && r.getType().getMark().Equals(filt.getMark()))
                    {
                        resez.Add(r);
                    }
                }
                res.Clear();
                foreach (Model.Resource re in resez)
                {
                    res.Add(re);
                }
            }
            else if(filt==null && !(searchBox.Text.Equals("") || searchBox.Text.Equals("Search for resources...")))
            {
                foreach (Model.Resource r in GlowingEarth.getInstance().getMaster().getResources())
                {
                    if (r.getMark().Contains(searchBox.Text))
                    {
                        resez.Add(r);
                    }
                }
                res.Clear();
                foreach (Model.Resource re in resez)
                {
                    res.Add(re);
                }
            }
            else if (filt == null && (searchBox.Text.Equals("") || searchBox.Text.Equals("Search for resources...")))
            {
                restartResez();
            }
            else
            {
                foreach (Model.Resource r in GlowingEarth.getInstance().getMaster().getResources())
                {
                    if (r.getMark().Contains(searchBox.Text) && r.getType().getMark().Equals(filt.getMark()))
                    {
                        resez.Add(r);
                    }
                }
                res.Clear();
                foreach (Model.Resource re in resez)
                {
                    res.Add(re);
                }
            }
        }

        private void searchBox_GotFocus(object sender, RoutedEventArgs e)
        {
            if (searchBox.Text.Equals("Search for resources..."))
            {
                searchBox.Text = "";
            }
            searchBox.Foreground = Brushes.Black;
        }

        private void searchBox_LostFocus(object sender, RoutedEventArgs e)
        {
            searchBox.Foreground = Brushes.Gray;
            if (searchBox.Text.Equals(""))
            {
                searchBox.Text = "Search for resources...";
                restartResez();
            }
        }

        public void restartResez()
        {
            if (res != null)
            {
                res.Clear();
                foreach (Model.Resource r in copy)
                {
                    res.Add(r);
                }
            }
            else
            {
                res = new ObservableCollection<Model.Resource>();
            }
        }

        private void reset_Click(object sender, RoutedEventArgs e)
        {
            selectedResource = null;
            picpath = "";
            filt = null;
            searchBox.Text = "Search for resources...";
            filter.SelectedItem = null;
        }

        private void Help(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == Key.F1)
            {
                HelpViewer hv = new HelpViewer("er");
            }
        }
    }
}
