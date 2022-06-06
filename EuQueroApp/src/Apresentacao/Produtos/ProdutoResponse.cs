namespace EuQueroApp.Apresentacao.Produtos;

public record ProdutoResponse(string Nome, string CategoriaNome, string Descricao, bool EmEstoque, bool Ativo);
