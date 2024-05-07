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
    
    public partial class AgregarLibroWindow : Window
    {
        private BibliotecaModel bbdd;

        public AgregarLibroWindow()
        {
            InitializeComponent();
            WindowStartupLocation = WindowStartupLocation.CenterOwner;
            bbdd = new BibliotecaModel();

        }

        private void Guardar_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(txtISBN.Text) || string.IsNullOrWhiteSpace(txtTitulo.Text) ||
                    string.IsNullOrWhiteSpace(txtAutor.Text) || string.IsNullOrWhiteSpace(txtGenero.Text) ||
                    string.IsNullOrWhiteSpace(txtAno.Text) || string.IsNullOrWhiteSpace(txtEditorial.Text) ||
                    string.IsNullOrWhiteSpace(txtExistencias.Text))
                {
                    MessageBox.Show("Por favor, complete todos los campos.", "Campos incompletos", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                //Out para no almacenar la variable transformada, solo queremos la comprobacion T/F
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

                // Crear un nuevo objeto Libro con los datos ingresados
                Libros nuevoLibro = new Libros
                {
                    ISBN = txtISBN.Text,
                    Titulo = txtTitulo.Text,
                    Autor = txtAutor.Text,
                    Genero = txtGenero.Text,
                    Anio = int.Parse(txtAno.Text),
                    Editorial = txtEditorial.Text,
                    Existencias = existencias
                };

                // Agregar el nuevo libro a la base de datos y guardar los cambios
                bbdd.Libros.Add(nuevoLibro);
                bbdd.SaveChanges();

                MessageBox.Show("Libro agregado correctamente.", "Éxito", MessageBoxButton.OK, MessageBoxImage.Information);

                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al agregar el libro: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Cancelar_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

    }
}
