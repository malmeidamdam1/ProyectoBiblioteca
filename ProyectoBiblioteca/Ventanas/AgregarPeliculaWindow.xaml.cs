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
                // Validar que el año sea un número entero
                if (!int.TryParse(txtAno.Text, out _))
                {
                    MessageBox.Show("Por favor, ingrese un año válido.", "Error de formato", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                // Obtener el último valor de ID_Pelicula
                int ultimoID = bbdd.Peliculas.Max(p => p.ID_Pelicula);

                // Incrementar el valor para asignarlo a la nueva película
                int nuevoID = ultimoID + 1;

                // Obtener los datos ingresados por el usuario
                string titulo = txtTitulo.Text;
                string director = txtDirector.Text;
                string genero = txtGenero.Text;
                int ano = int.Parse(txtAno.Text);
                int duracion = int.Parse(txtDuracion.Text);
                int existencias = int.Parse(txtExistencias.Text);

                // Crear un nuevo objeto Pelicula
                Peliculas nuevaPelicula = new Peliculas
                {
                    ID_Pelicula = nuevoID,
                    Titulo = titulo,
                    Director = director,
                    Genero = genero,
                    Anio = ano,
                    Duracion = duracion,
                    Existencias = existencias
                };

                // Agregar la nueva película al contexto y guardar los cambios en la base de datos
                bbdd.Peliculas.Add(nuevaPelicula);
                bbdd.SaveChanges();

                // Cerrar la ventana
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
