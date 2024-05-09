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
        }

        private void AgregarPelicula(object sender, RoutedEventArgs e)
        {
            AgregarPeliculaWindow agregarPeliWindow = new AgregarPeliculaWindow();

            Window mainWindow = Window.GetWindow(this);

            agregarPeliWindow.Owner = mainWindow;

            agregarPeliWindow.ShowDialog();
        }

        private void VerLibros(object sender, RoutedEventArgs e)
        {
            var libros = from libro in bbdd.Libros
                         select libro;

            gridResultados.ItemsSource = libros.ToList();
        }

        private void VerPeliculas(object sender, RoutedEventArgs e)
        {
            var peliculas = from pelicula in bbdd.Peliculas
                            select pelicula;

            gridResultados.ItemsSource = peliculas.ToList();
        }

        private void EliminarLibro(object sender, RoutedEventArgs e)
        {
            if (gridResultados.SelectedItem != null)
            {
                MessageBoxResult result = MessageBox.Show("¿Está seguro de que desea eliminar este libro?", "Confirmar eliminación", MessageBoxButton.YesNo, MessageBoxImage.Question);

                if (result == MessageBoxResult.Yes)
                {
                    try
                    {
                        Libros libroSeleccionado = (Libros)gridResultados.SelectedItem;

                        bbdd.Libros.Remove(libroSeleccionado);
                        bbdd.SaveChanges();

                        ActualizarDataGridLibros();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Error al eliminar el libro: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
            else
            {
                MessageBox.Show("Por favor, seleccione un libro para eliminar.", "Selección requerida", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void ActualizarDataGridLibros()
        {
            var libros = from libro in bbdd.Libros
                         select libro;
            gridResultados.ItemsSource = libros.ToList();
        }


        private void EliminarPeliculas(object sender, RoutedEventArgs e)
        {
            if (gridResultados.SelectedItem != null)
            {
                MessageBoxResult result = MessageBox.Show("¿Está seguro de que desea eliminar esta película?", "Confirmar eliminación", MessageBoxButton.YesNo, MessageBoxImage.Question);

                if (result == MessageBoxResult.Yes)
                {
                    try
                    {
                        Peliculas peliculaSeleccionada = (Peliculas)gridResultados.SelectedItem;

                        bbdd.Peliculas.Remove(peliculaSeleccionada);
                        bbdd.SaveChanges();

                        ActualizarDataGridPeliculas();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Error al eliminar la película: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
            else
            {
                MessageBox.Show("Por favor, seleccione una película para eliminar.", "Selección requerida", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void ActualizarDataGridPeliculas()
        {
            var peliculas = from pelicula in bbdd.Peliculas
                            select pelicula;

            gridResultados.ItemsSource = peliculas.ToList();
        }

        private void EditarLibro(object sender, RoutedEventArgs e)
        {
            try
            {
                if (gridResultados.SelectedItem != null)
                {
                    Libros libroSeleccionado = (Libros)gridResultados.SelectedItem;

                    EditarLibroWindow editarLibroWindow = new EditarLibroWindow(libroSeleccionado);

                    editarLibroWindow.ShowDialog();

                    ActualizarDataGridLibros();
                }
                else
                {
                    MessageBox.Show("Por favor, seleccione un libro para editar.", "Selección requerida", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al abrir la ventana de edición de libro. " + "Por favor asegúrese de que ha seleccionado el tipo de obra adecuado", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void EditarPelicula(object sender, RoutedEventArgs e)
        {
            try
            {
                if (gridResultados.SelectedItem != null)
                {
                    Peliculas peliculaSeleccionada = (Peliculas)gridResultados.SelectedItem;

                    EditarPeliculaWindow editarPeliWindow = new EditarPeliculaWindow(peliculaSeleccionada);

                    editarPeliWindow.ShowDialog();

                    ActualizarDataGridLibros();
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
        }

    }

}
