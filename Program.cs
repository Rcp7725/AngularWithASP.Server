using AngularWithASP.Server.Core;
using AngularWithASP.Server.Extensions;
using Microsoft.AspNetCore.HttpOverrides;
using System.Text.Json;


// 1. Configurazione iniziale e creazione del builder
var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

// 2. Aggiunta di servizi al contenitore di Dependency Injection (DI)
// Esempio per MVC, Razor Pages, o API
// builder.Services.AddControllersWithViews(); 
// builder.Services.AddRazorPages();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Aggiunge i servizi per i controller
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase; // camelCase per Angular
        options.JsonSerializerOptions.WriteIndented = true; // JSON leggibile
    });

// 3. Costruzione dell'applicazione
var app = builder.Build();

app.UseCors("AllowAll");

// 4. Configurazione della pipeline di richiesta HTTP (Middleware)
// L'ordine di queste chiamate è fondamentale
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

//app.UseHttpsRedirection();
// app.UseStaticFiles(); // Per servire file statici (es. CSS, JS)
app.UseRouting(); // Per il routing

//app.UseAuthorization();

// app.MapRazorPages(); // Esempio per Razor Pages
// app.MapControllerRoute( ... ); // Esempio per MVC
//app.MapGet("/", () => "Hello World!"); // Esempio per Minimal API
app.MapControllers();

// 5. Avvio dell'applicazione
app.Run();


//var builder = WebApplication.CreateBuilder(args);

////aggiunge gli attributi authentication su i controller
//builder.AddBusinessControllerAuthorization();

//// Aggiungi i servizi necessari
//builder.Services.AddControllers();
//// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
//builder.Services.AddEndpointsApiExplorer();

//builder.Services.AddSwaggerGen(); builder.Services.Configure<ForwardedHeadersOptions>(options =>
//{
//    options.ForwardedHeaders =
//        ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto;
//});

//var app = builder.Build();

//app.UseForwardedHeaders(); // Aggiungi prima di altri middleware

////abilita websocket
//app.UseWebSockets();

////if (builder.Environment.EnvironmentName.StartsWith("Development"))
////{
////    app.UseWebAssemblyDebugging();
////}
////else
////{
////    app.UseExceptionHandler("/Error");
////    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
////    app.UseHsts();
////}

//app.UseDefaultFiles();
//app.UseStaticFiles();

//// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
//{
//    app.UseSwagger();
//    app.UseSwaggerUI();
//    // Disabilita la validazione dei certificati SSL (solo per sviluppo)
//    AppContext.SetSwitch("System.Net.Http.SocketsHttpHandler.Http2UnencryptedSupport", true);

//    // end disabilita la validazione dei certificati SSL
//    app.UseHttpsRedirection();
//}

//app.UseAuthorization();

//app.UseRouting();
//app.UseEndpoints(endpoints =>
//{
//    endpoints.MapControllers();
//});

//app.MapFallbackToFile("/index.html");

//app.Lifetime.ApplicationStarted.Register(() =>
//{
//    //risolve la classe di bootstrap e avvia i componenti attivi
//    //non rimango in attesa perchè non necessario
//    app.Services.GetService<AngularWithASP.Server.Bootstrap>()?.StartAsync();
//    //app.Services.GetService<ISTS.BL.Pis.Core.ToCore.Event.PaZonesStatusHandler>()?.StartAsync();
//    //var o = app.Services.GetService<IstsCoreClientOptions>();
//    //var s = app.Services.GetService<IstsCorePisClient>();
//    //var h = app.Services.GetService<IstsCorePisHandler>();
//    //var r = app.Services.GetService< SetThresholdAction>();

//});

//app.Run();
