using ProyectoBiblioteca.Model;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace ProyectoBiblioteca.Ventanas
{
    public partial class InformeWindow : Page
    {
        private BibliotecaModel bbdd;

        public InformeWindow()
        {
            InitializeComponent();
            bbdd = new BibliotecaModel();
        }

        private void GenerarInforme_Click(object sender, RoutedEventArgs e)
        {
            if (cmbTipoInforme.SelectedItem != null)
            {
                string tipoInforme = ((ComboBoxItem)cmbTipoInforme.SelectedItem).Content.ToString();

                switch (tipoInforme)
                {
                    case "Préstamos":
                        GenerarInformePrestamos();
                        break;
                    // Otros casos aquí
                    default:
                        MessageBox.Show("Seleccione un tipo de informe válido.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                        break;
                }
            }
            else
            {
                MessageBox.Show("Seleccione un tipo de informe.", "Información", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private void GenerarInformePrestamos()
        {
            if (!string.IsNullOrEmpty(txtIDObra.Text))
            {
                string idObra = txtIDObra.Text;


                // Obtener el total de préstamos
                int totalPrestamos = bbdd.Prestamos.Count();

                // Obtener el número de préstamos para la obra especificada
                int prestamosObra = bbdd.Prestamos.Count(p => p.ID_Libro == idObra);

                if (totalPrestamos > 0)
                {
                    double porcentaje = (double)prestamosObra / totalPrestamos * 100;
                    MessageBox.Show($"El {porcentaje}% de los préstamos son de la obra con ID: {idObra}", "Informe de Préstamos", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else
                {
                    MessageBox.Show("No se han registrado préstamos.", "Informe de Préstamos", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            else
            {
                MessageBox.Show("Ingrese el ID de un libro o película.", "Informe de Préstamos", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }
    }
}
