using InnovaGraphics.Data;
using InnovaGraphics.Interactions;
using InnovaGraphics.Models;
using InnovaGraphics.Repositories.Implementations;
using InnovaGraphics.Repositories.Interfaces;
using InnovaGraphics.Repositories.Interfaces.InnovaGraphics.Repositories.Interfaces;
using InnovaGraphics.Services.Implementations;
using InnovaGraphics.Services.Interfaces;
using InnovaGraphics.Utils;
using InnovaGraphics.Utils.Builder;
using InnovaGraphics.Utils.Facade;
using InnovaGraphics.Utils.Strategy.Interfaces;
using InnovaGraphics.Utils.Strategy.Lab1.Implementations;
using InnovaGraphics.Utils.Strategy.Lab2.Implementations;
using InnovaGraphics.Utils.Strategy.Lab3.Implementations;
using InnovaGraphics.Utils.Strategy.Lab4.Implementations;
using InnovaGraphics.Utils.Strategy.Lab5.Implementations;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using System.Text;
using Newtonsoft.Json;
using InnovaGraphics.Dtos;
using InnovaGraphics.Utils.Validators.Implementations;
using InnovaGraphics.Utils.Validators.Interfaces;
using InnovaGraphics.Utils.Mediator.Interfaces;
using InnovaGraphics.Utils.Mediator.Implementations;
using InnovaGraphics.Utils.Factory.Implementations;
using InnovaGraphics.Utils.Factory.Interfaces;
using InnovaGraphics.Utils.Factory;
using InnovaGraphics.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddControllers()
    .AddNewtonsoftJson(options =>
    {
        options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore; 
    }); 

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSignalR();

builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("Default"));
});

builder.Services.AddIdentity<User, IdentityRole>()
    .AddEntityFrameworkStores<AppDbContext>()
    .AddDefaultTokenProviders()
    .AddErrorDescriber<LocalSubsystem>();


//Materials API and Libraries
builder.Services.Configure<YouTubeSettings>(
    builder.Configuration.GetSection("YouTube"));

builder.Services.AddHttpClient<HtmlMetadataFetcher>();



//Репозиторії
builder.Services.AddScoped<IUserRepository, UserRepository>();
//builder.Services.AddScoped<IRepository<Group>, GroupRepository>();
builder.Services.AddScoped<ITeacherRepository, TeacherRepository>();
builder.Services.AddScoped<IUserTestRepository, UserTestRepository>();
builder.Services.AddScoped<ICertificateRepository, CertificateRepository>();
builder.Services.AddScoped<IPurchaseRepository, PurchaseRepository>();
builder.Services.AddScoped<IExerciseRepository, ExerciseRepository>();
builder.Services.AddScoped<IUserPlanetRepository, UserPlanetRepository>();
builder.Services.AddScoped<ITheoryRepository, TheoryRepository>();
builder.Services.AddScoped<IRefreshTokenRepository, RefreshTokenRepository>();
builder.Services.AddScoped(typeof(IRepository<>), typeof(BaseRepository<>));
builder.Services.AddScoped<ITokenManagerRepository, TokenManagerRepository>();
builder.Services.AddScoped<ITestRepository, TestRepository>();
builder.Services.AddScoped<IPlanetRepository, PlanetRepository>();
builder.Services.AddScoped<IQuestionRepository, QuestionRepository>();
builder.Services.AddScoped<IAnswerRepository, AnswerRepository>();
builder.Services.AddScoped<IMaterialRepository, MaterialRepository>();
builder.Services.AddScoped<IShopItemRepository, ShopItemRepository>();
builder.Services.AddScoped<IHintRepository, HintRepository>();
builder.Services.AddScoped<IBackgroundRepository, BackgroundRepository>();
builder.Services.AddScoped<IRepository<Purchase>, PurchaseRepository>();


//Сервіси
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddHostedService<ExpiredTokenCleanupService>();
builder.Services.AddScoped<ICodeTest<IEnumerable<Case>, TestResult>, BezierParametricTest>();
builder.Services.AddScoped<ICodeTest<IEnumerable<Case>, TestResult>, BezierMatrixTest>();
builder.Services.AddScoped<ICodeTest<IEnumerable<Case>, TestResult>, BezierRecursiveTest>();
builder.Services.AddScoped<ICodeTest<IEnumerable<Case>, TestResult>, FractalTest>();
builder.Services.AddScoped<ICodeTest<IEnumerable<Case>, TestResult>, AffineTest>();
builder.Services.AddScoped<ICodeTest<IEnumerable<Case>, TestResult>, RGBtoHSBTest>();
builder.Services.AddScoped<ICodeTest<IEnumerable<Case>, TestResult>, TriangleTest>();
builder.Services.AddScoped<ICodeTestService, CodeTestService>();
builder.Services.AddScoped<ICaseRepository, CaseRepository>();
builder.Services.AddScoped<ICodeExecutor<CodeRequest, CodeResponse>, CodeExecutorLab1>();
builder.Services.AddScoped<ICodeExecutor<CodeRequest, CodeResponse>, CodeExecutorLab2>();
builder.Services.AddScoped<ICodeExecutor<CodeRequest, CodeResponse>, CodeExecutorLab3>();
builder.Services.AddScoped<ICodeExecutor<CodeRequest4, CodeResponse4>, CodeExecutorLab4>();
builder.Services.AddSingleton<BattleService>();
builder.Services.AddScoped<IUserPlanetService, UserPlanetService>();
builder.Services.AddScoped<ICertificateService, CertificateService>();
builder.Services.AddScoped<ITheoryService, TheoryService>();
builder.Services.AddScoped<IExerciseService, ExerciseService>();
builder.Services.AddScoped<ICertificateBuilder, CertificateBuilder>(provider =>
{ 
    var configuration = provider.GetRequiredService<IConfiguration>();
    string imagePath = configuration["CertificateTemplatePath"];
    return new CertificateBuilder(imagePath);
});
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<ITestService, TestService>();
builder.Services.AddScoped<IPlanetService, PlanetService>();
builder.Services.AddScoped<ICaseService, CaseService>();
builder.Services.AddScoped<IQuestionService, QuestionService>();
builder.Services.AddScoped<IAnswerService, AnswerService>();
builder.Services.AddScoped<IMaterialService, MaterialService>();
builder.Services.AddScoped<IShopItemService, ShopItemService>();
builder.Services.AddScoped<IAvatarService, AvatarService>();
builder.Services.AddScoped<IHintService, HintService>();
builder.Services.AddScoped<IBackgroundService, InnovaGraphics.Services.Implementations.BackgroundService>();
builder.Services.AddScoped<IPurchaseService, PurchaseService>();

// Реєстрація конкретної реалізації IExistenceChecker для Question
builder.Services.AddScoped<IExistenceChecker<Question, Guid>, QuestionExistenceChecker>();
builder.Services.AddScoped<IExistenceChecker<Test, Guid>, TestExistenceChecker>();
builder.Services.AddScoped<IExistenceChecker<Planet, Guid>, PlanetExistenceChecker>();

//Валідатори
builder.Services.AddScoped<IValidator<CreateQuestionDto, Guid>, CreateQuestionDtoValidator>();
builder.Services.AddScoped<IValidator<CreateAnswerDto, Guid>, CreateAnswerDtoValidator>();
builder.Services.AddScoped<IValidator<Question, Guid>, QuestionValidator>();
builder.Services.AddScoped<IValidator<Answer, Guid>, AnswerValidator>();
builder.Services.AddScoped<IValidator<Test, Guid>, TestValidator>();
builder.Services.AddScoped<IValidator<CreateTestDto, object>, CreateTestDtoValidator>();
builder.Services.AddScoped<IValidator<Material, Guid>, MaterialValidator>();

// Медіатор
builder.Services.AddScoped<IMediator, TestCreationMediator>();
builder.Services.AddScoped<ICodeExecutor<CodeRequest5, CodeResponse5>, CodeExecutorLab5>();



builder.Services.AddMemoryCache();
builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<AuthFacade>();
builder.Services.AddScoped<ValidationSubsystem>();
builder.Services.AddScoped<TokenGenerationSubsystem>();
builder.Services.AddScoped<RegistrationDataStorageSubsystem>();
builder.Services.AddScoped<SendMessageSubsystem>();
builder.Services.AddTransient<CodeExecutorLab2>();
builder.Services.AddTransient<CodeExecutorLab3>();
builder.Services.AddTransient<CodeExecutorLab5>();
builder.Services.AddScoped<CodeTestService>();
builder.Services.AddScoped<HtmlMetadataFetcher>();
builder.Services.AddScoped<YouTubeMetadataFetcher>();
builder.Services.AddScoped<IMaterialMetadataFetcher, YouTubeMetadataFetcher>();
builder.Services.AddScoped<MaterialMetadataFetcherFactory>();

// Додавання кешу для зберігання сесійних даних у пам'яті
builder.Services.AddDistributedMemoryCache();
//Зберігання інформації між http запитами
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(60);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("TeacherPolicy", policy => policy.RequireRole("Teacher"));
    options.AddPolicy("StudentPolicy", policy => policy.RequireRole("Student"));
});

builder.Services.AddCors(options =>
{
    options.AddPolicy(name: "Innova Graphics", configurePolicy: policyBuilder =>
    {
        policyBuilder.WithOrigins("https://localhost:5173", "http://localhost:5173");
        policyBuilder.AllowAnyHeader();
        policyBuilder.AllowAnyMethod();
        policyBuilder.AllowCredentials();
    });
});

// Конфігурації JWT
var jwtSettings = builder.Configuration.GetSection("Jwt");
var key = Encoding.ASCII.GetBytes(jwtSettings["Key"]);
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
    .AddJwtBearer(options =>
    {
        options.RequireHttpsMetadata = false;
        options.SaveToken = true;
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidIssuer = jwtSettings["Issuer"],
            ValidateAudience = true,
            ValidAudience = jwtSettings["Audience"],
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(key)
        };
        //читання токена з кукі
        options.Events = new JwtBearerEvents
        {
            OnMessageReceived = context =>
            {
                if (context.Request.Cookies.ContainsKey("access_token"))
                {
                    context.Token = context.Request.Cookies["access_token"];
                }
                return Task.CompletedTask;
            }
        };
    });

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Innova Graphics", Version = "v1" });
    var securityScheme = new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Description = "Enter JWT Bearer token",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.Http,
        Scheme = "bearer", 
        BearerFormat = "JWT",
        Reference = new OpenApiReference
        {
            Id = "Bearer",
            Type = ReferenceType.SecurityScheme
        }
    };
    c.AddSecurityDefinition("Bearer", securityScheme);
    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] {}
        }
    });
    c.MapType<string>(() => new OpenApiSchema { Example = new OpenApiString(builder.Configuration.GetValue<string>("CertificateTemplateFileName")) });
});


var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}


app.UseCors("Innova Graphics");
app.UseRouting();
app.UseSession();
app.UseStaticFiles();


app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.UseEndpoints(endpoints =>
{
    endpoints.MapHub<BattleHub>("/battleHub");
    endpoints.MapControllers();
});
app.MapControllers();

app.Run();
