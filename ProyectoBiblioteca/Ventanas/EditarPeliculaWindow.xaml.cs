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
    public partial class EditarPeliculaWindow : Window
    {
        private Peliculas peliculaEditar;
        private BibliotecaModel bbdd;

        public EditarPeliculaWindow(Peliculas pelicula)
        {
            InitializeComponent();
            peliculaEditar = pelicula;
            bbdd = new BibliotecaModel();

            txtTitulo.Text = peliculaEditar.Titulo;
            txtDirector.Text = peliculaEditar.Director;
            txtGenero.Text = peliculaEditar.Genero;
            txtAno.Text = peliculaEditar.Anio.ToString();
            txtDuracion.Text = peliculaEditar.Duracion.ToString();
            txtExistencias.Text = peliculaEditar.Existencias.ToString();
        }

        private void Guardar_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (!int.TryParse(txtAno.Text, out _))
                {
                    MessageBox.Show("Por favor, ingrese un año válido.", "Error de formato", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                if (!int.TryParse(txtDuracion.Text, out int duracion) || duracion <= 0)
                {
                    MessageBox.Show("Por favor, ingrese una duración válida.", "Error de formato", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                if (!int.TryParse(txtExistencias.Text, out int existencias) || existencias < 0)
                {
                    MessageBox.Show("Por favor, ingrese un número válido de existencias.", "Error de formato", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                // Actualizar los datos de la película
                peliculaEditar.Titulo = txtTitulo.Text;
                peliculaEditar.Director = txtDirector.Text;
                peliculaEditar.Genero = txtGenero.Text;
                peliculaEditar.Anio = int.Parse(txtAno.Text);
                peliculaEditar.Duracion = int.Parse(txtDuracion.Text);
                peliculaEditar.Existencias = int.Parse(txtExistencias.Text);


                bbdd.SaveChanges();


                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al guardar los cambios: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Cancelar_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
