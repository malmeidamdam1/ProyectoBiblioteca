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

namespace ProyectoBiblioteca.Pages
{
    public partial class VentanaInventario : Page
    {
        private BibliotecaModel bbdd;

        public VentanaInventario()
        {
            InitializeComponent();
            bbdd = new BibliotecaModel();
        }
        private void AgregarLibro(object sender, RoutedEventArgs e)
        {
            // Aquí puedes implementar la lógica para agregar un nuevo libro
        }

        private void AgregarPelicula(object sender, RoutedEventArgs e)
        {
            // Aquí puedes implementar la lógica para agregar una nueva película
        }

        private void VerLibros(object sender, RoutedEventArgs e)
        {
            // Consulta LINQ para obtener todos los libros de la base de datos
            var libros = from libro in bbdd.Libros
                         select libro;

            // Asignar la lista de libros al DataGrid
            gridResultados.ItemsSource = libros.ToList();
        }

        private void VerPeliculas(object sender, RoutedEventArgs e)
        {
            // Consulta LINQ para obtener todas las películas de la base de datos
            var peliculas = from pelicula in bbdd.Peliculas
                            select pelicula;

            // Asignar la lista de películas al DataGrid
            gridResultados.ItemsSource = peliculas.ToList();
        }

        private void EliminarLibro(object sender, RoutedEventArgs e)
        {
            // Aquí puedes implementar la lógica para eliminar un libro seleccionado del DataGrid
        }

        private void EliminarPeliculas(object sender, RoutedEventArgs e)
        {
            // Aquí puedes implementar la lógica para eliminar una película seleccionada del DataGrid
        }
    }

}
