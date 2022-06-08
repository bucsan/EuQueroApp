using EuQueroApp.Dominio.Usuarios;
using Serilog;
using Serilog.Sinks.MSSqlServer;

var builder = WebApplication.CreateBuilder(args);
builder.WebHost.UseSerilog((context, configuration) =>
{
    configuration
        .WriteTo.Console()
        .WriteTo.MSSqlServer(
            context.Configuration["ConnectionString:EuQueroDb"],
            sinkOptions: new MSSqlServerSinkOptions()
            {
                AutoCreateSqlTable = true,
                TableName = "LogApi"
            });
});
builder.Services.AddSqlServer<ApplicationDbContext>(builder.Configuration["ConnectionString:EuQueroDb"]);
builder.Services.AddIdentity<IdentityUser, IdentityRole>(options => 
{
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireDigit = false;
    options.Password.RequireUppercase = false;
    options.Password.RequireLowercase = false;
    options.Password.RequiredLength = 3;
}).AddEntityFrameworkStores<ApplicationDbContext>();

builder.Services.AddAuthorization(options => 
{
    options.FallbackPolicy = new AuthorizationPolicyBuilder()
    .AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme)
    .RequireAuthenticatedUser()
    .Build();
    options.AddPolicy("UsuarioPolicy", p => p.RequireAuthenticatedUser()
                                             .RequireClaim("UsuarioCodigo"));
    options.AddPolicy("Usuario005Policy", p => p.RequireAuthenticatedUser()
                                             .RequireClaim("UsuarioCodigo", "005"));
    options.AddPolicy("CpfPolicy", p => p.RequireAuthenticatedUser()
                                             .RequireClaim("Cpf"));
});
builder.Services.AddAuthentication(x =>
{
    x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters()
    {
        ValidateActor = true,
        ValidateAudience = true,
        ValidateIssuer = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ClockSkew = TimeSpan.Zero,
        ValidIssuer = builder.Configuration["JwtBearerTokenSettings:Issuer"],
        ValidAudience = builder.Configuration["JwtBearerTokenSettings:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JwtBearerTokenSettings:SecretKey"]))
    };
});

/*Serviços*/
builder.Services.AddScoped<QueryObterUsuariosClaimNome>();
builder.Services.AddScoped<UsuariosCriar>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();
app.UseAuthentication();
app.UseAuthorization();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

/*Categorias*/
app.MapMethods(CategoriaPost.Template, CategoriaPost.Methods, CategoriaPost.Handle);
app.MapMethods(CategoriaGetAll.Template, CategoriaGetAll.Methods, CategoriaGetAll.Handle);
app.MapMethods(CategoriaPut.Template, CategoriaPut.Methods, CategoriaPut.Handle);

/*Usuarios*/
app.MapMethods(UsuariosPost.Template, UsuariosPost.Methods, UsuariosPost.Handle);
app.MapMethods(UsuarioGetAll.Template, UsuarioGetAll.Methods, UsuarioGetAll.Handle);

/*Produtos*/
app.MapMethods(ProdutoPost.Template, ProdutoPost.Methods, ProdutoPost.Handle);
app.MapMethods(ProdutoGetAll.Template, ProdutoGetAll.Methods, ProdutoGetAll.Handle);
app.MapMethods(ProdutoGet.Template, ProdutoGet.Methods, ProdutoGet.Handle);
app.MapMethods(ProdutoGetVitrine.Template, ProdutoGetVitrine.Methods, ProdutoGetVitrine.Handle);

/*Clientes*/
app.MapMethods(ClientePost.Template, ClientePost.Methods, ClientePost.Handle);
app.MapMethods(ClienteGet.Template, ClienteGet.Methods, ClienteGet.Handle);

/*Pedidos*/
app.MapMethods(PedidoPost.Template, PedidoPost.Methods, PedidoPost.Handle);
app.MapMethods(PedidoGet.Template, PedidoGet.Methods, PedidoGet.Handle);

/*Token*/
app.MapMethods(TokenPost.Template, TokenPost.Methods, TokenPost.Handle);

/*Filter Error*/
app.UseExceptionHandler("/error");
app.Map("/error", (HttpContext http) =>
{
    var error = http.Features?.Get<IExceptionHandlerFeature>()?.Error;

    if(error != null)
    {
        if (error is SqlException)
            return Results.Problem(title: "Banco de dados offline", statusCode: 500);
        else if (error is BadHttpRequestException)
            return Results.Problem(title: "Erro ao converter o dado para outro tipo. Verifique as infromações enviadas!", statusCode: 500);
        else if(error is JsonException)
            return Results.Problem(title: "Erro ao converter o dado para outro tipo. Verifique as infromações enviadas!", statusCode: 500);
        else if (error is FormatException)
            return Results.Problem(title: "Erro ao converter o dado para outro tipo. Verifique as infromações enviadas!", statusCode: 500);
    }

    return Results.Problem(title: "Ocorreu um erro interno", statusCode: 500);
});

app.Run();
