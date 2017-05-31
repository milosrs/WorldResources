using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace WorldResources.View
{
    static class RoutedCommands
    {
        public static RoutedUICommand NewMap = new RoutedUICommand(
            "New Map",
            "NewMap",
            typeof(RoutedCommands),
            new InputGestureCollection()
            {
                new KeyGesture(Key.N, ModifierKeys.Control)
            }
            );

        public static RoutedUICommand NewRes = new RoutedUICommand(
            "New Resource",
            "NewRes",
            typeof(RoutedCommands),
            new InputGestureCollection()
            {
                new KeyGesture(Key.R, ModifierKeys.Control | ModifierKeys.Shift)
            }
            );

        public static RoutedUICommand NewType = new RoutedUICommand(
            "New Type",
            "NewType",
            typeof(RoutedCommands),
            new InputGestureCollection()
            {
                new KeyGesture(Key.T, ModifierKeys.Control | ModifierKeys.Shift)
            }
            );

        public static RoutedUICommand EditRes = new RoutedUICommand(
            "Edit Resource",
            "EditRes",
            typeof(RoutedCommands),
            new InputGestureCollection()
            {
                new KeyGesture(Key.R, ModifierKeys.Alt)
            }
            );

        public static RoutedUICommand EditType = new RoutedUICommand(
            "Edit Types",
            "EditType",
            typeof(RoutedCommands),
            new InputGestureCollection()
            {
                new KeyGesture(Key.T, ModifierKeys.Alt)
            }
            );

        public static RoutedUICommand EditTag = new RoutedUICommand(
            "Edit Tags",
            "EditTag",
            typeof(RoutedCommands),
            new InputGestureCollection()
            {
                new KeyGesture(Key.E, ModifierKeys.Alt)
            }
            );

        public static RoutedUICommand NewTag = new RoutedUICommand(
            "New Tag",
            "NewTag",
            typeof(RoutedCommands),
            new InputGestureCollection()
            {
                new KeyGesture(Key.E, ModifierKeys.Control | ModifierKeys.Shift)
            }
            );

        public static RoutedUICommand LoadMap = new RoutedUICommand(
            "Load Map",
            "LoadMap",
            typeof(RoutedCommands),
            new InputGestureCollection()
            {
                new KeyGesture(Key.L, ModifierKeys.Control)
            }
            );

        public static RoutedUICommand SaveMap = new RoutedUICommand(
            "Save Map",
            "SaveMap",
            typeof(RoutedCommands),
            new InputGestureCollection()
            {
                new KeyGesture(Key.S, ModifierKeys.Control)
            }
            );

        public static RoutedUICommand SaveAsMap = new RoutedUICommand(
            "Save Map As",
            "SaveAsMap",
            typeof(RoutedCommands),
            new InputGestureCollection()
            {
                new KeyGesture(Key.S, ModifierKeys.Control | ModifierKeys.Shift)
            }
            );

        public static RoutedUICommand ClearMap = new RoutedUICommand(
            "Clear Map",
            "ClearMap",
            typeof(RoutedCommands),
            new InputGestureCollection()
            {
                new KeyGesture(Key.D, ModifierKeys.Control)
            }
            );

        public static RoutedUICommand Exit = new RoutedUICommand(
            "Exit",
            "Exit",
            typeof(RoutedCommands),
            new InputGestureCollection()
            {
                new KeyGesture(Key.Q, ModifierKeys.Control)
            }
            );

        public static RoutedUICommand OpenHelp = new RoutedUICommand(
            "Help",
            "OpenHelp",
            typeof(RoutedCommands),
            new InputGestureCollection()
            {
                new KeyGesture(Key.F1)
            }
            );
    }
}
