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
    
    public partial class Devoluciones
    {
        public int ID_Devolucion { get; set; }
        public Nullable<int> ID_Prestamo { get; set; }
        public Nullable<System.DateTime> FechaDevolucion { get; set; }
    
        public virtual Prestamos Prestamos { get; set; }
    }
}
