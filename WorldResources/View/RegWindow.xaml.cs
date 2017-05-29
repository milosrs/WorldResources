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

namespace WorldResources
{
    /// <summary>
    /// Interaction logic for RegWindow.xaml
    /// </summary>
    public partial class RegWindow : Window
    {
        private bool succ = false;
        RegisterControl regCont;
        MainWindow reference;
        public RegWindow(MainWindow mw)
        {
            InitializeComponent();
            regCont = new RegisterControl(this);
            reference = mw;             //Zbog promene labele
            this.ShowDialog();
        }

        private void RegButton_Click(object sender, RoutedEventArgs e)
        {
            if(UserBox.GetLineText(0).Length == 0 && PWBox.Password.Length ==0)
            {
                ErrReg.Content = "Missing information";
            }
            else if(UserBox.GetLineText(0).Length == 0)
            {
                ErrReg.Content = "Missing username";
            }
            else if(PWBox.Password.Length == 0)
            {
                ErrReg.Content = "Missing password";
            }
            else if(PWBox.Password.Length < 6)
            {
                ErrReg.Content = "Invalid password";
            }
            else
            {
                regCont.openData();
                if (regCont.getSuccess())
                {
                    reference.wohoo();
                    this.Close();
                }
            }
        }

        public string getUser()
        {
            return UserBox.GetLineText(0);
        }

        public string getPass()
        {
            return PWBox.Password;
        }

        public void userExists()
        {
            ErrReg.Content = "User already exists";
        }

        public bool getSuccess()
        {
            return succ;
        }

        private void UserBox_KeyDown(object sender, KeyEventArgs e)
        {
            if(Keyboard.IsKeyDown(Key.Enter))
                RegButton_Click(sender, (RoutedEventArgs)e);
        }

        private void PWBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (Keyboard.IsKeyDown(Key.Enter))
                RegButton_Click(sender, (RoutedEventArgs)e);
        }
    }
}
