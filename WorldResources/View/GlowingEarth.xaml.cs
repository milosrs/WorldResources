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

namespace WorldResources
{
    /// <summary>
    /// Interaction logic for GlowingEarth.xaml
    /// </summary>
    enum clickState { FIND_ITEM, LIST_TO_MAP, MAP_TO_MAP}           //Pomaze nam pri Drag and Drop
                                                                    //Find Item ce hvatati tacke od gornjeg levog do donjeg desnog coska slike.
                                                                    //List to map ce hvatati tacke svih itema koji su na mapi od gornjeg levog do donjeg desnog coska slike.
                                                                    //Map to map ce ignorisati dragItem==m situaciju
    public partial class GlowingEarth : Window, INotifyPropertyChanged
    {
        private ObservableCollection<Model.Etiquette> _tags;        //tagovi
        private ObservableCollection<Model.Resource> _resources;    //resursi
        private ObservableCollection<Model.Type> _types;            //tipovi
        private Model.MapItem _dragItem;                            //Sta se vuce - referenca
        private Model.MapItem mi;                                   //Ono sto se vuce sa mape - zaseban objekat kopija
        private Point startPoint = new Point();                     //Klik tacka
        private ObservableCollection<Model.MapItem> _resOnCanvas;   //Resursi koji su na kanvasu
        private clickState searchMode;                              //Enum odgore
        private bool move;                                          //Da li se ono na mapi pomera ili ne
        private string _title;                                      //Ime projekta
        private string serPath;

        /*--------------------------Binding-----------------------------*/
        public ObservableCollection<Model.Etiquette> tags
        {
            get
            {
                return _tags;
            }
            set
            {
                _tags = value;
                OnPropertyChanged("tags");
            }
        }
        public ObservableCollection<Model.Resource> resources
        {
            get
            {
                return _resources;
            }
            set
            {
                _resources = value;
                OnPropertyChanged("resources");
            }
        }
        public ObservableCollection<Model.Type> types
        {
            get
            {
                return _types;
            }
            set
            {
                _types = value;
                OnPropertyChanged("types");
            }
        }
        public ObservableCollection<Model.MapItem> resOnCanvas
        {
            get
            {
                return _resOnCanvas;
            }
            set
            {
                _resOnCanvas = value;
                OnPropertyChanged("resOnCanvas");
            }
        }
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
        public string title
        {
            get
            {
                return _title;
            }
            set
            {
                _title = value;
                OnPropertyChanged("title");
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
            tags = new ObservableCollection<Model.Etiquette>();
            resources = new ObservableCollection<Model.Resource>();
            types = new ObservableCollection<Model.Type>();
            resOnCanvas = new ObservableCollection<Model.MapItem>();
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
            refresh();
        }

        private void Type_Click(object sender, RoutedEventArgs e)
        {
            View.NewType nt = new View.NewType(this);
            nt.ShowDialog();
            refresh();
        }

        private void Etiq_Click(object sender, RoutedEventArgs e)
        {
            View.NewTag nta = new View.NewTag(this);
            nta.ShowDialog();
            refresh();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            //can.AllowDrop = true;
            refresh();
        }
        public void refresh()
        {
            path = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "typeList");
            if (File.Exists(path))
            {
                fm = new BinaryFormatter();
                sm = File.OpenRead(path);
                types = (ObservableCollection<Model.Type>)fm.Deserialize(sm);
                sm.Dispose();
                sm.Close();
            }
            path = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "tagList");
            if (File.Exists(path))
            {
                fm = new BinaryFormatter();
                sm = File.OpenRead(path);
                tags = (ObservableCollection<Model.Etiquette>)fm.Deserialize(sm);
                sm.Dispose();
                sm.Close();
            }
            path = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "reslist");
            if (File.Exists(path))
            {
                fm = new BinaryFormatter();
                sm = File.OpenRead(path);
                resources = (ObservableCollection<Model.Resource>)fm.Deserialize(sm);
                sm.Dispose();
                sm.Close();
            }
            InitializeComponent();
            this.DataContext = this;
        }

        private void ResEditor_Click(object sender, RoutedEventArgs e)
        {
            ResEditor re = new ResEditor(this);
        }

        private void TypEditor_Click(object sender, RoutedEventArgs e)
        {
            TypeEditor type = new TypeEditor(this);
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
            Point p = new Point(e.GetPosition(map).X-45, e.GetPosition(map).Y-50);
            if (p.X < 0)
            {
                p.X = 0;
            }
            if (p.Y < 0)
            {
                p.Y = 0;
            }

            if (SearchPosition(e.GetPosition(map))==null && !move)
            {
                dragItem.setPosition(p);
                resOnCanvas.Add(dragItem);
            }
            else if(SearchPosition(p) != null && move)
            {
                dragItem.setPosition(mi.getPosition());
                move = false;
            }
            else if(SearchPosition(p) == null && move)
            {
                resOnCanvas.Remove(dragItem);
                dragItem.setPosition(p);
                resOnCanvas.Add(dragItem);
            }
            Console.WriteLine("Item spusten na X:" + p.X + " Y:" + p.Y);
            move = false;
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
            Model.MapItem ret=null;
            foreach (Model.MapItem m in resOnCanvas)
            {
                bool pointIsLefter = false;
                bool pointIsUpper = false;
                if (point.X < m.getPosition().X)
                {
                    pointIsLefter = true;
                }
                if(point.X < m.getPosition().Y)
                {
                    pointIsUpper = true;
                }
                if(point.X==m.getPosition().X && point.Y==m.getPosition().Y && dragItem.name == m.name)
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
                    if (Math.Abs(point.X - m.position.X) <= 65 && Math.Abs(point.Y - m.position.Y) <= 60)
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
            searchMode = clickState.FIND_ITEM;
            Model.MapItem m = SearchPosition(e.GetPosition(map));
            resOnCanvas.Remove(m);
        }

        private void Viewbox_MouseMove(object sender, MouseEventArgs e)
        {
            Console.WriteLine("X:" + e.GetPosition(map).X + " Y:" + e.GetPosition(map).Y);
        }

        public ObservableCollection<Model.Resource> getResources()
        {
            return _resources;
        }
        public ObservableCollection<Model.Type> getTypes()
        {
            return _types;
        }
        public ObservableCollection<Model.Etiquette> getTags()
        {
            return _tags;
        }

        public ObservableCollection<Model.MapItem> getMapItems()
        {
            return _resOnCanvas;
        }

        public void setTitle(string t)
        {
            _title = t;
        }
        public string getTitle()
        {
            return _title;
        }
        public string getSerPath()
        {
            return serPath;
        }
        public void setSerPath(string sp)
        {
            serPath = sp;
        }
    }
}
