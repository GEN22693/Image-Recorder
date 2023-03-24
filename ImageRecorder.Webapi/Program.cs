using Microsoft.AspNetCore.Builder;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();
// Leitet http auf https weiter (http Port 5000 auf https Port 5001)
app.UseHttpsRedirection();
app.UseStaticFiles();
app.MapFallbackToFile("index.html");
app.Run();



