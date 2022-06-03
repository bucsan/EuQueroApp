using EuQueroApp.Apresentacao.Categorias;
using EuQueroApp.Apresentacao.Funcionarios;
using EuQueroApp.Infraestrutura.Dados;
using Microsoft.AspNetCore.Identity;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddSqlServer<ApplicationDbContext>(builder.Configuration["ConnectionString:EuQueroDb"]);
builder.Services.AddIdentity<IdentityUser, IdentityRole>(options => 
{
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireDigit = false;
    options.Password.RequireUppercase = false;
    options.Password.RequireLowercase = false;
    options.Password.RequiredLength = 3;
}).AddEntityFrameworkStores<ApplicationDbContext>();
builder.Services.AddScoped<QueryObterFuncionariosClaimNome>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapMethods(CategoriaPost.Template, CategoriaPost.Methods, CategoriaPost.Handle);
app.MapMethods(CategoriaGetAll.Template, CategoriaGetAll.Methods, CategoriaGetAll.Handle);
app.MapMethods(CategoriaPut.Template, CategoriaPut.Methods, CategoriaPut.Handle);

app.MapMethods(FuncionarioPost.Template, FuncionarioPost.Methods, FuncionarioPost.Handle);
app.MapMethods(FuncionarioGetAll.Template, FuncionarioGetAll.Methods, FuncionarioGetAll.Handle);

app.Run();
