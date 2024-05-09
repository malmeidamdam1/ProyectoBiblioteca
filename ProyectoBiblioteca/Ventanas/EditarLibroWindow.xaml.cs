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
    public partial class EditarLibroWindow : Window
    {
        private Libros libroEditar;
        private BibliotecaModel bbdd;

        public EditarLibroWindow(Libros libro)
        {
            InitializeComponent();
            WindowStartupLocation = WindowStartupLocation.CenterOwner;
            libroEditar = libro;
            bbdd = new BibliotecaModel();

            // Mostrar los datos del libro pasado como parámetro en los campos de texto
            txtISBN.Text = libro.ISBN;
            txtTitulo.Text = libro.Titulo;
            txtAutor.Text = libro.Autor;
            txtGenero.Text = libro.Genero;
            txtAno.Text = libro.Anio.ToString();
            txtEditorial.Text = libro.Editorial;
            txtExistencias.Text = libro.Existencias.ToString();
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

                // Obtener los datos modificados
                libroEditar.ISBN = txtISBN.Text;
                libroEditar.Titulo = txtTitulo.Text;
                libroEditar.Autor = txtAutor.Text;
                libroEditar.Genero = txtGenero.Text;
                libroEditar.Anio = int.Parse(txtAno.Text);
                libroEditar.Editorial = txtEditorial.Text;
                libroEditar.Existencias = int.Parse(txtExistencias.Text);

                // Guardar los cambios en la base de datos
                bbdd.SaveChanges();

                // Cerrar la ventana
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
