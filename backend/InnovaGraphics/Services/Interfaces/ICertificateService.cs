using InnovaGraphics.Interactions;

namespace InnovaGraphics.Services.Interfaces
{
    public interface ICertificateService
    {
        Task<Response> GetTemplateImageUrl(string userId);
    }
}
