using ProyectoBiblioteca.Model;
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

namespace ProyectoBiblioteca.Ventanas
{
    public partial class AgregarPeliculaWindow : Window
    {
        private BibliotecaModel bbdd;

        public AgregarPeliculaWindow()
        {
            InitializeComponent();
            WindowStartupLocation = WindowStartupLocation.CenterOwner;
            bbdd = new BibliotecaModel();
        }

        private void Guardar_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(txtTitulo.Text) || string.IsNullOrWhiteSpace(txtDirector.Text) || string.IsNullOrWhiteSpace(txtGenero.Text) 
                    ||string.IsNullOrWhiteSpace(txtAno.Text) || string.IsNullOrWhiteSpace(txtDuracion.Text) || string.IsNullOrWhiteSpace(txtExistencias.Text))

                {
                    MessageBox.Show("Por favor, complete todos los campos.", "Campos incompletos", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                // Validar que el año sea un número entero
                if (!int.TryParse(txtAno.Text, out _))
                {
                    MessageBox.Show("Por favor, ingrese un año válido.", "Error de formato", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                // Validar que las existencias sean un número entero positivo
                if (!int.TryParse(txtExistencias.Text, out int existencias) || existencias < 0)
                {
                    MessageBox.Show("Por favor, ingrese un número válido de existencias.", "Error de formato", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                if (!int.TryParse(txtDuracion.Text, out _))
                {
                    MessageBox.Show("Por favor, ingrese un número válido de minutos", "Error de formato", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                int ultimoID = bbdd.Peliculas.Max(p => p.ID_Pelicula);

                int nuevoID = ultimoID + 1;

                Peliculas nuevaPelicula = new Peliculas
                {
                    ID_Pelicula = nuevoID,
                    Titulo = txtTitulo.Text,
                    Director = txtDirector.Text,
                    Genero = txtGenero.Text,
                    Anio = int.Parse(txtAno.Text),
                    Duracion = int.Parse(txtDuracion.Text),
                    Existencias = existencias
                };
                bbdd.Peliculas.Add(nuevaPelicula);
                bbdd.SaveChanges();
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al guardar la película: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Cancelar_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }


    }
}
