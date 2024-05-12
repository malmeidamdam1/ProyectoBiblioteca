using ProyectoBiblioteca.Model;
using System;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;

namespace ProyectoBiblioteca.Ventanas
{
    public partial class AgregarUsuarioVentana : Window
    {
        private BibliotecaModel bbdd;


        public AgregarUsuarioVentana()
        {
            InitializeComponent();
            WindowStartupLocation = WindowStartupLocation.CenterOwner;
            bbdd = new BibliotecaModel();
        }

        private void Guardar_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (!ValidarCorreoElectronico(txtCorreo.Text))
                {
                    MessageBox.Show("Por favor, ingrese un correo electrónico válido.", "Error de formato", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                if (!TieneNueveDigitos(txtTelefono.Text))
                {
                    MessageBox.Show("Por favor, ingrese un número de teléfono de 9 dígitos.", "Error de formato", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                // Obtener el último valor de ID_Usuario
                int ultimoID = bbdd.Usuarios.Max(u => u.ID_Usuario);

                // Sumar uno al último ID y añadirlo
                int nuevoID = ultimoID + 1;

                string nombre = txtNombre.Text;
                string apellido = txtApellido.Text;
                string correo = txtCorreo.Text;
                string telefono = txtTelefono.Text;

                Usuarios nuevoUsuario = new Usuarios
                {
                    ID_Usuario = nuevoID,
                    Nombre = nombre,
                    Apellido = apellido,
                    CorreoElectronico = correo,
                    Telefono = telefono
                };

                bbdd.Usuarios.Add(nuevoUsuario);
                bbdd.SaveChanges();

                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al guardar el usuario: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
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
