using ProyectoBiblioteca.Model;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace ProyectoBiblioteca.Ventanas
{
    public partial class RegistrarPrestamoWindow : Window
    {
        private BibliotecaModel bbdd;

        public RegistrarPrestamoWindow()
        {
            InitializeComponent();
            bbdd = new BibliotecaModel();

            // Cargar libros en el ListBox de libros
            lstLibros.ItemsSource = bbdd.Libros.ToList();

            // Cargar películas en el ListBox de películas
            lstPeliculas.ItemsSource = bbdd.Peliculas.ToList();

            // Cargar usuarios en el ListBox de usuarios
            lstUsuarios.ItemsSource = bbdd.Usuarios.ToList();
        }

        private void Registrar_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // Verificar si se ha seleccionado un libro o una película
                if (lstLibros.SelectedItem == null && lstPeliculas.SelectedItem == null)
                {
                    MessageBox.Show("Por favor, seleccione un libro o una película.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                // Verificar si se ha seleccionado un usuario
                if (lstUsuarios.SelectedItem == null)
                {
                    MessageBox.Show("Por favor, seleccione un usuario.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                // Obtener el ID del libro o película seleccionado
                string idLibro = null;
                int? idPelicula = null;

                if (lstLibros.SelectedItem != null)
                {
                    Libros libroSeleccionado = (Libros)lstLibros.SelectedItem;
                    idLibro = libroSeleccionado.ISBN;
                }
                else if (lstPeliculas.SelectedItem != null)
                {
                    Peliculas peliculaSeleccionada = (Peliculas)lstPeliculas.SelectedItem;
                    idPelicula = peliculaSeleccionada.ID_Pelicula;
                }

                // Verificar existencias
                int existencias = 0;
                if (idLibro != null)
                {
                    existencias = bbdd.Libros.Where(libro => libro.ISBN == idLibro).Select(libro => libro.Existencias).FirstOrDefault() ?? 0;
                }
                else if (idPelicula != null)
                {
                    existencias = bbdd.Peliculas.Where(pelicula => pelicula.ID_Pelicula == idPelicula).Select(pelicula => pelicula.Existencias).FirstOrDefault() ?? 0;
                }

                if (existencias <= 0)
                {
                    MessageBox.Show("No hay existencias suficientes para realizar el préstamo.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                // Crear un nuevo préstamo
                Prestamos nuevoPrestamo = new Prestamos
                {
                    ID_Libro = idLibro,
                    ID_Pelicula = idPelicula,
                    ID_Usuario = ((Usuarios)lstUsuarios.SelectedItem).ID_Usuario,
                    FechaPrestamo = DateTime.Now, // Fecha actual
                    FechaDevolucion = dpFechaDevolucion.SelectedDate // Fecha de devolución seleccionada en el DatePicker
                };

                // Agregar el nuevo préstamo a la base de datos
                bbdd.Prestamos.Add(nuevoPrestamo);
                bbdd.SaveChanges();

                MessageBox.Show("Préstamo registrado correctamente.", "Éxito", MessageBoxButton.OK, MessageBoxImage.Information);
                this.Close(); // Cerrar la ventana después de registrar el préstamo
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al registrar el préstamo: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void ListBox_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            // Obtener el ListBox
            var listBox = sender as ListBox;

            // Verificar si se ha seleccionado un elemento
            if (listBox.SelectedItem != null)
            {
                // Obtener el elemento seleccionado
                var selectedItem = listBox.SelectedItem;

                // Mostrar existencias
                int existencias = 0;
                if (selectedItem is Libros libro)
                {
                    existencias = libro.Existencias ?? 0;
                }
                else if (selectedItem is Peliculas pelicula)
                {
                    existencias = pelicula.Existencias ?? 0;
                }

                MessageBox.Show($"Existencias: {existencias}", "Existencias", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }
    }
}
