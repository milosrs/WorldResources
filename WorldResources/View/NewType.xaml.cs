using System;
using System.Collections.Generic;
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
using System.Windows.Forms;

namespace WorldResources.View
{
    /// <summary>
    /// Interaction logic for NewType.xaml
    /// </summary>
    public partial class NewType : Window
    {
        private OpenFileDialog fd;
        private System.Windows.Forms.DialogResult dr;
        private string iconPath = "";
        private GlowingEarth ge;
        public Controler.PicChanger pc { get; private set; }
        public NewType(GlowingEarth ge)
        {
            this.ge = ge;
            InitializeComponent();
            this.DataContext = this;
            pc = new Controler.PicChanger();
        }

        private void FileChooser_Click(object sender, RoutedEventArgs e)
        {
            fd = new OpenFileDialog();
            fd.DefaultExt = ".jpg";
            fd.Filter = "JPG files (*.jpg)|*.jpg|PNG files (*.png)|*.png|BMP files (*.bmp)|*.bmp|GIF files (*.gif)|*.gif|ICO files (*.ico)|*.ico";

            dr = fd.ShowDialog();
            if (dr == System.Windows.Forms.DialogResult.OK)
            {
                iconPath = fd.FileName;
                icoPath.Text = iconPath;
            }
        }

        private void cancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void ok_Click(object sender, RoutedEventArgs e)
        {
            if (cont())
            {
                Controler.AddTypeControler acc = new Controler.AddTypeControler(this);
                if (acc.getSuccess())
                {
                    System.Windows.MessageBox.Show("Type added successfully!", "Success!", MessageBoxButton.OK);
                    IDbox.Text = "";
                    nameBox.Text = "";
                    descBox.Text = "";
                    icoPath.Text = "";
                    Error.Content = "";
                    GlowingEarth.getInstance().getMaster().notifyChange();
                }
            }
        }
        public bool cont()
        {
            if (IDbox.Text.Equals(""))
            {
                Error.Content = "Missing ID";
                return false;
            }
            if (nameBox.Text.Equals(""))
            {
                Error.Content = "Missing Name";
                return false;
            }
            if (icoPath.Text.Equals(""))
            {
                Error.Content = "Missing Icon";
                return false;
            }
            return true;
        }

        private void nameBox_PreviewKeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            String txt = nameBox.Text;
            if (e.Key == Key.Space && (txt.Equals("") || txt[txt.Length-1].Equals(' ')))
            {
                e.Handled = true;
            }
            else if ((System.Windows.Forms.Control.ModifierKeys == Keys.Shift) && char.IsDigit((char)KeyInterop.VirtualKeyFromKey(e.Key)))
            {
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

        private void IDbox_PreviewKeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == Key.Space)
            {
                e.Handled = true;
                return;
            }
            else if ((e.Key == Key.LeftShift || e.Key == Key.RightShift) && char.IsDigit((char)KeyInterop.VirtualKeyFromKey(e.Key)))
            {
                e.Handled = true;
                return;
            }
        }

        private void IDbox_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            String curText = IDbox.Text;
            bool found = false;

            foreach (Model.Type r in GlowingEarth.getInstance().getMaster().types)
            {
                if (r.getMark().Equals(curText))
                {
                    found = true;
                    Error.Content = "Type with this ID already exists";
                    IDbox.BorderBrush = System.Windows.Media.Brushes.Red;
                    break;
                }
            }
            if (!found || curText.Equals(""))
            {
                BrushConverter bc = new BrushConverter();
                IDbox.BorderBrush = (System.Windows.Media.Brush)bc.ConvertFrom("#C7DFFC");
                if (Error.Content.Equals("Type with this ID already exists") || Error.Content.Equals("Invalid ID character"))
                {
                    Error.Content = "";
                }
            }
        }
    }
}
