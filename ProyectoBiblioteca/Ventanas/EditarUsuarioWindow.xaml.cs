using ProyectoBiblioteca.Model;
using System;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;

namespace ProyectoBiblioteca.Ventanas
{
    public partial class EditarUsuarioWindow : Window
    {
        private int idUsuario;
        private BibliotecaModel bbdd;

        public EditarUsuarioWindow(int idUsuario)
        {
            InitializeComponent();
            WindowStartupLocation = WindowStartupLocation.CenterScreen;
            this.idUsuario = idUsuario;
            bbdd = new BibliotecaModel();
            // Al abrir la ventana, cargar los datos del usuario en los campos de texto
            CargarDatosUsuario();
        }

        private void CargarDatosUsuario()
        {
            // Buscar el usuario en la base de datos por su ID
            var usuario = bbdd.Usuarios.FirstOrDefault(u => u.ID_Usuario == idUsuario);

            if (usuario != null)
            {
                // Mostrar los datos del usuario en los campos de texto
                txtNombre.Text = usuario.Nombre;
                txtApellido.Text = usuario.Apellido;
                txtCorreo.Text = usuario.CorreoElectronico;
                txtTelefono.Text = usuario.Telefono;
            }
            else
            {
                MessageBox.Show("No se pudo encontrar el usuario.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                this.Close();
            }
        }

        private void Guardar_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var usuario = bbdd.Usuarios.FirstOrDefault(u => u.ID_Usuario == idUsuario);

                if (string.IsNullOrWhiteSpace(txtNombre.Text) || string.IsNullOrWhiteSpace(txtApellido.Text) ||
                    string.IsNullOrWhiteSpace(txtCorreo.Text) || string.IsNullOrWhiteSpace(txtTelefono.Text))
                {
                    MessageBox.Show("Por favor, complete todos los campos.", "Campos incompletos", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                if (!ValidarCorreoElectronico(txtCorreo.Text))
                {
                    MessageBox.Show("Por favor, ingrese un correo electrónico válido.", "Error de formato", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                if (!TieneNueveDigitos(txtTelefono.Text))
                {
                    MessageBox.Show("Por favor, ingrese un número de teléfono válido.", "Error de formato", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                if (usuario != null)
                {
                    usuario.Nombre = txtNombre.Text;
                    usuario.Apellido = txtApellido.Text;
                    usuario.CorreoElectronico = txtCorreo.Text;
                    usuario.Telefono = txtTelefono.Text;

                    bbdd.SaveChanges();
                }

                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al guardar los cambios del usuario: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Cancelar_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private bool ValidarCorreoElectronico(string correo)
        {
            string patron = @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$";
            return Regex.IsMatch(correo, patron);
        }

        private bool EsNumero(string texto)
        {
            return int.TryParse(texto, out _);
        }

        private bool TieneNueveDigitos(string telefono)
        {
            return telefono.Length == 9 && EsNumero(telefono);
        }
    }
}
