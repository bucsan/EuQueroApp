namespace EuQueroApp.Infraestrutura.Dados;

public class ApplicationDbContext : IdentityDbContext<IdentityUser>
{
    public DbSet<Produto> Produtos { get; set; }
    public DbSet<Categoria> Categorias { get; set; }
    public DbSet<Pedido> Pedidos { get; set; }

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.Ignore<Notification>();

        builder.Entity<Produto>()
            .Property(p => p.Nome).IsRequired();
        builder.Entity<Produto>()
            .Property(p => p.Descricao).HasMaxLength(500);
        builder.Entity<Produto>()
           .Property(p => p.Preco).HasColumnType("decimal(10,2)").IsRequired();

        builder.Entity<Categoria>()
            .Property(c => c.Nome).IsRequired();

        builder.Entity<Pedido>()
           .Property(o => o.ClienteId).IsRequired();
        builder.Entity<Pedido>()
           .Property(o => o.EnderecoEntrega).IsRequired();
        builder.Entity<Pedido>()
            .HasMany(o => o.Produtos)
            .WithMany(p => p.Pedidos)
            .UsingEntity(x => x.ToTable("PedidoProdutos"));
    }

    protected override void ConfigureConventions(ModelConfigurationBuilder configuration)
    {
        configuration.Properties<string>()
            .HaveMaxLength(200);
    }
}
