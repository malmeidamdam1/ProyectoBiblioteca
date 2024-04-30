using ProyectoBiblioteca;
using ProyectoBiblioteca.Model;
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


namespace ProyectoBiblioteca
{
    public partial class MainWindow : Window
    {
        private BibliotecaModel bbdd;

        public MainWindow()
        {
            InitializeComponent();
            WindowStartupLocation = WindowStartupLocation.CenterScreen;
            bbdd = new BibliotecaModel();
        }

        private void Menu_Seleccionado(object sender, SelectionChangedEventArgs e)
        {
            var selected = sidebar.SelectedItem as NavButton;

            navFrame.Navigate(selected.Navlink);
        }




    }
}