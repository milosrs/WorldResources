using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Documents;
using System.Drawing;
using System.Drawing.Imaging;
using System.Windows.Media.Imaging;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Interop;
using System.Windows.Shapes;
using System.Windows.Forms;
using System.ComponentModel;
using WorldResources.Controler;
using System.Collections.ObjectModel;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;

namespace WorldResources.View
{
    /// <summary>
    /// Interaction logic for NewRes.xaml
    /// </summary>
    /// [System.Runtime.InteropServices.DllImport("gdi32.dll")]
    public partial class NewRes : Window, INotifyPropertyChanged
    {
        private Controler.AddRessControler resC;
        private OpenFileDialog fd;
        private System.Windows.Forms.DialogResult dr;
        private string iconPath = "";
        private GlowingEarth ge;

        public PicChanger pc { get; private set; }
        public ObservableCollection<Model.Etiquette> tagovi
        {
            get; set;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string name)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(name));
            }
        }

        public NewRes(GlowingEarth g)
        {
            pc = new PicChanger();
            InitializeComponent();
            ge = g;
            typeBox.DataContext = ge.getMaster().types;
            DataContext = this;
            tagovi = ge.getMaster().tags;
            if (ge.getMaster().types.Count > 0)
            {
                foreach (Model.Type t in ge.getMaster().types)
                {
                    typeBox.Items.Add(t);
                }
            }
            foreach (Model.Etiquette b in tagovi)
            {
                b.isPartOfRes = false;
            }
        }

        private void cancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void ok_Click(object sender, RoutedEventArgs e)
        {
            resC = new Controler.AddRessControler(this);
            if (resC.getSuccess())
            {
                System.Windows.MessageBox.Show("Resource added successfully!", "Success!", MessageBoxButton.OK);
                resetWindow();
                GlowingEarth.getInstance().getMaster().notifyChange();
            }
        }

        private void FileChooser_Click(object sender, RoutedEventArgs e)
        {
            fd = new OpenFileDialog();
            fd.DefaultExt = ".png";
            fd.Filter = "JPG files (*.jpg)|*.jpg|PNG files (*.png)|*.png|BMP files (*.bmp)|*.bmp|GIF files (*.gif)|*.gif|ICO files (*.ico)|*.ico";

            dr = fd.ShowDialog();
            if (dr == System.Windows.Forms.DialogResult.OK)
            {
                iconPath = fd.FileName;
                icoPath.Text = iconPath;
            }
        }
        public void resetWindow()
        {
            IDBox.Text = "";
            nameBox.Text = "";
            descBox.Text = "";
            priceBox.Text = "";
            typeBox.SelectedItem = null;
            radioBarrel.IsChecked = false;
            radioFreq.IsChecked = false;
            radioKG.IsChecked = false;
            radioRare.IsChecked = false;
            radioScoop.IsChecked = false;
            radioT.IsChecked = false;
            radioUniv.IsChecked = false;
            Strt.IsChecked = false;
            exp.IsChecked = false;
            ren.IsChecked = false;
            Date.SelectedDate = null;
            icoPath.Text = "";
            for (int i = 0; i < tagovi.Count; i++)       //setuj checkbox na false
            {
                tagovi[i].isPartOfRes = false;
            }

            Error.Content = "";
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            if (tagList.IsVisible)
            {
                tagList.Visibility = Visibility.Hidden;
            }
            else
            {
                tagList.Visibility = Visibility.Visible;
            }
        }


        private void nameBox_PreviewKeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            String txt = nameBox.Text;
            if (e.Key == Key.Space && (txt.Equals("") || txt[txt.Length - 1].Equals(' ')))
            {
                e.Handled = true;
            }
            else if ((Control.ModifierKeys == Keys.Shift) && char.IsDigit((char)KeyInterop.VirtualKeyFromKey(e.Key)))
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

        private void IDBox_PreviewKeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            String curText = IDBox.Text;
            bool found = false;
            if (e.Key == Key.Space)
            {
                e.Handled = true;
                return;
            }
            else if ((Control.ModifierKeys == Keys.Shift) && char.IsDigit((char)KeyInterop.VirtualKeyFromKey(e.Key)))
            {
                e.Handled = true;
                return;
            }
        }

        private void IDBox_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            String curText = IDBox.Text;
            bool found = false;
            foreach (Model.Resource r in ge.getMaster().resources)
            {
                if (r.getMark().Equals(curText))
                {
                    found = true;
                    Error.Content = "Resource with this ID already exists";
                    IDBox.BorderBrush = System.Windows.Media.Brushes.Red;
                    break;
                }
            }
            if (!found || curText.Equals(""))
            {
                BrushConverter bc = new BrushConverter();
                IDBox.BorderBrush = (System.Windows.Media.Brush)bc.ConvertFrom("#C7DFFC");
                if (Error.Content.Equals("Resource with this ID already exists") || Error.Content.Equals("Invalid ID character"))
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
                bool a = Regex.Match(p, @"^[0-9]*\.?$").Success;
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
                if(e.Text=="." && priceBox.Text.Contains("."))
                {
                    e.Handled = true;
                }
            }
            catch (Exception ex)
            {
                
            }
        }
        /*KRAJ VALIDACIJE PRICEBOXA*/
    }
}
