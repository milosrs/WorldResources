using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Forms;

namespace WorldResources.View
{
    public partial class NewTag : Window
    {
        private Controler.AddEtiqControler ec;
        private GlowingEarth ge;
        public NewTag(GlowingEarth ge)
        {
            this.ge = ge;
            InitializeComponent();
        }

        private void cancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void ok_Click(object sender, RoutedEventArgs e)
        {
            if (cont())
            {
                ec = new Controler.AddEtiqControler(this);
                if (ec.getSuccess())
                {
                    System.Windows.MessageBox.Show("Tag added successfully!", "Success!", MessageBoxButton.OK);
                    IDbox.Text = "";
                    descBox.Text = "";
                    Errl.Content = "";
                    GlowingEarth.getInstance().getMaster().notifyChange();
                }
            }
        }

        public bool cont()
        {
            if (IDbox.Text.Equals(""))
            {
                Errl.Content = "Missing ID";
                return false;
            }
            if (colorp.SelectedColor.Equals(null))
            {
                Errl.Content = "Color not selected";
                return false;
            }
            return true;
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
                IDbox.BorderBrush = System.Windows.Media.Brushes.Red;
                e.Handled = true;
                return;
            }
        }

        private void IDbox_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            String curText = IDbox.Text;
            bool found = false;

            foreach (Model.Etiquette r in GlowingEarth.getInstance().getMaster().tags)
            {
                if (r.getID().Equals(curText))
                {
                    found = true;
                    Error.Content = "Tag with this ID already exists";
                    IDbox.BorderBrush = System.Windows.Media.Brushes.Red;
                    break;
                }
            }
            if (!found || curText.Equals(""))
            {
                BrushConverter bc = new BrushConverter();
                IDbox.BorderBrush = (System.Windows.Media.Brush)bc.ConvertFrom("#C7DFFC");
                if (Error.Content.Equals("Tag with this ID already exists") || Error.Content.Equals("Invalid ID character"))
                {
                    Error.Content = "";
                }
            }
        }

        private void Help(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == Key.F1)
            {
                HelpViewer hv = new HelpViewer("ne");
            }
        }
    }
}
