﻿using ProyectoBiblioteca.Model;
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

                if (string.IsNullOrWhiteSpace(txtISBN.Text) || string.IsNullOrWhiteSpace(txtTitulo.Text) ||
                     string.IsNullOrWhiteSpace(txtAutor.Text) || string.IsNullOrWhiteSpace(txtGenero.Text) ||
                     string.IsNullOrWhiteSpace(txtAno.Text) || string.IsNullOrWhiteSpace(txtEditorial.Text) ||
                     string.IsNullOrWhiteSpace(txtExistencias.Text))
                {
                    MessageBox.Show("Por favor, complete todos los campos.", "Campos incompletos", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                if (txtISBN.Text.Length != 13)
                {
                    MessageBox.Show("El ISBN debe constar de 13 dígitos.", "Error de formato", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

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
