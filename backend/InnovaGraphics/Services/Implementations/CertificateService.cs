using InnovaGraphics.Repositories.Interfaces;
using InnovaGraphics.Services.Interfaces;
using InnovaGraphics.Interactions;
using InnovaGraphics.Utils.Builder;
using System.Drawing.Imaging;
using InnovaGraphics.Models;

namespace InnovaGraphics.Services.Implementations;

public class CertificateService : ICertificateService
{
    private readonly string _templateFileName;
    private readonly IWebHostEnvironment _environment;
    private readonly string _defaultCertificateDirectory;
    private readonly IUserRepository _userRepository;
    private readonly IUserTestRepository _userTestRepository;
    private readonly ICertificateRepository _certificateRepository;
    private readonly IUserPlanetRepository _userPlanetRepository; 

    public CertificateService(
        IWebHostEnvironment environment,
        IConfiguration configuration,
        IUserRepository userRepository,
        IUserTestRepository userTestRepository,
        ICertificateRepository certificateRepository,
        IUserPlanetRepository userPlanetRepository)
    {
        _environment = environment;
        _templateFileName = configuration.GetValue<string>("CertificateTemplateFileName");
        _userRepository = userRepository;
        _userTestRepository = userTestRepository;
        _defaultCertificateDirectory = "https://localhost:7218";
        _certificateRepository = certificateRepository;
        _userPlanetRepository = userPlanetRepository;
    }

    public async Task<Response> GetTemplateImageUrl(string userId)
    {
        if (string.IsNullOrEmpty(_templateFileName))
        {
            return new Response
            {
                Success = false,
                StatusCode = 500,
                ValidationErrors = new Dictionary<string, List<string>>
                {
                    { "CertificateTemplateFileName", new List<string> { "Назва файлу шаблону сертифіката не налаштована." } }
                }
            };
        }

        var user = await _userRepository.GetByIdAsync(userId);
        if (user == null)
        {
            return new Response
            {
                Success = false,
                StatusCode = 404,
                ValidationErrors = new Dictionary<string, List<string>>
                {
                    { "UserId", new List<string> { $"Користувача з ID '{userId}' не знайдено." } }
                }
            };
        }

        var existingCertificate = await _certificateRepository.GetAsync(c => c.UserId == userId);
        if (existingCertificate != null)
        {
            string existingImageUrl = $"{_defaultCertificateDirectory.TrimEnd('/')}/certificate_{user.RealName}.png"; 
            return new Response
            {
                Success = true,
                StatusCode = 200,
                Data = new { ImageUrl = existingImageUrl },
                Message = "Сертифікат вже існує"
            };
        }

        if (user.MarkCount <= 25)
        {
            return new Response
            {
                Success = false,
                StatusCode = 400,
                ValidationErrors = new Dictionary<string, List<string>>
                {
                    { "MarkCount", new List<string> { "Для отримання сертифіката необхідно мати більше 25 балів." } }
                }
            };
        }

        int connectedPlanetsCount = await _userPlanetRepository.GetUserPlanetCountAsync(userId);

        if (connectedPlanetsCount < 5)
        {
            return new Response
            {
                Success = false,
                StatusCode = 400,
                ValidationErrors = new Dictionary<string, List<string>>
                {
                    { "ConnectedPlanets", new List<string> { "Сертифікат недоступний, поки не приєднано щонайменше до 5 планет." } }
                }
            };
        }

        string wwwrootPath = _environment.WebRootPath;
        string fileName = $"certificate_{user.RealName}.png";
        string filePath = Path.Combine(_environment.WebRootPath, fileName);

        Certificate certificateEntity = null;

        try
        {
            using (var builder = new CertificateBuilder(Path.Combine(wwwrootPath, _templateFileName)))
            {
                builder.DrawName(user.RealName);
                builder.AddDate(DateTime.Now);
                builder.AddLecturer("Левус Є. В.");

                using (var certificateImage = builder.Build())
                {
                    certificateImage.Save(filePath, ImageFormat.Png);

                    certificateEntity = new Certificate
                    {
                        Date = DateOnly.FromDateTime(DateTime.Now),
                        UserId = userId
                    };

                    await _certificateRepository.AddAsync(certificateEntity);
                }
            }
            string imageUrl = $"{_defaultCertificateDirectory.TrimEnd('/')}/{fileName}";

            return new Response
            {
                Success = true,
                StatusCode = 200,
                Data = new { ImageUrl = imageUrl },
                Message = "Сертифікат успішно згенеровано та інформацію збережено в БД"
            };
        }
        catch (Exception ex)
        {
            return new Response
            {
                Success = false,
                StatusCode = 500,
                Message = $"Помилка під час генерації та збереження сертифіката: {ex.Message}"
            };
        }
    }
}
