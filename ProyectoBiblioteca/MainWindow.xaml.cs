using ProyectoBiblioteca;
using ProyectoBiblioteca.Model;
using ProyectoBiblioteca.Pages;
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
            if (sidebar.SelectedItem != null)
            {
                var selected = sidebar.SelectedItem as NavButton;
                navFrame.Navigate(selected.Navlink);
            }
        }

        private void logoImage_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            navFrame.Content = null;

            // Crear una instancia de la página de inicio y cargarla en el Frame
            labelInicio.Visibility = Visibility.Visible;
            textBlockInicio.Visibility = Visibility.Visible;
            botonInicio1.Visibility = Visibility.Visible;
            botonInicio2.Visibility = Visibility.Visible;
            botonInicio3.Visibility = Visibility.Visible;


            // Deseleccionar cualquier elemento del ListBox
            if (sidebar.SelectedItem != null)
            {
                sidebar.SelectedItem = null;
            }
        }

        private void navFrame_Navigated(object sender, NavigationEventArgs e)
        {
            // Verificar si el contenido del Frame es una página
            if (navFrame.Content is Page)
            {
                // Ocultar elementos de la MainWindow
                labelInicio.Visibility = Visibility.Collapsed;
                textBlockInicio.Visibility = Visibility.Collapsed;
                botonInicio1.Visibility = Visibility.Collapsed;
                botonInicio2.Visibility = Visibility.Collapsed;
                botonInicio3.Visibility = Visibility.Collapsed;
            }
            else
            {
                // Mostrar elementos de la MainWindow
                logoImage.Visibility = Visibility.Visible;
                sidebar.Visibility = Visibility.Visible;
                labelInicio.Visibility = Visibility.Visible;
            }
        }

       }
}