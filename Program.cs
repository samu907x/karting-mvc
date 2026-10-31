var builder = WebApplication.CreateBuilder(args);

// Registra los servicios de MVC (controladores y vistas)
builder.Services.AddControllersWithViews();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}
else
{
    app.UseHttpsRedirection();
}

app.UseStaticFiles();   // sirve los archivos de wwwroot (CSS, JS, librerías)
app.UseRouting();

// Ruta por defecto: /{controlador}/{acción}/{id?}
// Si no se indica nada, se usa Home/Index
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
