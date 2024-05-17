using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using ProyectoBiblioteca.Model;

namespace ProyectoBiblioteca.Pages
{
    public partial class VentanaInforme : Page
    {
        private BibliotecaModel bbdd;

        public VentanaInforme()
        {
            InitializeComponent();
            bbdd = new BibliotecaModel();
            CargarComboBoxes();
        }

        private void CargarComboBoxes()
        {
            // Cargar películas en el ComboBox de películas
            cmbPeliculas.ItemsSource = bbdd.Peliculas.ToList();
            cmbPeliculas.DisplayMemberPath = "Titulo";
            cmbPeliculas.SelectedValuePath = "ID_Pelicula";

            // Cargar libros en el ComboBox de libros
            cmbLibros.ItemsSource = bbdd.Libros.ToList();
            cmbLibros.DisplayMemberPath = "Titulo";
            cmbLibros.SelectedValuePath = "ISBN";

            // Cargar usuarios en el ComboBox de usuarios
            cmbUsuarios.ItemsSource = bbdd.Usuarios.ToList();
            cmbUsuarios.DisplayMemberPath = "Nombre";
            cmbUsuarios.SelectedValuePath = "ID_Usuario";
        }

        private void GenerarInforme_Click(object sender, RoutedEventArgs e)
        {
            //if (cmbTipoInforme.SelectedItem == null || ((cmbPeliculas.SelectedItem == null && cmbLibros.SelectedItem == null) && cmbUsuarios.SelectedItem == null))
            if (cmbTipoInforme.SelectedItem == null)
            {
                MessageBox.Show("Por favor, seleccione el tipo de informe.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            string tipoInforme = (cmbTipoInforme.SelectedItem as ComboBoxItem).Content.ToString();

            switch (tipoInforme)
            {
                case "Porcentaje de préstamos [obra]":
                    GenerarInformePorcentajePrestamos();
                    break;
                case "Género más predominante [obra]":
                    GenerarInformeGeneroPopular();
                    break;
                case "Obra más solicitada":
                    GenerarInformeObraMasSolicitada();
                    break;
                case "Porcentaje de sanciones [usuario]":
                    GenerarInformePorcentajeSanciones();
                    break;
                case "Veces se ha prestado [usuario]":
                    GenerarInformeVecesPrestado();
                    break;
                default:
                    MessageBox.Show("Tipo de informe no válido.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    break;
            }
        }

        private void GenerarInformePorcentajePrestamos()
        {
            if (cmbPeliculas.SelectedItem != null || cmbLibros.SelectedItem != null)
            {
                int idPelicula = 0;
                string idLibro = "";
                string tituloObra = "";
                int prestamosObra = 0;
                int totalPrestamos = bbdd.Prestamos.Count();

                if (cmbPeliculas.SelectedItem != null)
                {
                    idPelicula = (int)cmbPeliculas.SelectedValue;
                    tituloObra = (cmbPeliculas.SelectedItem as Peliculas)?.Titulo;
                    prestamosObra = bbdd.Prestamos.Where(p => p.ID_Pelicula == idPelicula).Count();
                }
                else if (cmbLibros.SelectedItem != null)
                {
                    idLibro = cmbLibros.SelectedValue.ToString();
                    tituloObra = (cmbLibros.SelectedItem as Libros)?.Titulo;
                    prestamosObra = bbdd.Prestamos.Where(p => p.ID_Libro == idLibro.ToString()).Count();
                }

                // Calcular el porcentaje de préstamos de la obra seleccionada
                double porcentajePrestamos = Math.Round((double)prestamosObra / totalPrestamos * 100, 2);

                MessageBox.Show($"La obra {tituloObra} tiene un porcentaje de préstamo del: {porcentajePrestamos}%", "Informe", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
                MessageBox.Show("Debe seleccionar una obra (libro o película) para generar el informe.", "Advertencia", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void GenerarInformeGeneroPopular()
        {
            string generoPopular = "";
            string tipoObra = "";
            if (cmbPeliculas.SelectedItem != null)
            {
                tipoObra = "las películas";
                var peliculas = bbdd.Peliculas.ToList();

                generoPopular = peliculas
                    .SelectMany(p => p.Genero.Split(',')) // Crear un array de gé>neros
                    .GroupBy(g => g.Trim()) // Agrupar por género y eliminar espacios en blanco
                    .OrderByDescending(grp => grp.Count()) // Ordenar por frecuencia
                    .Select(grp => grp.Key) // Obtener el género
                    .FirstOrDefault(); // Tomar el primer género más popular
            }
            else if (cmbLibros.SelectedItem != null)
            {
                tipoObra = "los libros";
                var libros = bbdd.Libros.ToList();

                generoPopular = libros
                    .SelectMany(l => l.Genero.Split(','))
                    .GroupBy(g => g.Trim())
                    .OrderByDescending(grp => grp.Count())
                    .Select(grp => grp.Key)
                    .FirstOrDefault();
            }

            if (!string.IsNullOrEmpty(generoPopular))
            {
                MessageBox.Show($"El género más predominante en {tipoObra} es: {generoPopular}", "Informe", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
                MessageBox.Show("Debe seleccionar una obra (libro o película) para generar el informe.", "Advertencia", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }


        // No requiere de una obra seleccionada
        private void GenerarInformeObraMasSolicitada()
        {
            string obraMasSolicitada = "";
            string tipoObra = "";
            int maxPrestamos = 0;
            try
            {

                // Contar el número de préstamos para cada película
                var prestamosPeliculas = bbdd.Prestamos.Where(p => p.Peliculas != null)
                                                        .GroupBy(p => p.Peliculas.ID_Pelicula)
                                                        .Select(g => new
                                                        {
                                                            ISBN = g.Key,
                                                            NumPrestamos = g.Count()
                                                        });

                // Contar el número de préstamos para cada libro
                var prestamosLibros = bbdd.Prestamos.Where(p => p.Libros != null)
                                                    .GroupBy(p => p.Libros.ISBN)
                                                    .Select(g => new
                                                    {
                                                        ID_Libro = g.Key,
                                                        NumPrestamos = g.Count()
                                                    });

                // Obtener la película con más préstamos
                var peliculaMasSolicitada = prestamosPeliculas.OrderByDescending(p => p.NumPrestamos).FirstOrDefault();
                if (peliculaMasSolicitada != null && peliculaMasSolicitada.NumPrestamos > maxPrestamos)
                {
                    maxPrestamos = peliculaMasSolicitada.NumPrestamos;
                    obraMasSolicitada = bbdd.Peliculas.FirstOrDefault(p => p.ID_Pelicula == peliculaMasSolicitada.ISBN)?.Titulo;
                    tipoObra = "la película";
                }

                // Obtener el libro con más préstamos y verificar si le gana al mayor de películas
                var libroMasSolicitado = prestamosLibros.OrderByDescending(p => p.NumPrestamos).FirstOrDefault();
                if (libroMasSolicitado != null && libroMasSolicitado.NumPrestamos > maxPrestamos)
                {
                    maxPrestamos = libroMasSolicitado.NumPrestamos;
                    obraMasSolicitada = bbdd.Libros.FirstOrDefault(l => l.ISBN == libroMasSolicitado.ID_Libro)?.Titulo;
                    tipoObra = "el libro";
                }

                if (!string.IsNullOrEmpty(obraMasSolicitada))
                {
                    MessageBox.Show($"La obra más solicitada es {tipoObra}: {obraMasSolicitada}, con {maxPrestamos} préstamos.", "Informe", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else
                {
                    MessageBox.Show("No hay una obra más solicitada, no se encontraron préstamos registrados.", "Informe", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al generar el informe: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }


        private void GenerarInformePorcentajeSanciones()
        {
            try
            {
                if (cmbUsuarios.SelectedItem != null)
                {
                    int idUsuario = (int)cmbUsuarios.SelectedValue;
                    string nombreUsuario = (cmbUsuarios.SelectedItem as dynamic)?.Nombre;
                    int totalSanciones = bbdd.Sanciones.Count();
                    int sancionesUsuario = bbdd.Sanciones.Where(s => s.ID_Usuario == idUsuario).Count();

                    if (totalSanciones > 0)
                    {
                        double porcentajeSanciones = Math.Round((double)sancionesUsuario / totalSanciones * 100, 2);
                        MessageBox.Show($"El usuario {nombreUsuario} consta en un: {porcentajeSanciones}% de las sanciones totales actualmente", "Informe", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                    else
                    {
                        MessageBox.Show($"No hay sanciones registradas en la base de datos.", "Informe", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                }
                else
                {
                    MessageBox.Show("Debe seleccionar un usuario para generar el informe.", "Advertencia", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al generar el informe: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void GenerarInformeVecesPrestado()
        {
            if (cmbUsuarios.SelectedItem != null)
            {
                int idUsuario = (int)cmbUsuarios.SelectedValue;
                string nombreUsuario = (cmbUsuarios.SelectedItem as dynamic)?.Nombre;

                int prestamosUsuario = bbdd.Prestamos.Where(p => p.ID_Usuario == idUsuario).Count();

                if (prestamosUsuario <= 1)
                {
                    MessageBox.Show($"El usuario {nombreUsuario} ha realizado {prestamosUsuario} préstamo en la biblioteca.", "Informe", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else
                {
                    MessageBox.Show($"El usuario {nombreUsuario} ha realizado {prestamosUsuario} préstamos en la biblioteca.", "Informe", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            else
            {
                MessageBox.Show("Debe seleccionar un usuario para generar el informe.", "Advertencia", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void cmbPeliculas_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // Para que no se elimine la primera vez que cambias
            if (cmbPeliculas.SelectedIndex != -1)
            {
                // Limpiar de los demas
                cmbLibros.SelectedIndex = -1;
                cmbUsuarios.SelectedIndex = -1;
                ActualizarOpcionesInforme();
            }
        }

        private void cmbLibros_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cmbLibros.SelectedIndex != -1)
            {
                cmbPeliculas.SelectedIndex = -1;
                cmbUsuarios.SelectedIndex = -1;
                ActualizarOpcionesInforme();
            }
        }

        private void cmbUsuarios_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cmbUsuarios.SelectedIndex != -1)
            {
                cmbPeliculas.SelectedIndex = -1;
                cmbLibros.SelectedIndex = -1;
                ActualizarOpcionesInforme();
            }
        }

        private void ActualizarOpcionesInforme()
        {
            bool obraSeleccionada = cmbPeliculas.SelectedItem != null || cmbLibros.SelectedItem != null;
            bool usuarioSeleccionado = cmbUsuarios.SelectedItem != null;

            // Habilitar o deshabilitar las opciones del ComboBox según si necesita el parametro de la obra o usuario
            foreach (ComboBoxItem item in cmbTipoInforme.Items)
            {
                string tipoInforme = item.Content.ToString();

                if (obraSeleccionada)
                {
                    if (tipoInforme == "Porcentaje de préstamos [obra]" || tipoInforme == "Género más predominante [obra]" || tipoInforme == "Obra más solicitada")
                    {
                        item.IsEnabled = true;
                    }
                    else if (tipoInforme != "Obra más solicitada")
                    {
                        item.IsEnabled = false;
                    }
                }

                else if (usuarioSeleccionado)
                {
                    if (tipoInforme == "Porcentaje de sanciones [usuario]" || tipoInforme == "Veces se ha prestado [usuario]")
                    {
                        item.IsEnabled = true;
                    }
                    else
                    {
                        item.IsEnabled = false;
                    }
                }
                else
                {
                    item.IsEnabled = false;
                }
            }
        }
    }
}
