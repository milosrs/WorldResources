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
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Collections.ObjectModel;
using WorldResources.View;
using System.ComponentModel;
using WorldResources.Controler;
using System.Collections.Specialized;

namespace WorldResources
{
    /// <summary>
    /// Interaction logic for GlowingEarth.xaml
    /// </summary>
    /// 
    [Serializable]
    public partial class GlowingEarth : Window, INotifyPropertyChanged
    {

        private Model.MapItem _dragItem;                            //Sta se vuce - referenca
        private Model.MapItem mi;                                   //Ono sto se vuce sa mape - zaseban objekat kopija
        private Point startPoint = new Point();                     //Klik tacka
        private bool move;                                          //Da li se ono na mapi pomera ili ne
        private Model.MasterClass _mc;
        /*--------------------------Binding-----------------------------*/
        public Model.MapItem dragItem
        {
            get
            {
                return _dragItem;
            }
            set
            {
                _dragItem = value;
                OnPropertyChanged("dragItem");
            }
        }
        public Model.MasterClass mc
        {
            get
            {
                return _mc;
            }
            set
            {
                _mc = value;
                OnPropertyChanged("mc");
            }
        }
        /*----------------------------Save--------------------------*/
        private BinaryFormatter fm;
        private FileStream sm = null;
        private string path;

        /*----------------------------INotify------------------------*/
        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string name)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(name));
            }
        }
        /*----------------------------Singleton------------------------*/
        private static GlowingEarth ge;
        public static GlowingEarth getInstance()
        {
            if (ge == null)
            {
                ge = new GlowingEarth();
            }
            return ge;
        }

        /*----------------------------Konstruktor------------------------*/
        private GlowingEarth()
        {
            InitializeComponent();
            mc = new Model.MasterClass();
            _mc = new Model.MasterClass();
        }

        private void About_Click(object sender, RoutedEventArgs e)
        {
            View.About ab = new View.About();
            ab.ShowDialog();
        }

        private void Add_Click(object sender, RoutedEventArgs e)
        {
            View.NewRes nr = new View.NewRes(this);
            nr.ShowDialog();
        }

        private void Type_Click(object sender, RoutedEventArgs e)
        {
            View.NewType nt = new View.NewType(this);
            nt.ShowDialog();
        }

        private void Etiq_Click(object sender, RoutedEventArgs e)
        {
            View.NewTag nta = new View.NewTag(this);
            nta.ShowDialog();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            refresh();
        }
        public void refresh()
        {
            ThisWasLastController twl = new ThisWasLastController("load");
            path = twl.GetFromHere();

            BinaryFormatter fm = new BinaryFormatter();
            FileStream sm = null;
            try
            {
                sm = File.OpenRead(path);
                Model.MasterClass x = (Model.MasterClass)fm.Deserialize(sm);
                GlowingEarth.getInstance().getMaster().setTags(x.getTags());
                GlowingEarth.getInstance().getMaster().setTypes(x.getTypes());
                GlowingEarth.getInstance().getMaster().setResources(x.getResources());
                GlowingEarth.getInstance().getMaster().setTitle(x.getTitle());
                GlowingEarth.getInstance().getMaster().setSerPath(x.getSerPath());
                GlowingEarth.getInstance().getMaster().setMapItems(x.getMapItems());
            }
            catch (Exception e)
            {
                NewProjectDialog npd = new NewProjectDialog();
                npd.ShowDialog();
                twl.save();
            }
            finally
            {
                if (sm != null)
                {
                    sm.Dispose();
                    sm.Close();
                }
            }
            this.DataContext = this;
        }

        public Model.MasterClass getMaster()
        {
            return _mc;
        }

        public void setMaster(Model.MasterClass mcc)
        {
            _mc = mcc;
            mc = _mc;
        }

        private void ResEditor_Click(object sender, RoutedEventArgs e)
        {
            ResEditor re = new ResEditor(this);
            foreach (Model.Etiquette et in mc.getTags())
            {
                et.isPartOfRes = false;
            }
        }

        private void TypEditor_Click(object sender, RoutedEventArgs e)
        {
            TypeEditor type = new TypeEditor(this);
            //resetCanvas();
        }

        private void TagEditor_Click(object sender, RoutedEventArgs e)
        {
            TagEditor te = new View.TagEditor(this);
        }

        private void itemList_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            startPoint = e.GetPosition(null);
        }

        private void itemList_MouseMove(object sender, MouseEventArgs e)
        {
            Point mousePos = e.GetPosition(null);
            Vector diff = startPoint - mousePos;

            if (e.LeftButton == MouseButtonState.Pressed &&
                (Math.Abs(diff.X) > SystemParameters.MinimumHorizontalDragDistance ||
                Math.Abs(diff.Y) > SystemParameters.MinimumVerticalDragDistance))
            {
                ListView listView = sender as ListView;
                ListViewItem listViewItem =
                    FindAncestor<ListViewItem>((DependencyObject)e.OriginalSource);

                Model.Resource res = (Model.Resource)listView.ItemContainerGenerator.
                    ItemFromContainer(listViewItem);

                dragItem = new Model.MapItem(res.getMark(), res.getName(), res.getIco());

                DataObject dragData = new DataObject("myFormat", dragItem);
                DragDrop.DoDragDrop(listViewItem, dragData, DragDropEffects.Move);
            }
        }

        private static T FindAncestor<T>(DependencyObject current) where T : DependencyObject
        {
            do
            {
                if (current is T)
                {
                    return (T)current;
                }
                current = VisualTreeHelper.GetParent(current);
            }
            while (current != null);
            return null;
        }

        private void can_DragEnter(object sender, DragEventArgs e)
        {
            if (!e.Data.GetDataPresent("myFormat") || sender == e.Source)
            {
                e.Effects = DragDropEffects.None;
            }
        }

        private void can_Drop(object sender, DragEventArgs e)
        {
            Point p = new Point(e.GetPosition(map).X - 45, e.GetPosition(map).Y - 50);
            if (p.X < 0)
            {
                p.X = 0;
            }
            if (p.Y < 0)
            {
                p.Y = 0;
            }

            if (SearchPosition(e.GetPosition(map)) == null && !move)
            {
                dragItem.setPosition(p);
                mc.resOnCanvas.Add(dragItem);
            }
            else if (SearchPosition(p) != null && move)
            {
                dragItem.setPosition(mi.getPosition());
                move = false;
            }
            else if (SearchPosition(p) == null && move)
            {
                mc.resOnCanvas.Remove(dragItem);
                dragItem.setPosition(p);
                mc.resOnCanvas.Add(dragItem);
            }
            Console.WriteLine("Item spusten na X:" + p.X + " Y:" + p.Y);
            move = false;
            GlowingEarth.getInstance().getMaster().notifyChange();
        }

        private void can_DragOver(object sender, DragEventArgs e)
        {
            Console.WriteLine("X:" + e.GetPosition(map).X + " Y:" + e.GetPosition(map).Y);
            if (SearchPosition(e.GetPosition(map)) != null)
            {
                e.Effects = DragDropEffects.None;
            }
        }

        private void map_MouseMove(object sender, MouseEventArgs e)
        {
            //Console.WriteLine("X:" + e.GetPosition(map).X + " Y:" + e.GetPosition(map).Y);
            Point mousePos = e.GetPosition(map);
            if (e.LeftButton == MouseButtonState.Pressed &&
                (Math.Abs(mousePos.X) > SystemParameters.MinimumHorizontalDragDistance ||
                 Math.Abs(mousePos.Y) > SystemParameters.MinimumVerticalDragDistance))
            {
                Console.WriteLine("WASAAAP");
                ItemsControl itemsControl = sender as ItemsControl;

                if (dragItem != null)
                {
                    DataObject dragData = new DataObject("myFormat", dragItem);
                    move = true;
                    DragDrop.DoDragDrop(itemsControl, dragData, DragDropEffects.Move);
                }
            }
        }

        private Model.MapItem SearchPosition(Point point)
        {
            Model.MapItem ret = null;
            foreach (Model.MapItem m in mc.resOnCanvas)
            {
                bool pointIsLefter = false;
                bool pointIsUpper = false;
                if (point.X < m.getPosition().X)
                {
                    pointIsLefter = true;
                }
                if (point.X < m.getPosition().Y)
                {
                    pointIsUpper = true;
                }
                if (point.X == m.getPosition().X && point.Y == m.getPosition().Y && dragItem.name == m.name)
                {
                    continue;
                }
                if (!move)
                {
                    if (pointIsLefter || pointIsUpper)
                    {
                        continue;
                    }
                    else
                    {
                        if (Math.Abs(point.X - m.position.X) <= 100 && Math.Abs(point.Y - m.position.Y) <= 90)
                        {
                            ret = m;
                        }
                    }
                }
                else
                {
                    if (Math.Abs(point.X - m.position.X) <= 100 && Math.Abs(point.Y - m.position.Y) <= 100)
                    {
                        ret = m;
                    }
                }
            }
            return ret;
        }

        private void map_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            dragItem = SearchPosition(e.GetPosition(map));
            if (dragItem != null)
            {
                mi = new Model.MapItem(dragItem.getID(), dragItem.getName(), dragItem.getPath());
                mi.setPosition(dragItem.getPosition());
                move = true;
            }
            else
            {
                move = false;
            }
        }

        private void map_PreviewMouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            Model.MapItem m = SearchPosition(e.GetPosition(map));
            mc.resOnCanvas.Remove(m);
        }

        private void Viewbox_MouseMove(object sender, MouseEventArgs e)
        {
            Console.WriteLine("X:" + e.GetPosition(map).X + " Y:" + e.GetPosition(map).Y);
        }

        /*-------------------Akcije-------------------*/
        private void NewFile_Click(object sender, RoutedEventArgs e)
        {
            NewProjectDialog npd = new NewProjectDialog();
            npd.ShowDialog();
        }

        private void LoadProject_Click(object sender, RoutedEventArgs e)
        {
            LoadProjectControler lpc = new LoadProjectControler();
            setMaster(mc);
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            SaveProjectControler spc = new SaveProjectControler();
        }

        private void SaveAs_Click(object sender, RoutedEventArgs e)
        {
            SaveProjectAsControler spac = new SaveProjectAsControler();
        }

        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            if(MessageBox.Show("This will discard everything in your project. Are you sure you want to continue?", "Warning", MessageBoxButton.YesNoCancel, MessageBoxImage.Warning) == MessageBoxResult.Yes)
            {
                DeleteProjectControler dpc = new DeleteProjectControler();
            }
        }
        private void Exit_Executed(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Help_Click(object sender, RoutedEventArgs e)
        {
            var outPutDirectory = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().CodeBase);
            var helpPath = System.IO.Path.Combine(outPutDirectory, "r\\Help\\Glowing Earth Help.chm");
            string path = String.Format(@"..\r\Help\Glowing Earth Help.chm");
            string help_path = new Uri(helpPath).LocalPath;
            System.Diagnostics.Process.Start(help_path);
        }

        private void ContextHelp_Click(object sender, RoutedEventArgs e)
        {
            View.HelpViewer hw = new HelpViewer("main");
        }
    }
}
