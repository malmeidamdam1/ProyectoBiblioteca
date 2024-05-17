using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using ProyectoBiblioteca.Model;

namespace ProyectoBiblioteca.Pages
{
    public partial class VentanaSanciones : Page
    {
        private int idUsuario;
        private BibliotecaModel bbdd;

        public VentanaSanciones(int idUsuario)
        {
            InitializeComponent();
            this.idUsuario = idUsuario;
            bbdd = new BibliotecaModel();
            // Mostrar el nombre e ID del usuario pasado como parámetro
            Usuarios usuario = bbdd.Usuarios.FirstOrDefault(u => u.ID_Usuario == idUsuario);
            if (usuario != null)
            {
                lblNombreUsuario.Content = usuario.Nombre;
                lblIDUsuario.Content = usuario.ID_Usuario;
            }
            // Mostrar solo el usuario pasado como parámetro en el ComboBox
            cmbUsuarios.ItemsSource = new[] { usuario };
            cmbUsuarios.DisplayMemberPath = "Nombre";
            cmbUsuarios.SelectedValuePath = "ID_Usuario";
            cmbUsuarios.SelectedIndex = 0;
            CargarSanciones();
            MessageBox.Show($"Cargando al usuario {usuario.Nombre}, complete el motivo y duración de la sanción.", "Sancionando usuario", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        public VentanaSanciones()
        {
            InitializeComponent();
            bbdd = new BibliotecaModel();
            MostrarUsuarios();
            CargarSanciones();
        }

        private void CambioUsuario(object sender, SelectionChangedEventArgs e)
        {
            int idUsuarioSeleccionado = (int)cmbUsuarios.SelectedValue;
            Usuarios usuarioSeleccionado = bbdd.Usuarios.FirstOrDefault(u => u.ID_Usuario == idUsuarioSeleccionado);

            if (usuarioSeleccionado != null)
            {
                // Actualizar el nombre e ID del usuario seleccionado
                lblNombreUsuario.Content = usuarioSeleccionado.Nombre;
                lblIDUsuario.Content = +usuarioSeleccionado.ID_Usuario;
            }
        }


        private void MostrarUsuarios()
        {
            var usuarios = bbdd.Usuarios.Select(u => new { ID_Usuario = u.ID_Usuario, Nombre = u.Nombre }).ToList();
            cmbUsuarios.ItemsSource = usuarios;
            cmbUsuarios.DisplayMemberPath = "Nombre";
            cmbUsuarios.SelectedValuePath = "ID_Usuario";
        }

        private void CargarSanciones()
        {
            try
            {
                var sanciones = bbdd.Sanciones.Select(s => new { ID_Sancion = s.ID_Sancion, Motivo = s.Motivo, FechaSancion = s.FechaSancion, FechaFinSancion = s.FechaFinSancion }).ToList();
                gridSanciones.ItemsSource = sanciones;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al cargar las sanciones: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void RegistrarSancion(object sender, RoutedEventArgs e)
        {
            try
            {
                string motivo = txtMotivo.Text;
                string duracion = txtDuracion.Text;
                DateTime fechaFinSancion = DateTime.Now.AddDays(Convert.ToInt32(duracion));

                int idUsuarioSeleccionado = (int)cmbUsuarios.SelectedValue;

                // Obtener el último ID de sanciones y sumarle uno para el nuevo ID
                int ultimoIdSancion = bbdd.Sanciones.Any() ? bbdd.Sanciones.Max(s => s.ID_Sancion) : 0;
                int nuevoIdSancion = ultimoIdSancion + 1;

                Sanciones nuevaSancion = new Sanciones
                {
                    ID_Sancion = nuevoIdSancion,
                    ID_Usuario = idUsuarioSeleccionado,
                    Motivo = motivo,
                    FechaSancion = DateTime.Now,
                    FechaFinSancion = fechaFinSancion
                };
                bbdd.Sanciones.Add(nuevaSancion);
                bbdd.SaveChanges();

                MessageBox.Show("Sanción registrada correctamente.", "Éxito", MessageBoxButton.OK, MessageBoxImage.Information);

            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al registrar la sanción: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            CargarSanciones();
        }


        private void EliminarSancion(object sender, RoutedEventArgs e)
        {
            try
            {
                if (gridSanciones.SelectedItem != null)
                {
                    MessageBoxResult result = MessageBox.Show("¿Está seguro de que desea eliminar la sanción seleccionada?", "Confirmar eliminación", MessageBoxButton.YesNo, MessageBoxImage.Question);
                    if (result == MessageBoxResult.Yes)
                    {
                        dynamic sancionSeleccionada = gridSanciones.SelectedItem;

                        int idSancion = sancionSeleccionada.ID_Sancion;

                        // Buscar la sanción en la base de datos y eliminarla
                        Sanciones sancion = bbdd.Sanciones.Find(idSancion);
                        if (sancion != null)
                        {
                            bbdd.Sanciones.Remove(sancion);
                            bbdd.SaveChanges();
                            CargarSanciones();
                            MessageBox.Show("Sanción eliminada correctamente.", "Éxito", MessageBoxButton.OK, MessageBoxImage.Information);
                        }
                        else
                        {
                            MessageBox.Show("No se pudo encontrar la sanción seleccionada.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                        }
                    }
                }
                else
                {
                    MessageBox.Show("Por favor, seleccione una sanción para eliminar.", "Selección requerida", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al eliminar la sanción: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
