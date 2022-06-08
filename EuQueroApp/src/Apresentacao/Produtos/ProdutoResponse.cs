namespace EuQueroApp.Apresentacao.Produtos;

public record ProdutoResponse(Guid id, string Nome, string CategoriaNome, string Descricao, bool EmEstoque, decimal Preco, bool Ativo);
