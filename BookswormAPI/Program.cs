using System.Text;
using BookswormAPI.Configuration;
using BookswormAPI.Data;
using BookswormAPI.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

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
builder.Services.AddScoped<DatabaseInit>();
builder.Services.AddTransient<ExternalApiService>();
builder.Services.AddHttpClient();
builder.Services.AddTransient<DataSeed>();

builder.Services.Configure<AppSettings>(builder.Configuration.GetSection("AppSettings"));
builder.Services.Configure<NewYorkTimesApi>(builder.Configuration.GetSection("NewYorkTimesApi"));
builder.Services.AddCors(opt =>
{
    opt.AddPolicy("AllowOrigin", corsBuilder =>
        corsBuilder.WithOrigins("http://localhost:5173")
            .AllowAnyMethod()
            .AllowAnyHeader()
    );
});

builder.Services.AddAuthentication(opt =>
    {
        opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    })
    .AddJwtBearer(opt =>
    {
        opt.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["AppSettings:TokenIssuer"],
            ValidAudience = builder.Configuration["AppSettings:TokenIssuer"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["AppSettings:Secret"]))
        };

    });

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var databaseInit = scope.ServiceProvider.GetRequiredService<DatabaseInit>();
    await databaseInit.DataSeedingAsync();
}

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