using ProyectoBiblioteca.Model;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace ProyectoBiblioteca.Pages
{
    public partial class VentanaPrestamos : Page
    {
        private BibliotecaModel bbdd;

        public VentanaPrestamos()
        {
            InitializeComponent();
            bbdd = new BibliotecaModel();
            ActualizarPrestamos();
        }

        private void RegistrarPrestamo(object sender, RoutedEventArgs e)
        {
            // Implementar lógica para registrar un nuevo préstamo
        }

        private void RegistrarDevolucion(object sender, RoutedEventArgs e)
        {
            // Implementar lógica para registrar la devolución de un préstamo
        }

        private void RegistrarSancion(object sender, RoutedEventArgs e)
        {
            // Implementar lógica para registrar una sanción
        }

        private void VerDevoluciones(object sender, RoutedEventArgs e)
        {
            var devoluciones = from devolucion in bbdd.Devoluciones
                               join prestamo in bbdd.Prestamos on devolucion.ID_Prestamo equals prestamo.ID_Prestamo into gj
                               from subprestamo in gj.DefaultIfEmpty()
                               join sancion in bbdd.Sanciones on subprestamo.ID_Usuario equals sancion.ID_Usuario into sj
                               from subsancion in sj.DefaultIfEmpty()
                               select new
                               {
                                   TituloObra = subprestamo != null && subprestamo.Libros != null ? subprestamo.Libros.Titulo : (subprestamo != null && subprestamo.Peliculas != null ? subprestamo.Peliculas.Titulo : string.Empty),
                                   NombreUsuario = subprestamo != null && subprestamo.Usuarios != null ? subprestamo.Usuarios.Nombre : string.Empty,
                                   FechaPrestamo = subprestamo != null ? subprestamo.FechaPrestamo : (DateTime?)null,
                                   FechaDevolucionPrevista = devolucion.FechaDevolucion,
                                   Sancion = subsancion != null ? subsancion.Motivo : string.Empty
                               };

            gridResultados.ItemsSource = devoluciones.ToList();
        }



        private void ActualizarPrestamos()
        {
            var prestamos = from prestamo in bbdd.Prestamos
                                //select prestamo;
                            select new
                            {
                                TituloObra = prestamo.Libros != null ? prestamo.Libros.Titulo : (prestamo.Peliculas != null ? prestamo.Peliculas.Titulo : string.Empty),
                                NombreUsuario = prestamo.ID_Usuario != null ? prestamo.Usuarios.Nombre : string.Empty,
                                FechaPrestamo = prestamo.FechaPrestamo,
                                FechaDevolucionPrevista = prestamo.FechaDevolucion
                            };

            gridResultados.ItemsSource = prestamos.ToList();
        }

    }
}
