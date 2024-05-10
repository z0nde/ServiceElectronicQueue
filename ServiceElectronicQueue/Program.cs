using Microsoft.EntityFrameworkCore;
using ServiceElectronicQueue.Models.DataBaseCompany;

var builder = WebApplication.CreateBuilder(args);

string connectionString = builder.Configuration.GetConnectionString("ConnectionCompanyDb") ?? 
                          throw new InvalidOperationException("Строка подключения ''ConnectionCompanyDB'' не найдена");
builder.Services.AddDbContext<CompanyDbContext>(optionsAction => 
    optionsAction.UseNpgsql(connectionString));

// Add services to the container.
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

app.MapControllerRoute(name: "default", pattern: "{controller=UserAuth}/{action=UserRegister}/{id?}");

app.Run();