using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Data.Common;
using System.IO;
using System.Linq;
using System.Reflection.Emit;
using System.Runtime.CompilerServices;
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

namespace narozeniny
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    /// 
    public class WindowCommand : ICommand
    {
        private MainWindow _window;

        //Set this delegate when you initialize a new object. This is the method the command will execute. You can also change this delegate type if you need to.
        public Action ExecuteDelegate { get; set; }

        //You don't have to add a parameter that takes a constructor. I've just added one in case I need access to the window directly.
        public WindowCommand(MainWindow window)
        {
            _window = window;
        }

        //always called before executing the command, mine just always returns true
        public bool CanExecute(object parameter)
        {
            return true; //mine always returns true, yours can use a new CanExecute delegate, or add custom logic to this method instead.
        }

        public event EventHandler CanExecuteChanged; //i'm not using this, but it's required by the interface

        //the important method that executes the actual command logic
        public void Execute(object parameter)
        {
            if (ExecuteDelegate != null)
            {
                ExecuteDelegate();
            }
            else
            {
                throw new InvalidOperationException();
            }
        }
    }

    public partial class MainWindow : Window
    {
        private SpravceOsob spravceOsob = new SpravceOsob();


        public MainWindow()
        {
            InitializeComponent();
            // dnesLabel.Text = DateTime.Now.ToLongDateString();
            osobyListBox.ItemsSource = spravceOsob.Osoby;
            datumDnes.Text = DateTime.Now.ToShortDateString();

            InputBindings.Add(new KeyBinding( //add a new key-binding, and pass in your command object instance which contains the Execute method which WPF will execute
               new WindowCommand(this)
               {
                   ExecuteDelegate = SaveButton_Click
               }, new KeyGesture(Key.S, ModifierKeys.Control)));
            InputBindings.Add(new KeyBinding( //add a new key-binding, and pass in your command object instance which contains the Execute method which WPF will execute
                new WindowCommand(this)
                {
                    ExecuteDelegate = LoadButton_Click
                }, new KeyGesture(Key.L, ModifierKeys.Control)));
        }

        public bool setNew(bool e) {
            var a = spravceOsob.NejblizsiOsoba;
            if(a == null) {
                return false;
            }
            zaDni.Text = a.ZbyvaDni().ToString();
            jmeno.Text = a.Jmeno;
            return true;
        }


        private void pridatButton_Click(object sender, EventArgs e)
        {
            OsobaWindow osobaForm = new OsobaWindow(spravceOsob, setNew);
            osobaForm.ShowDialog();

        }

        private void OdebratButton_Click(object sender, RoutedEventArgs e)
        {
            var selected = osobyListBox.SelectedIndex;
            if(selected == -1) {
                return;
            }
            spravceOsob.Osoby.RemoveAt(selected);
            setNew(true);
        }

        private void osobyListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if(osobyListBox.SelectedIndex == -1) {
                return;
            }
            var osoba = spravceOsob.Osoby[osobyListBox.SelectedIndex];

            narozeniny.Text = osoba.Narozeniny.ToShortDateString();
            vek.Text = osoba.SpoctiVek().ToString();
            narozenCalendar.DisplayDate = osoba.Narozeniny;
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.SaveFileDialog dlg = new Microsoft.Win32.SaveFileDialog();
            dlg.FileName = "narozeniny";
            dlg.DefaultExt = ".csv";
            dlg.Filter = "Comma-separated values (.csv) |*.csv";
            if (dlg.ShowDialog() != true){
                return;
            }
            string csv = "Jmeno,Narozeniny\n";
            foreach (Osoba osoba in this.spravceOsob.Osoby) {
                csv += osoba.Jmeno +","+ osoba.Narozeniny.ToString() + "\n";
            }
            File.WriteAllText(dlg.FileName, csv);
        }

        private void SaveButton_Click()
        {
            Microsoft.Win32.SaveFileDialog dlg = new Microsoft.Win32.SaveFileDialog();
            dlg.FileName = "narozeniny";
            dlg.DefaultExt = ".csv";
            dlg.Filter = "Comma-separated values (.csv) |*.csv";
            if (dlg.ShowDialog() != true)
            {
                return;
            }
            string csv = "Jmeno,Narozeniny\n";
            foreach (Osoba osoba in this.spravceOsob.Osoby)
            {
                csv += osoba.Jmeno + "," + osoba.Narozeniny.ToString() + "\n";
            }
            File.WriteAllText(dlg.FileName, csv);
        }
        // funkce znovu protože pro shortcut potřebuji overload fukce aby se mohla stát delegát
        private void LoadButton_Click(object sender, RoutedEventArgs e)
        {
            var dlg = new Microsoft.Win32.OpenFileDialog();
            dlg.Filter = "Comma-separated values (.csv) |*.csv";
            if(dlg.ShowDialog() != true) {
                return;
            }
            var lines = File.ReadAllLines(dlg.FileName);
            foreach (var line in lines)
            {
                if (line == "Jmeno,Narozeniny")
                    continue;
                String[] a = line.Split(',');
                String jmeno = a[0];
                String narozeniny = a[1];
                spravceOsob.Pridej(jmeno, DateTime.Parse(narozeniny));
            }
            setNew(true);
        }
        // funkce znovu protože pro shortcut potřebuji overload fukce aby se mohla stát delegát
        private void LoadButton_Click()
        {
            var dlg = new Microsoft.Win32.OpenFileDialog();
            dlg.Filter = "Comma-separated values (.csv) |*.csv";
            if (dlg.ShowDialog() != true)
            {
                return;
            }
            var lines = File.ReadAllLines(dlg.FileName);
            foreach (var line in lines)
            {
                if (line == "Jmeno,Narozeniny")
                    continue;
                String[] a = line.Split(',');
                String jmeno = a[0];
                String narozeniny = a[1];
                spravceOsob.Pridej(jmeno, DateTime.Parse(narozeniny));
            }
            setNew(true);
        }
    }
}
