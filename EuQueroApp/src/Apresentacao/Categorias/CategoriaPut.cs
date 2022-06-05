﻿namespace EuQueroApp.Apresentacao.Categorias;

public class CategoriaPut
{
    public static string Template => "/categorias/{id:guid}";
    public static string[] Methods => new string[] { HttpMethod.Put.ToString() };
    public static Delegate Handle => Action;

    [Authorize(Policy = "UsuarioPolicy")]
    public static async Task<IResult> Action(
        [FromRoute] Guid id, 
        CategoriaRequest categoriaRequest, 
        HttpContext http, 
        ApplicationDbContext context)
    {
        var userId = http.User.Claims.First(c => c.Type == ClaimTypes.NameIdentifier).Value;
        var categoria = await context.Categorias.Where(c => c.Id == id).FirstOrDefaultAsync();

        if (categoria == null)
            return Results.NotFound();

        categoria.Atualizar(categoriaRequest.Nome, categoriaRequest.Ativo, userId);

        if (!categoria.IsValid)
            return Results.ValidationProblem(categoria.Notifications.ConvertToProblemDetails());

        await context.SaveChangesAsync();

        return Results.Ok();
    }
}
