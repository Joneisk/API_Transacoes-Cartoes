#region Serilog

using System.Reflection;
using System.Text;
using APICARTOES.Models;
using APICARTOES.Repository;
using APICARTOES.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Serilog;
using Serilog.Formatting.Compact;


var logFolder = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "logs");
Directory.CreateDirectory(logFolder);

Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Error()

    .WriteTo.File(new CompactJsonFormatter(),
           Path.Combine(logFolder, ".json"),
            retainedFileCountLimit: 7,
            rollingInterval: RollingInterval.Day)

    .WriteTo.File(Path.Combine(logFolder, ".log"),
            retainedFileCountLimit: 7,
            rollingInterval: RollingInterval.Day)
    .CreateLogger();

#endregion


try
{

    var builder = WebApplication.CreateBuilder(args);

    #region Lendo as configura��es do projeto

    var env = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
    string pathAppSettings = "appsettings.json";

    if (env == "Development")
    {
        pathAppSettings = "appsettings.Development.json";
    }

    var config = new ConfigurationBuilder()
        .SetBasePath(Directory.GetCurrentDirectory())
        .AddJsonFile(pathAppSettings)
        .Build();

    var appSettings = config.Get<AppSettings>();

    //Registra a inst�ncia como Singleton
    builder.Services.AddSingleton(appSettings);

    #endregion

    //***Adicionar o Middleware do Swagger
    builder.Services.AddSwaggerGen(c =>
    {
        c.SwaggerDoc("v1", new OpenApiInfo
        {
            Title = "Gerenciamento da API...",
            Version = "v1",
            Description = $@"<h3>T�tulo <b>da API</b></h3>
                            <p>
                                Alguma descri��o....
                            </p>",
            Contact = new OpenApiContact
            {
                Name = "Suporte Unoeste",
                Email = string.Empty,
                Url = new Uri("https://www.unoeste.br"),
            },
        });



        // ----------------------add Bearer Authentication (apenas se usando autentica��o)
        var securityScheme = new OpenApiSecurityScheme
        {
            Name = "Authorization",
            Description = "<b>Informe dentro do campo a palavra \"Bearer\" segundo por espa�o e o APIKEY. Exemplo: Bearer SDJKF83248923</b>",
            In = ParameterLocation.Header,
            BearerFormat = "JWT",
            Type = SecuritySchemeType.ApiKey,
            Reference = new OpenApiReference
            {
                Id = "Bearer",
                Type = ReferenceType.SecurityScheme
            }
        };

        c.AddSecurityDefinition(securityScheme.Reference.Id, securityScheme);
        c.AddSecurityRequirement(new OpenApiSecurityRequirement
        {
            {securityScheme, new string[] { }},


        });
        //------------------------------------------------------



        // Set the comments path for the Swagger JSON and UI.
        var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
        c.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));


    }); ;

    //Habilitar o uso do serilog.
    builder.Host.UseSerilog();

    // *** Adiciona o Middleware de autentica��o e autoriza��o
    //Estamos falando para o ASP.NET
    //que agora tamb�m queremos verificar o cabe�alho da requisi��o
    //para buscar um Token ou algo do tipo.
    builder.Services
        .AddAuthentication(x =>
        {
            //Especificando o Padr�o do Token

            //para definir que o esquema de autentica��o que queremos utilizar � o Bearer e o
            x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;

            //Diz ao asp.net que utilizamos uma autentica��o interna,
            //ou seja, ela � gerada neste servidor e vale para este servidor apenas.
            //N�o � gerado pelo google/fb
            x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;

        })
        .AddJwtBearer(x =>
        {
            ////Lendo o Token

            //// Obriga uso do HTTPs
            //x.RequireHttpsMetadata = false;

            //// Configura��es para leitura do Token
            //x.TokenValidationParameters = new TokenValidationParameters
            //{
            //    // Chave que usamos para gerar o Token
            //    IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes("minha-chave-secreta-minha-chave-secreta")),
            //    ValidAudience = "Usu�rios da API",
            //    ValidIssuer = "Unoeste",
            //    ValidateLifetime = true, // Expira��o do token
            //    ValidateIssuerSigningKey = true,
            //    ClockSkew = TimeSpan.FromMinutes(5)

            //};
        });



    //pol�tica
    builder.Services.AddAuthorization(options =>
    {
        options.AddPolicy("APIAuth", new AuthorizationPolicyBuilder()
                .AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme)
                .RequireAuthenticatedUser().Build());
    });


    builder.Services.AddHttpContextAccessor();


    // Add services to the container.

    builder.Services.AddControllers();


    builder.Services.AddScoped<CartaoService>();
    builder.Services.AddScoped<CartaoRepository>();
    builder.Services.AddScoped<TransacaoRepository>();
    builder.Services.AddScoped<TransacaoService>();
    builder.Services.AddScoped<MySqlDbContext>();

    //builder.Services.AddTransient //???



    var app = builder.Build();

    // *** Usa o Middleware do Swagger
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
        c.RoutePrefix = ""; //habilitar a p�gina inicial da API ser a doc.
        c.DocumentTitle = "Gerenciamento de Produtos - API V1";
    });


    // *** Usa o Middleware de autentica��o e autoriza��    Fo
    app.UseAuthorization();
    app.UseAuthentication();

    app.MapControllers();

    //int.Parse("fatallll");
    app.Run();

}
catch (Exception ex)
{
    Log.Logger.Fatal(ex, "Erro fatal na aplica��o");
}
