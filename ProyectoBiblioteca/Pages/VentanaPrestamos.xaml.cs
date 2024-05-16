using ProyectoBiblioteca.Model;
using ProyectoBiblioteca.Ventanas;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace ProyectoBiblioteca.Pages
{
    public partial class VentanaPrestamos : Page
    {
        private BibliotecaModel bbdd;
        private int idUsuario;


        public VentanaPrestamos()
        {
            InitializeComponent();
            bbdd = new BibliotecaModel();
            MostrarPrestamosActivos();
        }

        public VentanaPrestamos(int idUsuario)
        {
            InitializeComponent();
            this.idUsuario = idUsuario;
            bbdd = new BibliotecaModel();
            Usuarios usuarioSeleccionado = bbdd.Usuarios.FirstOrDefault(u => u.ID_Usuario == idUsuario);
            MostrarPrestamosActivos();
            MessageBox.Show($"Usuario seleccionado: {usuarioSeleccionado.Nombre}", "Información", MessageBoxButton.OK, MessageBoxImage.Information);

        }

        private void RegistrarPrestamo(object sender, RoutedEventArgs e)
        {
            RegistrarPrestamoWindow registroPrestamo = new RegistrarPrestamoWindow();
            Window mainWindow = Window.GetWindow(this);
            registroPrestamo.Owner = mainWindow;
            registroPrestamo.ShowDialog();
            MostrarPrestamosActivos();
        }

        private void RegistrarDevolucion(object sender, RoutedEventArgs e)
        {
            try
            {
                if (gridResultados.SelectedItem != null)
                {
                    MessageBoxResult result = MessageBox.Show("¿Está seguro de que desea registrar la devolución para el préstamo seleccionado?", "Confirmar devolución", MessageBoxButton.YesNo, MessageBoxImage.Question);

                    if (result == MessageBoxResult.Yes)
                    {
                        // Obtener el préstamo seleccionado en el DataGrid
                        dynamic prestamoSeleccionado = gridResultados.SelectedItem;

                        int idPrestamo = prestamoSeleccionado.ID_Prestamo;

                        // Mostrar ventana de confirmación

                        int ultimoIdDevolucion = bbdd.Devoluciones.Any() ? bbdd.Devoluciones.Max(p => p.ID_Devolucion) : 0;
                        int nuevoIdDevolucion = ultimoIdDevolucion + 1;

                        // Crear una nueva instancia de Devoluciones
                        Devoluciones nuevaDevolucion = new Devoluciones
                        {
                            ID_Devolucion = nuevoIdDevolucion,
                            ID_Prestamo = idPrestamo,
                            FechaDevolucion = DateTime.Now // Fecha actual
                        };
                        bbdd.Devoluciones.Add(nuevaDevolucion);
                        bbdd.SaveChanges();

                        MessageBox.Show("Devolución registrada correctamente.", "Éxito", MessageBoxButton.OK, MessageBoxImage.Information);
                        MostrarPrestamosActivos(); // Actualizar la lista de préstamos después de registrar la devolución
                    }
                }
                else
                {
                    MessageBox.Show("Por favor, seleccione un préstamo para registrar la devolución.", "Selección requerida", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al registrar la devolución: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }


        private void RegistrarSancion(object sender, RoutedEventArgs e)
        {
            try
            {
                if (gridResultados.SelectedItem != null)
                {
                    dynamic prestamoSeleccionado = gridResultados.SelectedItem;
                    int idUsuario = prestamoSeleccionado.ID_Usuario;

                    // Navegar hacia la ventana de Sanciones y pasar el ID_Usuario como parámetro
                    VentanaSanciones ventanaSanciones = new VentanaSanciones(idUsuario);
                    NavigationService.Navigate(ventanaSanciones);

                    MostrarPrestamosActivos();
                }
                else
                {
                    MessageBox.Show("Por favor, seleccione un préstamo para registrar la sanción.", "Selección requerida", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al registrar la sanción: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void VerDevoluciones(object sender, RoutedEventArgs e)
        {
            labelDataGrid.Content = "Préstamos finalizados";

            var devoluciones = from devolucion in bbdd.Devoluciones
                               join prestamo in bbdd.Prestamos on devolucion.ID_Prestamo equals prestamo.ID_Prestamo into pd 
                               //pd tiene campos coincidentes de Préstamos y Devoluciones
                               from subprestamo in pd.DefaultIfEmpty() 
                               //Si no hay coincidntes hace uno que es null
                               join sancion in bbdd.Sanciones on subprestamo.ID_Usuario equals sancion.ID_Usuario into ps 
                               from subsancion in ps.DefaultIfEmpty()
                               select new
                               {
                                   ID_Prestamo = subprestamo.ID_Prestamo,
                                   TituloObra = subprestamo != null && subprestamo.Libros != null ? subprestamo.Libros.Titulo : (subprestamo != null && subprestamo.Peliculas != null ? subprestamo.Peliculas.Titulo : string.Empty),
                                   NombreUsuario = subprestamo != null && subprestamo.Usuarios != null ? subprestamo.Usuarios.Nombre : string.Empty,
                                   FechaPrestamo = subprestamo != null ? subprestamo.FechaPrestamo : (DateTime?)null,
                                   FechaDevolucionPrevista = subprestamo.FechaPrestamo,
                                   FechaDevuelta = devolucion.FechaDevolucion,
                                   Sanción = subsancion != null ? subsancion.Motivo : string.Empty
                               };

            gridResultados.ItemsSource = devoluciones.ToList();
        }


        private void MostrarPrestamosActivos()
        {
            labelDataGrid.Content = "Préstamos activos";

            var prestamosActivos = from prestamo in bbdd.Prestamos
                                   where !bbdd.Devoluciones.Any(devolucion => devolucion.ID_Prestamo == prestamo.ID_Prestamo)
                                   select new
                                   {
                                       ID_Prestamo = prestamo.ID_Prestamo, //Necesario para obtenerlo en RegistrarDevoluciones
                                       TituloObra = prestamo.Libros != null ? prestamo.Libros.Titulo : (prestamo.Peliculas != null ? prestamo.Peliculas.Titulo : string.Empty),
                                       NombreUsuario = prestamo.Usuarios != null ? prestamo.Usuarios.Nombre : string.Empty,
                                       ID_Usuario = prestamo.Usuarios != null ? prestamo.Usuarios.ID_Usuario : 0,
                                       FechaPrestamo = prestamo.FechaPrestamo,
                                       FechaDevolucionPrevista = prestamo.FechaDevolucion
                                   };
            gridResultados.ItemsSource = prestamosActivos.ToList();
        }

        private void Actualizar(object sender, RoutedEventArgs e)
        {
            MostrarPrestamosActivos();
        }


    }
}
