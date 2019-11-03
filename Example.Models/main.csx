#r "nuget:Microsoft.EntityFrameworkCore, 3.0.0"
#r "nuget:Microsoft.EntityFrameworkCore.SqlServer, 3.0.0"
#r "bin\Debug\netcoreapp3.0\Example.Models.dll"

using Example.Models;
using Microsoft.EntityFrameworkCore;

//var options = new DbContextOptionsBuilder<MovieContext>().UseSqlite("../Example.WebApp/MvcMovie.db");
var options = new DbContextOptionsBuilder<MovieContext>().UseSqlServer("Server=172.27.98.165;Database=Movie;User Id=SA;Password=P@55w0rd;");
