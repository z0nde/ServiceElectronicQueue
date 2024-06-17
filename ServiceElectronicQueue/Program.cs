using Confluent.Kafka;
using Microsoft.AspNetCore.Components.Forms.Mapping;
using Microsoft.EntityFrameworkCore;
using ServiceElectronicQueue.Models.DataBaseCompany;
using ServiceElectronicQueue.Models.DataBaseCompany.Patterns;
using ServiceElectronicQueue.Models.KafkaQueue;/*
using ServiceElectronicQueue.Models.KafkaQueue.SignalrKafka;*/

var builder = WebApplication.CreateBuilder(args);

string connectionString = builder.Configuration.GetConnectionString("ConnectionCompanyDb") ?? 
                          throw new InvalidOperationException("Строка подключения ''ConnectionCompanyDB'' не найдена");
builder.Services.AddDbContext<CompanyDbContext>(optionsAction => 
    optionsAction.UseNpgsql(connectionString));

/*builder.Services.AddSignalR();*/

// Добавление сессий в сервисы
builder.Services.AddSession();

// Add services to the container.
builder.Services.AddControllersWithViews();

// Добавление средства доступа к контексту Http
builder.Services.AddHttpContextAccessor();

var app = builder.Build();

// Использование сессий
app.UseSession();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

/*app.MapHub<KafkaMessageHub>("/KafkaMessageHubBrOffice");*/

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(name: "default", pattern: "{controller=UserAuth}/{action=UserRegister}/{id?}");

app.Run();