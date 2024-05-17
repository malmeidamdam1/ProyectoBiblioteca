using ProyectoBiblioteca.Model;
using ProyectoBiblioteca.Ventanas;
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
            AgregarLibroWindow agregarLibroWindow = new AgregarLibroWindow();

            Window mainWindow = Window.GetWindow(this);

            agregarLibroWindow.Owner = mainWindow;

            agregarLibroWindow.ShowDialog();

            ActualizarDataGridLibros();
        }
        private void AgregarPelicula(object sender, RoutedEventArgs e)
        {
            AgregarPeliculaWindow agregarPeliWindow = new AgregarPeliculaWindow();

            Window mainWindow = Window.GetWindow(this);

            agregarPeliWindow.Owner = mainWindow;

            agregarPeliWindow.ShowDialog();

            ActualizarDataGridPeliculas();
        }
        private void ActualizarDataGridLibros()
        {
            VerLibros(null, null);
        }
        private void ActualizarDataGridPeliculas()
        {
            VerPeliculas(null, null);
        }
        private void VerLibros(object sender, RoutedEventArgs e)
        {
            labelViendo.Content = "Visualizando la lista de libros";
            var libros = from libro in bbdd.Libros
                         select new
                         {
                             ISBN = libro.ISBN,
                             Título = libro.Titulo,
                             Autor = libro.Autor,
                             Género = libro.Genero,
                             Año = libro.Anio,
                             Editorial = libro.Editorial,
                             Existencias = libro.Existencias,
                         };

            gridResultados.ItemsSource = libros.ToList();
        }

        private void VerPeliculas(object sender, RoutedEventArgs e)
        {
            labelViendo.Content = "Visualizando la lista de películas";

            var peliculas = from pelicula in bbdd.Peliculas
                            select new
                            {
                                ID_Película = pelicula.ID_Pelicula,
                                Título = pelicula.Titulo,
                                Director = pelicula.Director,
                                Género = pelicula.Genero,
                                Año = pelicula.Anio,
                                Duración_Mints = pelicula.Duracion,
                                Existencias = pelicula.Existencias,
                            };

            gridResultados.ItemsSource = peliculas.ToList();
        }

        private void EliminarLibro(object sender, RoutedEventArgs e)
        {
            if (gridResultados.SelectedItem != null)
            {
                MessageBoxResult result = MessageBox.Show("¿Está seguro de que desea eliminar este libro?",
                    "Confirmar eliminación", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (result == MessageBoxResult.Yes)
                {
                    try
                    {
                        dynamic seleccion = gridResultados.SelectedItem;
                        string isbn = seleccion.ISBN;
                        var libroSeleccionado = bbdd.Libros.FirstOrDefault(l => l.ISBN == isbn);
                        if (libroSeleccionado != null)
                        {
                            bbdd.Libros.Remove(libroSeleccionado);
                            bbdd.SaveChanges();
                            ActualizarDataGridLibros();
                            MessageBox.Show("El libro se ha eliminado correctamente.", "Eliminación exitosa",
                                             MessageBoxButton.OK, MessageBoxImage.Information); 
                        }
                        else
                        {
                            MessageBox.Show("No se pudo encontrar el libro para eliminar.", "Error",
                                MessageBoxButton.OK, MessageBoxImage.Error);
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Error al eliminar el libro: " + ex.Message, "Error",
                            MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
            else
            {
                MessageBox.Show("Por favor, seleccione un libro para eliminar.", "Selección requerida", MessageBoxButton.OK,
                    MessageBoxImage.Warning);
            }
        }

        private void EliminarPelicula(object sender, RoutedEventArgs e)
        {
            if (gridResultados.SelectedItem != null)
            {
                MessageBoxResult result = MessageBox.Show("¿Está seguro de que desea eliminar esta película?",
                    "Confirmar eliminación", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (result == MessageBoxResult.Yes)
                {
                    try
                    {
                        dynamic seleccion = gridResultados.SelectedItem;
                        int idPelicula = seleccion.ID_Película;
                        var peliculaSeleccionada = bbdd.Peliculas.FirstOrDefault(p => p.ID_Pelicula == idPelicula);
                        if (peliculaSeleccionada != null)
                        {
                            bbdd.Peliculas.Remove(peliculaSeleccionada);
                            bbdd.SaveChanges();
                            ActualizarDataGridPeliculas();
                            MessageBox.Show("La película se ha eliminado correctamente.", "Eliminación exitosa",
                                MessageBoxButton.OK, MessageBoxImage.Information);

                        }
                        else
                        {
                            MessageBox.Show("No se pudo encontrar la película para eliminar.", "Error",
                                MessageBoxButton.OK, MessageBoxImage.Error);
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Error al eliminar la película: " + ex.Message, "Error",
                            MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
            else
            {
                MessageBox.Show("Por favor, seleccione una película para eliminar.", "Selección requerida", MessageBoxButton.OK,
                    MessageBoxImage.Warning);
            }
        }

        private void EditarLibro(object sender, RoutedEventArgs e)
        {
            try
            {
                if (gridResultados.SelectedItem != null)
                {
                    dynamic seleccion = gridResultados.SelectedItem;
                    string isbn = seleccion.ISBN;
                    Window mainWindow = Window.GetWindow(this);
                    EditarLibroWindow editarLibroWindow = new EditarLibroWindow(isbn);
                    editarLibroWindow.Owner = mainWindow;
                    editarLibroWindow.ShowDialog();
                }
                else
                {
                    MessageBox.Show("Por favor, seleccione un libro para editar.", "Selección requerida", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al abrir la ventana de edición de libro." + "Por favor asegúrese de que ha seleccionado el tipo de obra adecuado", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            ActualizarDataGridLibros();
        }

        private void EditarPelicula(object sender, RoutedEventArgs e)
        {
            try
            {
                if (gridResultados.SelectedItem != null)
                {
                    dynamic seleccion = gridResultados.SelectedItem;
                    int id_peli = seleccion.ID_Película;
                    Window mainWindow = Window.GetWindow(this);
                    EditarPeliculaWindow editarPeliWindow = new EditarPeliculaWindow(id_peli);
                    editarPeliWindow.Owner = mainWindow;
                    editarPeliWindow.ShowDialog();
                }
                else
                {
                    MessageBox.Show("Por favor, seleccione una película para editar.", "Selección requerida", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al abrir la ventana de edición de película. " + "Por favor asegúrese de que ha seleccionado el tipo de obra adecuado", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            ActualizarDataGridPeliculas();
        }

    }
}
