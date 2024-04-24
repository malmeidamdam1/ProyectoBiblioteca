using System.Data.Entity;

public class LibreriaContext : DbContext
{
    public DbSet<Libro> Libros { get; set; }
    // Agrega otras propiedades DbSet para representar tus tablas aquí



}