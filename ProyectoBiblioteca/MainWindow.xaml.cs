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
            bbdd = new BibliotecaModel();
        }

        private void buscarLibro(object sender, RoutedEventArgs e)
        {
            string textoBusqueda = txtBusqueda.Text.Trim();

            // Verificar si el texto de búsqueda es un ISBN
            var libroEncontrado = bbdd.Libros.FirstOrDefault(libro => libro.ISBN == textoBusqueda);

            // Si no es un ISBN, buscar por título
            if (libroEncontrado == null)
            {
                libroEncontrado = bbdd.Libros.FirstOrDefault(libro => libro.Titulo == (textoBusqueda));
            }

            if (libroEncontrado != null)
            {
                var librosResultados = new[] { new { ISBN = libroEncontrado.ISBN, Título = libroEncontrado.Titulo, Autor = libroEncontrado.Autor, Existencias = libroEncontrado.Existencias } };
                gridResultados.ItemsSource = librosResultados.ToList();
            }
            else
            {
                MessageBox.Show("No se encontró ningún libro con el título o ISBN proporcionado.");
            }
        }

        private void buscarPelicula(object sender, RoutedEventArgs e)
        {
            string textoBusqueda = txtBusqueda.Text.Trim();

            var peliculasPorTitulo = bbdd.Peliculas.Where(pelicula => pelicula.Titulo == (textoBusqueda));

            var peliculasPorDirector = bbdd.Peliculas.Where(pelicula => pelicula.Director.Contains(textoBusqueda));

            // Combinar los resultados de ambas búsquedas
            var peliculasResultados = peliculasPorTitulo.Concat(peliculasPorDirector)
                                                        .Select(pelicula => new { Título = pelicula.Titulo, Director = pelicula.Director, Existencias = pelicula.Existencias })
                                                        .ToList();

            if (peliculasResultados.Any())
            {
                gridResultados.ItemsSource = peliculasResultados;
            }
            else
            {
                MessageBox.Show("No se encontraron películas con el título o director proporcionado.");
            }
        }
    }
}
