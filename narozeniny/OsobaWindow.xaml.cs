using System;
using System.Collections.Generic;
using System.Linq;
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
using System.Windows.Shapes;
using static System.Net.WebRequestMethods;

namespace narozeniny
{
    /// <summary>
    /// Interakční logika pro OsobaWindow.xaml
    /// </summary>
    public partial class OsobaWindow : Window
    {
        private SpravceOsob spravceOsob;
        private Func<bool, bool> callback;

        public OsobaWindow(SpravceOsob spravceOsob, Func<bool, bool> f)
        {
            InitializeComponent();
            this.spravceOsob = spravceOsob;
            this.callback = f;
        }

        private void okButton_Click(object sender, RoutedEventArgs e)
        {
            try {
                spravceOsob.Pridej(jmenoTextBox.Text, narozeninyDatePicker.SelectedDate);
                Close();
            }catch(Exception ex) {
                MessageBox.Show(ex.Message, "Chyba", MessageBoxButton.OK, MessageBoxImage.Exclamation);
            }
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            this.callback?.Invoke(true);
        }
    }
}
