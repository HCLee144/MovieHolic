using Microsoft.EntityFrameworkCore;
using prjMovieHolic.Models;
using System.Text.Encodings.Web;
using System.Text.Unicode;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddSession();
builder.Services.AddMvc().AddJsonOptions(options =>
{
    //允許基本拉丁英文及中日韓文字維持原字元
    options.JsonSerializerOptions.Encoder =
        JavaScriptEncoder.Create(UnicodeRanges.All);
});
builder.Services.AddDbContext<MovieContext>(
    options => options.UseSqlServer("name=ConnectionStrings:DefaultConnection"));
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

app.UseSession();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
