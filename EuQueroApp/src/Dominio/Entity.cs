using Flunt.Notifications;

namespace EuQueroApp.Dominio;

public abstract class Entity : Notifiable<Notification>
{
    public Entity()
    {
        Id = Guid.NewGuid();
    }

    public Guid Id { get; set; }
    public string Nome { get; set; }
    public string CriadoPor { get; set; }   
    public DateTime DataCriacao { get; set; }
    public string AtualizadoPor { get; set; }
    public DateTime DataAtualizacao { get; set; }
}
