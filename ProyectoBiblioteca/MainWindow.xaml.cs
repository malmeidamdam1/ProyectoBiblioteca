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
        }

        private void botonInicio1_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("¡Bienvenido a la aplicación Gestión de Biblioteca!\n " +
                "Recuerda verificar que tienes creada la base de datos con el nombre: GestionBiblioteca.\n"+ 
                "Si sigues experimentando problemas comprueba que el nombre de tu servidor sea: (LocalDB)\\MSSQLLocalDB)"
                                      ,"Importante", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void botonInicio2_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Preguntas Frecuentes\n\n" +
                    "1. ¿Cómo puedo agregar un nuevo libro o película al catálogo?" +  "-Desde la ventana inventario\n"+
                    "2. ¿Qué debo hacer si un usuario ha perdido un libro?" + "-Se deberá crear una nueva sanción\n" +
                    "2. ¿Qué debo hacer si ha caducado una sanción?" + "-Si la fecha de fin de la sanción ya ha sido superada, se debe eliminar la sanción desde la ventana sanciones\n" +
                    "Si tienes alguna otra pregunta, no dudes en contactarnos.",
                    "Preguntas Frecuentes", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void botonInicio3_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Sobre nuestro equipo:\n" +
                "Desarrolado por: Midgard Almeida", "Información", MessageBoxButton.OK, MessageBoxImage.Information);
        }

    }
}