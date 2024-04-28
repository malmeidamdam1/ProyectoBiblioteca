//------------------------------------------------------------------------------
// <auto-generated>
//     Este código se generó a partir de una plantilla.
//
//     Los cambios manuales en este archivo pueden causar un comportamiento inesperado de la aplicación.
//     Los cambios manuales en este archivo se sobrescribirán si se regenera el código.
// </auto-generated>
//------------------------------------------------------------------------------

namespace ProyectoBiblioteca.Model
{
    using System;
    using System.Collections.Generic;
    
    public partial class Prestamos
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Prestamos()
        {
            this.Devoluciones = new HashSet<Devoluciones>();
        }
    
        public int ID_Prestamo { get; set; }
        public Nullable<int> ID_Usuario { get; set; }
        public string ID_Libro { get; set; }
        public Nullable<int> ID_Pelicula { get; set; }
        public Nullable<System.DateTime> FechaPrestamo { get; set; }
        public Nullable<System.DateTime> FechaDevolucion { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Devoluciones> Devoluciones { get; set; }
        public virtual Libros Libros { get; set; }
        public virtual Peliculas Peliculas { get; set; }
        public virtual Usuarios Usuarios { get; set; }
    }
}