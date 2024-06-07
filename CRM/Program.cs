using CRM;
using CRM.DbInitialize;
using CRM.Repository.IRepository;
using CRM.Repository;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;
using Microsoft.AspNetCore.ResponseCompression;
using CRM.Hubs;
using Microsoft.Extensions.FileProviders;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers().AddNewtonsoftJson();
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultSQLConnection"))
    );

builder.Services.AddScoped<IActivityStatusRepository, ActivityStatusRepository>();
builder.Services.AddScoped<ICallResultRepository, CallResultRepository>();
builder.Services.AddScoped<ICallStatusRepository, CallStatusRepository>();
builder.Services.AddScoped<ICityRepository, CityRepository>();
builder.Services.AddScoped<ICountryRepository, CountryRepository>();
builder.Services.AddScoped<ICompanyRepository, CompanyRepository>();
builder.Services.AddScoped<IDbinitializer, DBInitializer>();
builder.Services.AddScoped<IDiscountScheduleRepository, DiscountScheduleRepository>();
builder.Services.AddScoped<IEmployeeRepository, EmployeeRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IRoleRepository, RoleRepository>();
builder.Services.AddScoped<IBranchRepository, BranchRepository>();
builder.Services.AddScoped<ILeadRepository, LeadRepository>();
builder.Services.AddScoped<IOTPRepository, OTPRepository>();
builder.Services.AddScoped<IMailServiceRepository, MailServiceRepository>();
builder.Services.AddScoped<IUserActivationTokenRepository, UserActivationTokenRepository>();
builder.Services.AddScoped<ILeadNoteRepository, LeadNoteRepository>();
builder.Services.AddScoped<ILeadCallRepository, LeadCallRepository>();
builder.Services.AddScoped<ILeadMeetingRepository, LeadMeetingRepository>();
builder.Services.AddScoped<ILeadStatusRepository, LeadStatusRepository>();
builder.Services.AddScoped<ILeadTaskRepository, LeadTaskRepository>();
builder.Services.AddScoped<ILeadTaskStatusRepository, LeadTaskStatusRepository>();
builder.Services.AddScoped<IMeetingParticipentRepository, MeetingParticipentRepository>();
builder.Services.AddScoped<IPriorityStatusRepository, PriorityStatusRepository>();
builder.Services.AddScoped<IReminderRepository, ReminderRepository>();
builder.Services.AddScoped<IRoleRepository, RoleRepository>();
builder.Services.AddScoped<IStateRepository, StateRepository>();
builder.Services.AddScoped<ISubscriptionTypeRepository, SubscriptionTypeRepository>();
builder.Services.AddScoped<ISubscriptionFeatureRepository, SubscriptionFeatureRepository>();
builder.Services.AddAutoMapper(typeof(MappingConfig));

/*builder.Services.AddResponseCompression(options =>
{
    options.MimeTypes = ResponseCompressionDefaults.MimeTypes.Concat(
        new[] {"application/octet-stream"});
});*/
    
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(2);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});


var key = builder.Configuration.GetValue<string>("JTWSettings:SecretKey");
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    options.RequireHttpsMetadata = true;
    options.SaveToken = true; ;
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key)),
        ValidateIssuer = false,
        ValidateAudience = false
    };
});

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description =
            "JWT Authorization header using the Bearer scheme. \r\n\r\n " +
            "Enter 'Bearer' [space] and then your token in the text input below.\r\n\r\n" +
            "Example: \"Bearer 12345abcdef\"",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Scheme = "Bearer"
    });
    options.AddSecurityRequirement(new OpenApiSecurityRequirement()
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            },
                Scheme = "oauth2",
                Name = "Bearer",
                In = ParameterLocation.Header
            },
            new List<string>()
        }
    });
});

var  MyAllowSpecificOrigins = "_myAllowSpecificOrigins";

builder.Services.AddCors(options =>
{
    options.AddPolicy(name: MyAllowSpecificOrigins,
                      builder =>
                      {
                          builder.WithOrigins("http://localhost:4200","http://192.168.29.106:4200").AllowAnyMethod().AllowAnyHeader();
                      });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseSession();

app.UseCors(MyAllowSpecificOrigins);

app.UseStaticFiles(new StaticFileOptions
{
    FileProvider  = new PhysicalFileProvider(Path.Combine(builder.Environment.ContentRootPath,"Storage")),
    RequestPath = "/Storage"
});

app.UseAuthentication();
app.UseAuthorization();

SeedDatabse();

app.MapControllers();

app.Run();


void SeedDatabse()
{
    using (var scop = app.Services.CreateScope())
    {
        var DBInitialiser = scop.ServiceProvider.GetRequiredService<IDbinitializer>();
        DBInitialiser.Initialize();
    }
}
