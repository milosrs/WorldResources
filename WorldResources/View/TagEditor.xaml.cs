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
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace WorldResources.View
{
    /// <summary>
    /// Interaction logic for TagEditor.xaml
    /// </summary>
    public partial class TagEditor : Window, INotifyPropertyChanged
    {
        public ObservableCollection<Model.Etiquette> tags
        {
            get; set;
        }
        private ObservableCollection<Model.Etiquette> copy;
        private Model.Etiquette _selectedTag;
        public Model.Etiquette selectedTag
        {
            get
            {
                return _selectedTag;
            }
            set
            {
                _selectedTag = value;
                OnPropertyChanged("selectedTag");
            }
        }
        private GlowingEarth refer = null;
        public TagEditor(GlowingEarth ge)
        {
            refer = ge;
            InitializeComponent();
            tags = ge.getMaster().tags;
            copy = new ObservableCollection<Model.Etiquette>();
            foreach(Model.Etiquette e in refer.getMaster().tags)
            {
                copy.Add(e);
            }
            ShowDialog();
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string name)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(name));
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            this.DataContext = this;
        }

        private void delete_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult mbr = System.Windows.MessageBox.Show("Are you sure?", "Confirm Deletion", MessageBoxButton.YesNo);
            if (mbr == MessageBoxResult.Yes)
            {
                Controler.DeleteControler dc = new Controler.DeleteControler(_selectedTag);
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
                System.Windows.MessageBox.Show("Resource modified successfully!", "Success!", MessageBoxButton.OK);
                GlowingEarth.getInstance().getMaster().notifyChange();
            }
            else
            {
                Error.Content = "Something went wrong :(";
            }
        }

        private void searchBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            _selectedTag = null;
            if (searchBox.Text.Equals(""))
            {
                tags.Clear();
                foreach (Model.Etiquette et in copy)
                {
                    tags.Add(et);
                }
                return;
            }
            if (tags != null)
            {
                ObservableCollection<Model.Etiquette> tagz = new ObservableCollection<Model.Etiquette>();
                foreach (Model.Etiquette et in refer.getMaster().tags)
                {
                    if (et.getID().Contains(searchBox.Text))
                    {
                        tagz.Add(et);
                    }
                }
                tags.Clear();
                foreach (Model.Etiquette et in tagz)
                {
                    tags.Add(et);
                }
            }
        }

        private void searchBox_GotFocus(object sender, RoutedEventArgs e)
        {
            if (searchBox.Text.Equals("Search for tags..."))
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
                searchBox.Text = "Search for tags...";
                restartTagz();
            }
        }

        public void restartTagz()
        {
            tags.Clear();
            foreach (Model.Etiquette e in copy)
            {
                tags.Add(e);
            }
        }
    }
}
