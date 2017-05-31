using System;
using System.Collections.Generic;
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
    /// Interaction logic for NewProjectDialog.xaml
    /// </summary>
    public partial class NewProjectDialog : Window
    {
        public NewProjectDialog()
        {
            InitializeComponent();
        }

        private void createBtn_Click(object sender, RoutedEventArgs e)
        {
            if (canMake())
            {
                Controler.NewProjectControler npc = new Controler.NewProjectControler(nameBox.Text);
            }
            else
            {
                error.Content = "Invalid project name";
            }
            this.Close();
        }

        private void cancelBtn_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        public bool canMake()
        {
            if (nameBox.Text.Length == 0)
            {
                return false;
            }
            return true;
        }

        private void nameBox_PreviewKeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            String txt = nameBox.Text;
            if (e.Key == Key.Space && (txt.Equals("") || txt[txt.Length - 1].Equals(' ')))
            {
                e.Handled = true;
            }
            if (e.Key == Key.Enter)
            {
                if (canMake())
                {
                    Controler.NewProjectControler npc = new Controler.NewProjectControler(nameBox.Text);
                }
                else
                {
                    error.Content = "Invalid project name";
                }
                this.Close();
                return;
            }
            else if ((System.Windows.Forms.Control.ModifierKeys == System.Windows.Forms.Keys.Shift) && char.IsDigit((char)KeyInterop.VirtualKeyFromKey(e.Key)))
            {
                error.Content = "Invalid name character";
                nameBox.BorderBrush = System.Windows.Media.Brushes.Red;
                e.Handled = true;
            }
            else
            {
                BrushConverter bc = new BrushConverter();
                nameBox.BorderBrush = (System.Windows.Media.Brush)bc.ConvertFrom("#C7DFFC");
                if (error.Content.Equals("Invalid name character"))
                {
                    error.Content = "";
                }
            }
        }
    }
}
