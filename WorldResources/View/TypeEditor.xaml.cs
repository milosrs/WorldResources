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
    public partial class TypeEditor : Window, INotifyPropertyChanged
    {
        public ObservableCollection<Model.Type> tyx { get; set; }
        private ObservableCollection<Model.Type> copy;
        private Model.Type _selType;
        public Model.Type selType
        {
            get
            {
                return _selType;
            }
            set
            {
                _selType = value;
                OnPropertyChanged("selType");
            }
        }
        private GlowingEarth refer = null;
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
        public TypeEditor(GlowingEarth ge)
        {
            refer = ge;
            copy = new ObservableCollection<Model.Type>();
            foreach(Model.Type t in refer.getMaster().types)
            {
                copy.Add(t);
            }
            InitializeComponent();
            tyx = ge.getMaster().types;
            ShowDialog();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            this.DataContext = this;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string name)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(name));
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
                System.Windows.MessageBox.Show("Type modified successfully!", "Success!", MessageBoxButton.OK);
                Error.Content = "";
                GlowingEarth.getInstance().getMaster().notifyChange();
            }
            else
            {
                Error.Content = "Something went wrong :(";
            }
        }

        private void delete_Click(object sender, RoutedEventArgs e)
        {
            Error.Content = "";
            MessageBoxResult mbr = System.Windows.MessageBox.Show("Are you sure you want to delete this type? This could lead to deletion of resources that contain this type.", "Confirm Deletion", MessageBoxButton.YesNo);
            if (mbr == MessageBoxResult.Yes)
            {
                Controler.DeleteControler dec = new Controler.DeleteControler(_selType);
                GlowingEarth.getInstance().getMaster().notifyChange();
            }
        }

        private void browse_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Forms.OpenFileDialog fd = new System.Windows.Forms.OpenFileDialog();
            fd.DefaultExt = ".png";
            fd.Filter = "JPG files (*.jpg)|*.jpg|PNG files (*.png)|*.png|BMP files (*.bmp)|*.bmp|GIF files (*.gif)|*.gif|ICO files (*.ico)|*.ico";

            DialogResult dr = fd.ShowDialog();
            if (dr == System.Windows.Forms.DialogResult.OK)
            {
                picpath = fd.FileName;
            }
        }

        private void DataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if(_selType == null)
            {
                modify.IsEnabled = false;
                delete.IsEnabled = false;
            }
            else
            {
               
                this.modify.IsEnabled = true;
                this.delete.IsEnabled = true;
                picpath = selType.getImg();
                return;              
            }
        }

        private void textBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            _selType = null;
            picpath = "";
            if (searchBox.Text.Equals(""))
            {
                tyx.Clear();
                foreach (Model.Type r in copy)
                {
                    tyx.Add(r);
                }
                return;
            }
            if (tyx != null)
            {
                ObservableCollection<Model.Type> typez = new ObservableCollection<Model.Type>();
                foreach (Model.Type t in refer.getMaster().types)
                {
                    if (t.getMark().Contains(searchBox.Text))
                    {
                        typez.Add(t);
                    }
                }
                tyx.Clear();
                foreach (Model.Type t in typez)
                {
                    tyx.Add(t);
                }
            }
        }

        private void textBox_GotFocus(object sender, RoutedEventArgs e)
        {
            if (searchBox.Text.Equals("Search for types..."))
            {
                searchBox.Text = "";
            }
            searchBox.Foreground = Brushes.Black;
        }

        private void textBox_LostFocus(object sender, RoutedEventArgs e)
        {
            searchBox.Foreground = Brushes.Gray;
            if (searchBox.Text.Equals(""))
            {
                searchBox.Text = "Search for resources...";
                restartTypez();
            }
        }

        public void restartTypez()
        {
            tyx.Clear();
            foreach (Model.Type t in copy)
            {
                tyx.Add(t);
            }
        }
    }
}
