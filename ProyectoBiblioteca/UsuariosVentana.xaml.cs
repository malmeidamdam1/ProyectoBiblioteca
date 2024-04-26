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
using System.Windows.Shapes;

namespace ProyectoBiblioteca
{
    /// <summary>
    /// Lógica de interacción para Usuarios.xaml
    /// </summary>
    public partial class UsuariosVentana : Window
    {
        public UsuariosVentana()
        {
            InitializeComponent();
        }

        private void btnAgregarUsuario_Click(object sender, RoutedEventArgs e)
        {
            // Abre la ventana emergente para agregar usuario
            AgregarUsuarioVentana agregarUsuarioWindow = new AgregarUsuarioVentana();
            agregarUsuarioWindow.ShowDialog();
        }

        private void btnEliminarUsuario_Click(object sender, RoutedEventArgs e)
        {
            // Aquí implementa la lógica para eliminar usuario
        }



        private void btnEditarUsuario_Click(object sender, RoutedEventArgs e)
        {
            // Aquí implementa la lógica para editar usuario
        }

        private void btnVerDetalles_Click(object sender, RoutedEventArgs e)
        {
            // Aquí implementa la lógica para ver detalles de usuario
        }
    }
}
