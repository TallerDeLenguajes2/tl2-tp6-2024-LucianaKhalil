var builder = WebApplication.CreateBuilder(args);


// Add services to the container.
builder.Services.AddScoped<IPresupuestosRepository, PresupuestoRepositorio>();//inyectar repositorios
builder.Services.AddScoped<IClienteRepository, ClienteRepositorio>();
builder.Services.AddScoped<IproductoRepository, ProductoRepositorio>();
builder.Services.AddSingleton<IUsuarioRepository, UsuarioRepositorio>();

//inyectar la cadena de conexion
var cadenaDeConexion = builder.Configuration.GetConnectionString("SqliteConexion")!;
builder.Services.AddSingleton<string>(cadenaDeConexion);

var CadenaDeConexion=builder.Configuration.GetConnectionString("SqliteConexion")!.ToString();//inyectar cadena de conexion
builder.Services.AddSingleton<string>(CadenaDeConexion);
builder.Services.AddControllersWithViews();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
