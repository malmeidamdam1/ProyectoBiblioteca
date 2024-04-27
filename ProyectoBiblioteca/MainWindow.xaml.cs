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

        private void buscarLibro(object sender, RoutedEventArgs e)
        {
            string textoBusqueda = txtBusqueda.Text.Trim();

            if (!string.IsNullOrWhiteSpace(textoBusqueda))
            {
                var librosEncontrados = bbdd.Libros
                    .Where(libro =>
                    libro.ISBN == textoBusqueda ||
                    libro.Titulo.Contains(textoBusqueda) ||
                    libro.Autor.Contains(textoBusqueda)
                );

                if (librosEncontrados.Any())
                {
                    var librosResultados = librosEncontrados.Select(libro =>
                        new { ISBN = libro.ISBN, Título = libro.Titulo, Autor = libro.Autor, Existencias = libro.Existencias }
                    ).ToList();
                    gridResultados.ItemsSource = librosResultados;
                }
                else
                {
                    MessageBox.Show("No se encontró ningún libro con el título, ISBN o autor proporcionado.");
                }
            }
            else
            {
                MessageBox.Show("Ingrese un término de búsqueda antes de realizar la búsqueda.");
            }
        }


        private void buscarPelicula(object sender, RoutedEventArgs e)
        {
            string textoBusqueda = txtBusqueda.Text.Trim();

            if (!string.IsNullOrWhiteSpace(textoBusqueda))
            {
                var peliculasResultados = bbdd.Peliculas
                .Where(pelicula => pelicula.Titulo == textoBusqueda || pelicula.Director == textoBusqueda)
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
            else
            {
                MessageBox.Show("Ingrese un término de búsqueda antes de realizar la búsqueda.");
            }
        }


        private void MostrarAyuda_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("El parámetro admite:\n\nLibros: Título, ISBN, Autor\nPelículas: Título, Director", "Ayuda", MessageBoxButton.OK, MessageBoxImage.Question);
        }

    }
}
