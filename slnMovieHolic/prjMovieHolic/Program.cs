using Microsoft.EntityFrameworkCore;
using prjMovieHolic.Models;
using System.Text.Encodings.Web;
using System.Text.Unicode;
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddMvc().AddJsonOptions(options =>
{

    options.JsonSerializerOptions.Encoder =
        JavaScriptEncoder.Create(UnicodeRanges.All);
});
builder.Services.AddDbContext<MovieContext>(
    options => options.UseSqlServer(
        builder.Configuration.GetConnectionString("MovieConnection")));

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
    pattern: "{controller=Order}/{action=ListSession}/{movieid=8}");

app.Run();
