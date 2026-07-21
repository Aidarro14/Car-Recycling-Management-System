using CarRecyclingWeb.Data;
using Microsoft.EntityFrameworkCore; // ОБЯЗАТЕЛЬНО для UseMySql
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var builder = WebApplication.CreateBuilder(args);

// Добавление контекста базы данных с MySQL
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseMySql(
        builder.Configuration.GetConnectionString("DefaultConnection"),
        new MySqlServerVersion(new Version(8, 0, 33)) // укажи нужную тебе версию
    )
);

builder.Services.AddAuthentication("MyCookieAuth").AddCookie("MyCookieAuth", options =>
{
    options.LoginPath = "/Client/Login";  // куда перенаправлять, если не авторизован
});

// Добавление поддержки контроллеров с представлениями
builder.Services.AddControllersWithViews();

var app = builder.Build();

// НОВЫЙ БЛОК: Вызов SeedData (ВСТАВИТЬ СЮДА)
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        var context = services.GetRequiredService<AppDbContext>();
        // Применяем незавершенные миграции. Это важно, чтобы SeedData работал с актуальной схемой.
        context.Database.Migrate();
        // Вызываем ваш метод засеивания данных
        context.SeedData();
    }
    catch (Exception ex)
    {
        var logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "An error occurred seeding the DB.");
    }
}
// КОНЕЦ НОВОГО БЛОКА

// Конфигурация HTTP-конвейера
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}
app.UseStaticFiles();

app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();


app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();