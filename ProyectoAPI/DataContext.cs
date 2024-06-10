using Microsoft.EntityFrameworkCore;

namespace ProyectoAPI
{
    public class DataContext : DbContext
    {
        public DbSet<Producto> Productos { get; set; }
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        { }
            public DbSet<Producto> Producto {  get; set; }
        public DbSet<Cliente> Cliente  { get; set; }
        public DbSet<Compra> Compra { get; set; }
        public DbSet<Proveedor> Proveedor { get; set; }
        public DbSet<Usuario> Usuario { get; set; }
        public DbSet<Venta> Venta { get; set; }

    }
}
 

