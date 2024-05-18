using BookswormAPI.Configuration;
using BookswormAPI.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers(options =>
{
    options.Filters.Add(new Microsoft.AspNetCore.Mvc.Authorization.AuthorizeFilter());
});


builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
                       ?? throw new InvalidOperationException("Connection string not found");

builder.Services.AddDbContext<ApplicationDbContext>(opt =>
    opt.UseSqlite(connectionString));

// Auth 
builder.Services.AddAuthorization();
builder.Services.AddIdentityApiEndpoints<IdentityUser>()
    .AddEntityFrameworkStores<ApplicationDbContext>();

builder.Services.Configure<AppSettings>(builder.Configuration.GetSection("AppSettings"));
builder.Services.AddCors(opt =>
{
    opt.AddPolicy("AllowOrigin", corsBuilder =>
        corsBuilder.WithOrigins("http://localhost:5173")
            .AllowAnyMethod()
            .AllowAnyHeader()
    );
});

var app = builder.Build();

// Configure the HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseRouting();
app.UseCors("AllowOrigin");
app.UseAuthentication();
app.UseAuthorization();

app.UseEndpoints(endPoints =>
{
    endPoints.MapControllers();
});

//This line of code handles the register,login and so on.
app.MapIdentityApi<IdentityUser>();

app.Run();