using OCP.API.Data;
using OCP.API.Repositories;
using OCP.API.Services.Audit;
using OCP.API.Services.Auth;
using OCP.API.Services.Cart;
using OCP.API.Services.Catalog;
using OCP.API.Services.Comm;
using OCP.API.Services.Geo;
using OCP.API.Services.Meta;
using OCP.API.Services.Navigation;
using OCP.API.Services.Orders;
using OCP.API.Services.Pricing;
using OCP.API.Services.Security;
using OCP.API.Services.Stores;
using OCP.API.Services.Users;

var builder = WebApplication.CreateBuilder(args);

Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .CreateLogger();

builder.Host.UseSerilog();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo { Title = "OCP.API", Version = "v1" });
    var bearerScheme = new OpenApiSecurityScheme
    {
        Description = "Please enter JWT prefixed 'Bearer' into field",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "Bearer" }
    };
    options.AddSecurityDefinition("Bearer",
        new OpenApiSecurityScheme
        {
            Description = "Please enter JWT prefixed 'Bearer' into field",
            Name = "Authorization",
            In = ParameterLocation.Header,
            Type = SecuritySchemeType.ApiKey
        });
    options.AddSecurityRequirement(new OpenApiSecurityRequirement { { bearerScheme, Array.Empty<string>() } });
});

// Read connection string from config
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

// Register DbContext from external library
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(connectionString));

// JWT Auth
var jwtKey = builder.Configuration["Jwt:Key"] ?? string.Empty;
var jwtIssuer = builder.Configuration["Jwt:Issuer"] ?? "OCP.API";
var jwtAudience = builder.Configuration["Jwt:Audience"] ?? "OCP.API.Clients";
var signingKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey));

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    options.RequireHttpsMetadata = false;
    options.SaveToken = true;
    options.MapInboundClaims = false;
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = false,
        ValidateAudience = false,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = jwtIssuer,
        ValidAudience = jwtAudience,
        IssuerSigningKey = signingKey,
        ClockSkew = TimeSpan.FromMinutes(1)
    };
    options.Events = new JwtBearerEvents
    {
        OnTokenValidated = context =>
        {
            var identity = context.Principal?.Identity as ClaimsIdentity;
            identity?.AddClaim(new Claim(ClaimTypes.NameIdentifier,
                identity.FindFirst("sub")?.Value ?? string.Empty));

            return Task.CompletedTask;
        }
    };
});

// register repositories and services
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IUsersService, UsersService>();
builder.Services.AddScoped<IStoreRepository, StoreRepository>();
builder.Services.AddScoped<IStoresService, StoresService>();
builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
builder.Services.AddScoped<ICategoriesService, CategoriesService>();
builder.Services.AddScoped<IItemRepository, ItemRepository>();
builder.Services.AddScoped<IItemsService, ItemsService>();
builder.Services.AddScoped<ISubCategoryRepository, SubCategoryRepository>();
builder.Services.AddScoped<ISubCategoriesService, SubCategoriesService>();
builder.Services.AddScoped<IUnitRepository, UnitRepository>();
builder.Services.AddScoped<IUnitsService, UnitsService>();
builder.Services.AddScoped<ICartRepository, CartRepository>();
builder.Services.AddScoped<ICartService, CartService>();
builder.Services.AddScoped<IDiscountRepository, DiscountRepository>();
builder.Services.AddScoped<IDiscountsService, DiscountsService>();
builder.Services.AddScoped<IPincodeRepository, PincodeRepository>();
builder.Services.AddScoped<IPincodesService, PincodesService>();
builder.Services.AddScoped<IPhoneRepository, PhoneRepository>();
builder.Services.AddScoped<IPhonesService, PhonesService>();
builder.Services.AddScoped<INavigationRepository, NavigationRepository>();
builder.Services.AddScoped<INavigationService, NavigationService>();
builder.Services.AddScoped<IRoleRepository, RoleRepository>();
builder.Services.AddScoped<IRolesService, RolesService>();
builder.Services.AddScoped<IAuditRepository, AuditRepository>();
builder.Services.AddScoped<IAuditService, AuditService>();
builder.Services.AddScoped<IStatusRepository, StatusRepository>();
builder.Services.AddScoped<IStatusService, StatusService>();
builder.Services.AddScoped<IOrderRepository, OrderRepository>();
builder.Services.AddScoped<IOrdersService, OrdersService>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
