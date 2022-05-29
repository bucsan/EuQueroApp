using EuQueroApp.Dominio.Produtos;
using EuQueroApp.Infraestrutura.Dados;
using Microsoft.AspNetCore.Mvc;

namespace EuQueroApp.Apresentacao.Categorias;

public class CategoriaPut
{
    public static string Template => "/categorias/{id}";
    public static string[] Methods => new string[] { HttpMethod.Put.ToString() };
    public static Delegate Handle => Action;

    public static IResult Action([FromRoute] Guid id, CategoriaRequest categoriaRequest, ApplicationDbContext context)
    {
        var categoria = context.Categorias.Where(c => c.Id == id).FirstOrDefault();
        categoria.Nome = categoriaRequest.Nome;
        categoria.Ativo = categoriaRequest.Ativo;

        context.SaveChanges();

        return Results.Ok();
    }
}
