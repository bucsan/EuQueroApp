namespace EuQueroApp.Apresentacao.Produtos;

public record ProdutoRequest(string Nome, Guid CategoriaId, string Descricao, bool EmEstoque, bool Ativo);
