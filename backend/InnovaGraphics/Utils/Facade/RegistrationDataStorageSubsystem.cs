using InnovaGraphics.Dtos;
using InnovaGraphics.Extensions;

namespace InnovaGraphics.Utils.Facade
{
    public class RegistrationDataStorageSubsystem
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public RegistrationDataStorageSubsystem(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public Dictionary<string, object> PreparePendingUserData(
           RegisterDto request,
           string password,
           string verificationCode,
           object? groupIdToStore)
        {
            var userData = new Dictionary<string, object>
            {
                { "Email", request.Email },
                { "Password", password },
                { "RealName", request.RealName },
                { "UserName", request.UserName },
                { "Group", groupIdToStore },
                { "IsTeacher", request.IsTeacher },
                { "VerificationCode", verificationCode }
            };
            return userData;
        }

        public void StorePendingRegistrationData(string processId, Dictionary<string, object> data)
        {
            var session = _httpContextAccessor.HttpContext?.Session;
            if (session == null || string.IsNullOrEmpty(processId))
                return;

            session.SetObjectAsJson(processId, data);
        }

        public Dictionary<string, object>? GetPendingRegistrationData(string processId)
        {
            var session = _httpContextAccessor.HttpContext?.Session;
            if (session == null || string.IsNullOrEmpty(processId))
            {
                return null;
            }

            var data = session.GetObjectFromJson<Dictionary<string, object>>(processId);
            return data;
        }

        public void RemovePendingRegistrationData(string processId)
        {
            var session = _httpContextAccessor.HttpContext?.Session;
            if (session == null || string.IsNullOrEmpty(processId))
                return;

            session.Remove(processId);
        }
    }
}
