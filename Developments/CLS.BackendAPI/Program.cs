using Microsoft.EntityFrameworkCore;
using CLS.BackendAPI.Data;
using CLS.BackendAPI.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Đăng ký ApplicationDbContext (Dùng chuỗi kết nối chuẩn trong appsettings, ở đây dùng tạm dummy cho MVP)
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection") 
                         ?? "Server=(localdb)\\mssqllocaldb;Database=CLS_DB;Trusted_Connection=True;"));

// Đăng ký Services
builder.Services.AddScoped<ILearnerService, LearnerService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// Use Custom Global Error Handling
app.UseMiddleware<CLS.BackendAPI.Middlewares.GlobalExceptionHandlingMiddleware>();

app.UseAuthorization();

app.MapControllers();

app.Run();
