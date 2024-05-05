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
    public partial class VentanaCatalogo : Page
    {
        private BibliotecaModel bbdd;

        public VentanaCatalogo()
        {
            InitializeComponent();
            bbdd = new BibliotecaModel();
            gridResultados.MouseDoubleClick += MostrarDetallesObra;
        }

        private void BuscarLibro(object sender, RoutedEventArgs e)
        {
            string textoBusqueda = txtBusqueda.Text.Trim();

            if (!string.IsNullOrWhiteSpace(textoBusqueda) && textoBusqueda.Length > 1)
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
                MessageBox.Show("Ingrese un término de búsqueda váido para realizar la búsqueda.");
            }
        }


        private void BuscarPelicula(object sender, RoutedEventArgs e)
        {
            string textoBusqueda = txtBusqueda.Text.Trim();

            if (!string.IsNullOrWhiteSpace(textoBusqueda) && textoBusqueda.Length > 1)
            {
                var peliculasResultados = bbdd.Peliculas
                .Where(pelicula => pelicula.Titulo.Contains(textoBusqueda) || pelicula.Director.Contains(textoBusqueda))
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
                MessageBox.Show("Ingrese un término de búsqueda váido para realizar la búsqueda.");
            }
        }


        private void MostrarAyuda_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("El parámetro admite:\n\nLibros: Título, ISBN, Autor\nPelículas: Título, Director", "Ayuda", MessageBoxButton.OK, MessageBoxImage.Question);
        }


        private void MostrarDetallesObra(object sender, MouseButtonEventArgs e)
        {
            if (gridResultados.SelectedItem != null)
            {
                dynamic obraSeleccionada = gridResultados.SelectedItem;
                try
                {
                    if (obraSeleccionada.ISBN != null)
                    {
                        string isbn = obraSeleccionada.ISBN;

                        var libroEncontrado = bbdd.Libros.FirstOrDefault(libro => libro.ISBN == isbn);
                        if (libroEncontrado != null)
                        {
                            MostrarDetallesLibro(libroEncontrado);
                        }
                        else
                        {
                            MessageBox.Show("No se encontró el libro en la base de datos.");
                        }
                    }
                }
                catch (Microsoft.CSharp.RuntimeBinder.RuntimeBinderException)
                {
                    //Si sale exception, no tiene ISBN, es película
                    //Nota para obraSeleccionada debes usar los nombres que tienen en el dataGrid no en la clase Libros o Peliculas
                    if (obraSeleccionada.Título != null)
                    {
                        string titulo = obraSeleccionada.Título;

                        var peliculaEncontrada = bbdd.Peliculas.FirstOrDefault(pelicula => pelicula.Titulo == titulo);
                        if (peliculaEncontrada != null)
                        {
                            // Mostrar detalles de la película
                            MostrarDetallesPelicula(peliculaEncontrada);
                        }
                        else
                        {
                            MessageBox.Show("No se encontró la película en la base de datos.");
                        }
                    }
                }
            }
        }


        private void MostrarDetallesLibro(Libros libro)
        {
            StringBuilder detalles = new StringBuilder();
            detalles.AppendLine("Detalles del libro:");
            detalles.AppendLine($"Título: {libro.Titulo}");
            detalles.AppendLine($"Autor: {libro.Autor}");
            detalles.AppendLine($"Género: {libro.Genero}");
            detalles.AppendLine($"Año: {libro.Anio}");
            detalles.AppendLine($"Editorial: {libro.Editorial}");
            detalles.AppendLine($"Existencias: {libro.Existencias}");

            MessageBox.Show(detalles.ToString(), "Detalles del libro", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void MostrarDetallesPelicula(Peliculas pelicula)
        {
            StringBuilder detalles = new StringBuilder();
            detalles.AppendLine("Detalles de la película:");
            detalles.AppendLine($"Título: {pelicula.Titulo}");
            detalles.AppendLine($"Director: {pelicula.Director}");
            detalles.AppendLine($"Género: {pelicula.Genero}");
            detalles.AppendLine($"Año: {pelicula.Anio}");
            detalles.AppendLine($"Duración: {pelicula.Duracion} minutos");
            detalles.AppendLine($"Existencias: {pelicula.Existencias}");

            MessageBox.Show(detalles.ToString(), "Detalles de la película", MessageBoxButton.OK, MessageBoxImage.Information);
        }

    }
}


    
