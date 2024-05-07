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
            bbdd = new BibliotecaModel();
        }

        private void Guardar_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // Validar formato de correo electrónico
                if (!ValidarCorreoElectronico(txtCorreo.Text))
                {
                    MessageBox.Show("Por favor, ingrese un correo electrónico válido.", "Error de formato", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                // Validar que el teléfono tenga exactamente 9 dígitos
                if (!TieneNueveDigitos(txtTelefono.Text))
                {
                    MessageBox.Show("Por favor, ingrese un número de teléfono de 9 dígitos.", "Error de formato", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                // Obtener el último valor de ID_Usuario
                int ultimoID = bbdd.Usuarios.Max(u => u.ID_Usuario);

                // Incrementar el valor para asignarlo al nuevo usuario
                int nuevoID = ultimoID + 1;

                // Obtener los datos ingresados por el usuario
                string nombre = txtNombre.Text;
                string apellido = txtApellido.Text;
                string correo = txtCorreo.Text;
                string telefono = txtTelefono.Text;

                // Crear un nuevo objeto Usuario
                Usuarios nuevoUsuario = new Usuarios
                {
                    ID_Usuario = nuevoID,
                    Nombre = nombre,
                    Apellido = apellido,
                    CorreoElectronico = correo,
                    Telefono = telefono
                };

                // Agregar el nuevo usuario al contexto y guardar los cambios en la base de datos
                bbdd.Usuarios.Add(nuevoUsuario);
                bbdd.SaveChanges();

                // Cerrar la ventana
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

        // Método para validar el formato del correo electrónico
        private bool ValidarCorreoElectronico(string correo)
        {
            // Utilizamos una expresión regular para validar el formato del correo electrónico
            string patron = @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$";
            return Regex.IsMatch(correo, patron);
        }

        // Método para validar si una cadena es un número
        private bool EsNumero(string texto)
        {
            return int.TryParse(texto, out _);
        }

        // Método para validar si el número de teléfono tiene exactamente 9 dígitos
        private bool TieneNueveDigitos(string telefono)
        {
            return telefono.Length == 9 && EsNumero(telefono);
        }
    }
}
