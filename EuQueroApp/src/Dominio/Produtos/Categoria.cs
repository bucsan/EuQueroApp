namespace EuQueroApp.Dominio.Produtos;

public class Categoria : Entity
{
    public string Nome { get; private set; }
    public bool Ativo { get; private set; }

    public Categoria(string nome, string criadoPor, string atualizadoPor)
    {
        Nome = nome;
        Ativo = true;
        CriadoPor = criadoPor;
        AtualizadoPor = atualizadoPor;
        DataCriacao = DateTime.Now;
        DataAtualizacao = DateTime.Now;

        Validate();
    }

    private void Validate()
    {
        var contrato = new Contract<Categoria>()
                    .IsNotNullOrEmpty(Nome, "Nome", "Nome é obrigatório!")
                    .IsGreaterOrEqualsThan(Nome, 3, "Nome", "Nome tem que ser maior que 3 caracteres!")
                    .IsNotNullOrEmpty(CriadoPor, "CriadoPor", "CriadoPor é obrigatório!")
                    .IsNotNullOrEmpty(AtualizadoPor, "AtualizadoPor", "AtualizadoPor é obrigatório!");
        AddNotifications(contrato);
    }

    public void Atualizar(string nome, bool ativo, string atualizadoPor)
    {
        Ativo = ativo;
        Nome = nome;
        AtualizadoPor = atualizadoPor;
        DataAtualizacao = DateTime.Now;

        Validate();
    }
}
