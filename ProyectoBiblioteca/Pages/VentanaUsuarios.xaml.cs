using ProyectoBiblioteca.Model;
using ProyectoBiblioteca.Ventanas;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace ProyectoBiblioteca.Pages
{
    public partial class VentanaUsuarios : Page
    {
        private BibliotecaModel bbdd;

        public VentanaUsuarios()
        {
            InitializeComponent();
            bbdd = new BibliotecaModel();

            // Llamar a VerUsuarios al inicializar la ventana para cargar los datos automáticamente
            VerUsuarios(null, null);
        }

        private void AgregarUsuario(object sender, RoutedEventArgs e)
        {
            AgregarUsuarioVentana agregarUsuario = new AgregarUsuarioVentana();

            Window mainWindow = Window.GetWindow(this);

            agregarUsuario.Owner = mainWindow;

            agregarUsuario.ShowDialog();

            // Actualizar la lista de usuarios después de agregar uno nuevo
            ActualizarUsuario();
        }

        private void VerUsuarios(object sender, RoutedEventArgs e)
        {
            var usuarios = from usuario in bbdd.Usuarios
                           join sancion in bbdd.Sanciones on usuario.ID_Usuario equals sancion.ID_Usuario into sj
                           from subsancion in sj.DefaultIfEmpty()
                           select new
                           {
                               ID_Usuario = usuario.ID_Usuario,
                               Nombre = usuario.Nombre,
                               Apellido = usuario.Apellido,
                               CorreoElectronico = usuario.CorreoElectronico,
                               Telefono = usuario.Telefono,
                               HaTenidoSancion = subsancion != null
                           };

            gridResultados.ItemsSource = usuarios.ToList();
        }

        private void probar(object sender, RoutedEventArgs e)
        {
            if (gridResultados.SelectedItem != null)
            {
                dynamic usuarioSeleccionado = gridResultados.SelectedItem;
                int idUsuario = usuarioSeleccionado.ID_Usuario;

                // usuarioSeleccionado = bbdd.Usuarios.FirstOrDefault(u => u.ID_Usuario == idUsuario);

                //    EditarUsuarioWindow editarUsuarioWindow = new EditarUsuarioWindow(idUsuario);


                     MessageBox.Show(idUsuario.ToString());

                    VentanaPrestamos ventanaPrestamos = new VentanaPrestamos(idUsuario);

                    NavigationService.Navigate(ventanaPrestamos);

                
                // Crear una instancia del Page VentanaPrestamos pasando el usuario seleccionado como parámetro

                // Navegar a la página VentanaPrestamos
            }
            else
            {
                MessageBox.Show("Por favor, seleccione un usuario en el DataGrid.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }


        private void EliminarUsuario(object sender, RoutedEventArgs e)
        {
            if (gridResultados.SelectedItem != null)
            {
                MessageBoxResult result = MessageBox.Show("¿Está seguro de que desea eliminar este usuario?",
                "Confirmar eliminación", MessageBoxButton.YesNo, MessageBoxImage.Question);

                if (result == MessageBoxResult.Yes)
                {
                    try
                    {
                        Usuarios usuarioSeleccionado = (Usuarios)gridResultados.SelectedItem;

                        bbdd.Usuarios.Remove(usuarioSeleccionado);
                        bbdd.SaveChanges();

                        // Actualizar la lista de usuarios después de eliminar uno
                        ActualizarUsuario();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Error al eliminar el usuario: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
            else
            {
                MessageBox.Show("Por favor, seleccione un usuario para eliminar", "Selección requerida", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void EditarUsuario(object sender, RoutedEventArgs e)
        {
            try
            {
                if (gridResultados.SelectedItem != null)
                {
                    dynamic usuarioSeleccionado = gridResultados.SelectedItem;
                    int idUsuario = usuarioSeleccionado.ID_Usuario;

                    EditarUsuarioWindow editarUsuarioWindow = new EditarUsuarioWindow(idUsuario);
                    editarUsuarioWindow.ShowDialog();

                    // Actualizar el DataGrid después de la edición
                    ActualizarUsuario();
                }
                else
                {
                    MessageBox.Show("Por favor, seleccione un usuario para editar.", "Selección requerida", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al editar el usuario " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void ActualizarUsuario()
        {
            VerUsuarios(null, null);
        }
    }
}
