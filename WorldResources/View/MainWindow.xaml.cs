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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace WorldResources
{
    public partial class MainWindow : Window
    {
        private User u;
        private readonly string path;
        private List<User> userList;
        private FileStream stream = null;
        private BinaryFormatter formater = new BinaryFormatter();
        public MainWindow()
        {
            path = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory,"usersList");
            InitializeComponent();
        }

        private void Login_Click(object sender, RoutedEventArgs e)
        {
            u = new User(UserBox.GetLineText(0), PwBox.Password);

            if (!Reg_success.Content.Equals(""))
            {
                Error.Content = "";
                Reg_success.Content = "";
            }
            if(UserBox.GetLineText(0).Length==0 && PwBox.Password.Length == 0)
            {
                Error.Content = "Login information missing";
            }
            else if (UserBox.GetLineText(0).Length == 0)
            {
                Error.Content = "Missing username";
            }

            else if (PwBox.Password.Length == 0)
            {
                Error.Content = "Missing Password";
            }
            else
            {
                if (existance(u))
                {
                    GlowingEarth ge = GlowingEarth.getInstance();
                    this.Close();
                    ge.Show();
                }
                else
                {
                    Error.Content = "Wrong username or password";
                }
            }
        }

        private void SignUp_Click(object sender, RoutedEventArgs e)
        {
            RegWindow reg = new RegWindow(this);
        }

        public void wohoo()
        {
            Reg_success.Content = "Signed up sucessfully! You may login";
            Error.Content = "";
        }

        public bool existance(User man)
        {
            bool success = false;
            try {
                stream = File.Open(path, FileMode.Open);
                userList = (List<User>)formater.Deserialize(stream);
                foreach(User r in userList)
                {
                    if (r.getName().Equals(u.getName()))
                    {
                        if (r.getPass().Equals(u.getPass()))
                        {
                            return true;   //Ako se prekine for u nekom trenutku, postoji korisnik
                        }
                        else
                        {
                            return false;   //Ako se prekine for u ovom trenutku, ne postoji korisnik
                        }
                    }
                }
                return false;               //Ako se ne prekine for u nekom trenutku, ne postoji korisnik
            }
            catch                           //Ako je izuzetak, onda ne postoji fajl
            {
                Error.Content = "User doesnt exist. Please register";
                success = false;
            }
            finally{
                if (stream != null)
                {
                    stream.Dispose();
                }
            }
            return success;
        }

        private void UserBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (Keyboard.IsKeyDown(Key.Enter))
            {
                Login_Click(sender, (RoutedEventArgs)e);
            }
        }

        private void PwBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (Keyboard.IsKeyDown(Key.Enter))
            {
                Login_Click(sender, (RoutedEventArgs)e);
            }
        }
    }
}
