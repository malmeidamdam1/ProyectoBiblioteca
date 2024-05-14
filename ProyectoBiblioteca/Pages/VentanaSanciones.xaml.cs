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
        }

        public VentanaSanciones()
        {
            InitializeComponent();
            bbdd = new BibliotecaModel();
        }

        private void RegistrarSancion_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string motivo = txtMotivo.Text;
                DateTime fechaFinSancion = dpFechaFinSancion.SelectedDate ?? DateTime.Now;

                // Crear una nueva instancia de Sanciones
                Sanciones nuevaSancion = new Sanciones
                {
                    ID_Usuario = idUsuario,
                    Motivo = motivo,
                    FechaSancion = DateTime.Now, 
                    FechaFinSancion = fechaFinSancion
                };

                // Agregar la nueva sanción a la base de datos
                bbdd.Sanciones.Add(nuevaSancion);
                bbdd.SaveChanges();

                MessageBox.Show("Sanción registrada correctamente.", "Éxito", MessageBoxButton.OK, MessageBoxImage.Information);

            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al registrar la sanción: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
