using System;
using System.Collections.Generic;
using System.IO;
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
using System.Windows.Resources;
using System.Windows.Shapes;

namespace WorldResources.View
{
    /// <summary>
    /// Interaction logic for HelpViewer.xaml
    /// </summary>
    public partial class HelpViewer : Window
    {
        public HelpViewer(string window)
        {
            StreamResourceInfo  info;
            InitializeComponent();
            if (window.Equals("main"))
            {
                var outPutDirectory = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().CodeBase);
                var helpPath = System.IO.Path.Combine(outPutDirectory, "r\\Help\\Using Main Window.htm");
                string help_path = new Uri(helpPath).LocalPath;
                System.Diagnostics.Process.Start(help_path);
            }
            else if (window.Equals("nr"))
            {
                var outPutDirectory = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().CodeBase);
                var helpPath = System.IO.Path.Combine(outPutDirectory, "r\\Help\\Adding a New Resource.htm");
                string help_path = new Uri(helpPath).LocalPath;
                System.Diagnostics.Process.Start(help_path);
            }
            else if (window.Equals("nt"))
            {
                var outPutDirectory = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().CodeBase);
                var helpPath = System.IO.Path.Combine(outPutDirectory, "r\\Help\\Adding a New Type.htm");
                string help_path = new Uri(helpPath).LocalPath;
                System.Diagnostics.Process.Start(help_path);
            }
            else if (window.Equals("ne"))
            {
                var outPutDirectory = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().CodeBase);
                var helpPath = System.IO.Path.Combine(outPutDirectory, "r\\Help\\Adding a New Tag.htm");
                string help_path = new Uri(helpPath).LocalPath;
                System.Diagnostics.Process.Start(help_path);
            }
            else if (window.Equals("er"))
            {
                var outPutDirectory = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().CodeBase);
                var helpPath = System.IO.Path.Combine(outPutDirectory, "r\\Help\\Modifying a Resource.htm");
                string help_path = new Uri(helpPath).LocalPath;
                System.Diagnostics.Process.Start(help_path);
            }
            else if (window.Equals("et"))
            {
                var outPutDirectory = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().CodeBase);
                var helpPath = System.IO.Path.Combine(outPutDirectory, "r\\Help\\Modifying a Type.htm");
                string help_path = new Uri(helpPath).LocalPath;
                System.Diagnostics.Process.Start(help_path);
            }
            else if (window.Equals("ee"))
            {
                var outPutDirectory = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().CodeBase);
                var helpPath = System.IO.Path.Combine(outPutDirectory, "r\\Help\\Modifying a Tag.htm");
                string help_path = new Uri(helpPath).LocalPath;
                System.Diagnostics.Process.Start(help_path);
            }
            else
            {
                var outPutDirectory = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().CodeBase);
                var helpPath = System.IO.Path.Combine(outPutDirectory, "r\\Help\\introduction.htm");
                string help_path = new Uri(helpPath).LocalPath;
                System.Diagnostics.Process.Start(help_path);
            }
            //wbHelp.NavigateToStream();

        }
    }
}
