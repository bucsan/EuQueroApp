using EuQueroApp.Dominio.Produtos;
using EuQueroApp.Infraestrutura.Dados;
using Microsoft.AspNetCore.Mvc;

namespace EuQueroApp.Apresentacao.Categorias;

public class CategoriaPut
{
    public static string Template => "/categorias/{id:guid}";
    public static string[] Methods => new string[] { HttpMethod.Put.ToString() };
    public static Delegate Handle => Action;

    public static IResult Action([FromRoute] Guid id, CategoriaRequest categoriaRequest, ApplicationDbContext context)
    {
        var categoria = context.Categorias.Where(c => c.Id == id).FirstOrDefault();

        if (categoria == null)
            return Results.NotFound();

        categoria.Atualizar(categoriaRequest.Nome, categoriaRequest.Ativo);

        if (!categoria.IsValid)
            return Results.ValidationProblem(categoria.Notifications.ConvertToProblemDetails());

        context.SaveChanges();

        return Results.Ok();
    }
}
