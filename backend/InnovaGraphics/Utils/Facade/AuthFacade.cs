using InnovaGraphics.Dtos;
using InnovaGraphics.Interactions;
using InnovaGraphics.Models;
using InnovaGraphics.Repositories.Interfaces;
using InnovaGraphics.Repositories.Interfaces.InnovaGraphics.Repositories.Interfaces;
using Microsoft.AspNetCore.Identity;

namespace InnovaGraphics.Utils.Facade
{
    public class AuthFacade
    {
        private readonly IUserRepository _userRepository;
        private readonly IRepository<Group> _groupRepository;
        private readonly IRepository<Profile> _profileRepository;
        private readonly ITeacherRepository _teacherRepository;
        private readonly ITokenManagerRepository _tokenManagerRepository;
        private readonly TokenGenerationSubsystem _tokenGenerationSubsystem;
        private readonly SendMessageSubsystem _sendMessageSubsystem;
        private readonly ValidationSubsystem _validationSubsystem;
        private readonly RegistrationDataStorageSubsystem _registrationDataStorageSubsystem;
        private readonly ILogger<RegistrationDataStorageSubsystem> _logger;
        private readonly IRefreshTokenRepository _refreshTokenRepository;

        public AuthFacade(
            IUserRepository userRepository,
            IRepository<Group> groupRepository,
            IRepository<Profile> profileRepository,
            ITeacherRepository teacherRepository,
            ITokenManagerRepository tokenManagerRepository,
            TokenGenerationSubsystem tokenGenerationSubsystem,
            SendMessageSubsystem sendMessageSubsystem,
            ValidationSubsystem validationSubsystem,
            RegistrationDataStorageSubsystem registrationDataStorageSubsystem,
            ILogger<RegistrationDataStorageSubsystem> logger,
            IRefreshTokenRepository refreshTokenRepository)
        {
            _userRepository = userRepository;
            _groupRepository = groupRepository;
            _profileRepository = profileRepository;
            _teacherRepository = teacherRepository;
            _tokenManagerRepository = tokenManagerRepository;
            _tokenGenerationSubsystem = tokenGenerationSubsystem;
            _sendMessageSubsystem = sendMessageSubsystem;
            _validationSubsystem = validationSubsystem;
            _registrationDataStorageSubsystem = registrationDataStorageSubsystem;
            _logger = logger;
            _refreshTokenRepository = refreshTokenRepository;
        }

        public async Task<Response> CheckPasswordAsync(Models.User user, string password)
        {
            if (user == null || string.IsNullOrEmpty(password))
            {
                return new Response
                {
                    Success = false,
                    StatusCode = 400, 
                    Message = "Пароль не може бути порожніми."
                };
            }

            var passwordIsValid = await _userRepository.CheckPasswordAsync(user, password);

            if (passwordIsValid)
            {
                return new Response
                {
                    Success = true,
                    StatusCode = 200, 
                    Message = "Пароль дійсний."
                };
            }
            else
            {
                return new Response
                {
                    Success = false,
                    StatusCode = 401, 
                    Message = "Недійсний пароль."
                };
            }
        }

        public async Task<(Response Result, string? ProcessId)> RegisterUserAsync(RegisterDto request)
        {
            var validationErrors = new Dictionary<string, List<string>>();
            bool hasErrors = false;

            var existingUserByEmail = await _userRepository.FindByEmailAsync(request.Email);
            if (existingUserByEmail != null && existingUserByEmail.IsActive)
            {
                if (!validationErrors.ContainsKey("email")) validationErrors["email"] = new List<string>();
                validationErrors["email"].Add($"Цея пошта '{request.Email}' вже існує.");
                hasErrors = true;
            }

            var existingUserByUserName = await _userRepository.FindByNameAsync(request.UserName);
            if (existingUserByUserName != null && existingUserByUserName.IsActive)
            {
                if (!validationErrors.ContainsKey("UserName")) validationErrors["UserName"] = new List<string>();
                hasErrors = true;
            }

            object? groupIdForCache = null;
            Group? groupEntity = null;

            if (!request.IsTeacher)
            {
                if (request.Group.HasValue)
                {
                    int nonNullableGroupId = request.Group.Value;
                    groupIdForCache = nonNullableGroupId;
                    groupEntity = await _groupRepository.GetByIdAsync(nonNullableGroupId);
                    if (groupEntity == null)
                    {
                        if (!validationErrors.ContainsKey("Group")) validationErrors["Group"] = new List<string>();
                        validationErrors["group"].Add($"Група з ід '{nonNullableGroupId}' не існує.");
                        hasErrors = true;
                    }
                }
                else
                {
                    if (!validationErrors.ContainsKey("group")) validationErrors["group"] = new List<string>();
                    validationErrors["group"].Add("Ідентифікатор групи необхідний для студентів.");
                    hasErrors = true;
                }
            }
            else
            {
                groupIdForCache = null;
                groupEntity = null;
            }

            var tempUserForValidation = new Models.User
            {
                UserName = request.UserName,
                Email = request.Email,
                RealName = request.RealName
            };

            var validationResult = await _validationSubsystem.ValidateUserDataAsync(tempUserForValidation, request.Password);

            if (!validationResult.Succeeded)
            {
                hasErrors = true;
                foreach (var error in validationResult.Errors)
                {
                    if (error == null) continue;

                    string? key = null;
                    if (key == null && error.Description != null)
                    {
                        if (error.Code != null)
                        {
                            if (error.Code.StartsWith("Password")) { key = "Password"; }
                            else if (error.Code.StartsWith("User") || error.Code.StartsWith("UserName")) { key = "UserName"; }
                            else if (error.Code.StartsWith("Email")) { key = "Email"; }
                            else if (error.Code.StartsWith("RealName")) { key = "RealName"; }
                        }


                        if (error.Description.Contains("Email", StringComparison.OrdinalIgnoreCase)) { key = "Email"; }
                        else if (error.Description.Contains("RealName", StringComparison.OrdinalIgnoreCase)) { key = "RealName"; }
                        else if (error.Description.Contains("Username", StringComparison.OrdinalIgnoreCase) || error.Description.Contains("UserName", StringComparison.OrdinalIgnoreCase)) { key = "UserName"; }
                    }

                    if (key != null)
                    {
                        if (!validationErrors.ContainsKey(key)) validationErrors[key] = new List<string>();
                        validationErrors[key].Add(error.Description ?? "Невідома помилка валідації");
                    }
                }
            }

            if (request.IsTeacher)
            {
                bool teacherExists = await _teacherRepository.CheckIfTeacherExistsByEmailAsync(request.Email);
                if (!teacherExists)
                {
                    string teacherKey = "IsTeacher";
                    if (!validationErrors.ContainsKey(teacherKey)) validationErrors[teacherKey] = new List<string>();
                    validationErrors[teacherKey].Add($"Пошту '{request.Email}' не знайдено в таблиці вчителів");

                    hasErrors = true;
                }
            }

            if (hasErrors)
            {
                return (new Response
                {
                    Success = false,
                    StatusCode = 400,
                    Message = "Registration data is invalid.",
                    ValidationErrors = validationErrors
                }, null);
            }

            string verificationCode;
            try
            {
                verificationCode = _sendMessageSubsystem.GenerateVerificationCode();
                await _sendMessageSubsystem.SendVerificationCodeAsync(request.Email, verificationCode);
            }
            catch (Exception ex)
            {
                return (new Response
                {
                    Success = false,
                    StatusCode = 500,
                    Message = $"Помилка надсилання коду підтвердження: {ex.Message}"
                }, null);
            }

            string tempProcessId = Guid.NewGuid().ToString();

            var pendingUserData = _registrationDataStorageSubsystem.PreparePendingUserData(
                request,
                request.Password,
                verificationCode,
                groupIdForCache
            );

            _registrationDataStorageSubsystem.StorePendingRegistrationData(tempProcessId, pendingUserData);
            _logger.LogInformation("Дані збережено для processId: {processId}", tempProcessId);

            return (new Response
            {
                Success = true,
                StatusCode = 200,
                Message = $"Код підтвердження надіслано {request.Email}.",
                ValidationErrors = null
            }, tempProcessId);
        }

        public async Task<(Response Result, string? AccessToken, string? RefreshToken, Models.User? User)> CompleteRegistrationAsync(string processId, string verificationCode) 
        {
            if (string.IsNullOrEmpty(verificationCode))
            {
                return (new Response
                {
                    Success = false,
                    StatusCode = 400,
                    Message = "Код підтвердження не може бути порожнім."
                }, null, null, null);
            }

            var pendingUserData = _registrationDataStorageSubsystem.GetPendingRegistrationData(processId);

            if (pendingUserData == null)
            {
                return (new Response
                {
                    Success = false,
                    StatusCode = 400,
                    Message = "Термін дії запиту на реєстрацію закінчився або дані не знайдені. Спробуйте зареєструватися ще раз."
                }, null, null, null);
            }

            if (!pendingUserData.TryGetValue("VerificationCode", out object? storedCodeObj) || storedCodeObj?.ToString() != verificationCode)
            {
                return (new Response
                {
                    Success = false,
                    StatusCode = 400,
                    Message = "Недійсний код підтвердження."
                }, null, null, null);
            }

            bool isTeacher;
            string email;
            string userName;
            string rawPassword;
            string realName;

            if (
                !pendingUserData.TryGetValue("Email", out object? emailObj) || emailObj == null || !(emailObj is string emailString) ||
                !pendingUserData.TryGetValue("UserName", out object? userNameObj) || userNameObj == null || !(userNameObj is string userNameString) ||
                !pendingUserData.TryGetValue("Password", out object? rawPasswordObj) || rawPasswordObj == null || !(rawPasswordObj is string rawPasswordString) ||
                !pendingUserData.TryGetValue("RealName", out object? realNameObj) || realNameObj == null || !(realNameObj is string realNameString) ||
                !pendingUserData.TryGetValue("IsTeacher", out object? isTeacherObj) || isTeacherObj == null || !(isTeacherObj is bool teacherFlag)
               )
            {
                _registrationDataStorageSubsystem.RemovePendingRegistrationData(processId);
                return (new Response
                {
                    Success = false,
                    StatusCode = 500,
                    Message = "Внутрішня логічна помилка: відсутні дані користувача або неправильний тип у кеші."
                }, null, null, null);
            }
            email = emailString;
            userName = userNameString;
            rawPassword = rawPasswordString;
            realName = realNameString;
            isTeacher = teacherFlag;

            int? groupId = null;
            Group? groupEntity = null;

            if (!isTeacher)
            {
                if (pendingUserData.TryGetValue("Group", out object? groupObj) &&
                    groupObj != null &&
                    int.TryParse(groupObj.ToString(), out int nonNullableGroupId))
                {
                    groupId = nonNullableGroupId;
                    groupEntity = await _groupRepository.GetByIdAsync(groupId.Value);

                    if (groupEntity == null)
                    {
                        _registrationDataStorageSubsystem.RemovePendingRegistrationData(processId);
                        return (new Response
                        {
                            Success = false,
                            StatusCode = 400,
                            Message = $"Група з ід '{groupId}' не існує."
                        }, null, null, null);
                    }
                }
                else
                {
                    _registrationDataStorageSubsystem.RemovePendingRegistrationData(processId);
                    return (new Response
                    {
                        Success = false,
                        StatusCode = 400,
                        Message = "Дані групи потрібні для студентів."
                    }, null, null, null);
                }
            }


            string roleName = isTeacher ? "Teacher" : "Student";
            int initialCoinCount = isTeacher ? 0 : 150;
            int initialEnergyCount = isTeacher ? 0 : 680;

            var newUser = new User
            {
                Email = email,
                UserName = userName,
                RealName = realName,
                EmailConfirmed = true,
                IsActive = true,
                GroupId = groupId,
                Group = groupEntity,
                Role = roleName,
                CoinCount = initialCoinCount,
                EnergyCount = initialEnergyCount,
                MarkCount = 0,
            };

            var createResult = await _userRepository.CreateAsync(newUser, rawPassword);
            if (!createResult.Succeeded)
            {
                _registrationDataStorageSubsystem.RemovePendingRegistrationData(processId);
                var identityErrors = string.Join(" ", createResult.Errors.Select(e => e.Description));
                return (new Response
                {
                    Success = false,
                    StatusCode = 400,
                    Message = $"Не вдалося створити користувача: {identityErrors}"
                }, null, null, null);
            }

            var profile = new Profile
            {
                UserId = newUser.Id,
                AboutMyself = null,
                InstagramLink = null,
                GitHubLink = null,
                LinkedInLink = null
            };

            await _profileRepository.AddAsync(profile);
            Console.WriteLine($"ID створеного профілю для користувача з ID '{newUser.Id}': {profile.Id}");

            newUser = await _userRepository.GetByIdAsync(newUser.Id);
            IdentityResult roleAssignResult = await _userRepository.AddToRoleAsync(newUser, roleName);

            if (!roleAssignResult.Succeeded)
            {
                var roleErrors = string.Join(" ", roleAssignResult.Errors.Select(e => e.Description));
                _registrationDataStorageSubsystem.RemovePendingRegistrationData(processId);
                return (new Response
                {
                    Success = false,
                    StatusCode = 500,
                    Message = $"Користувача створено, але не вдалося призначити роль '{roleName}': {roleErrors}"
                }, null, null, null);
            }
            string accessToken;
            string refreshToken;
            DateTime refreshExpiry;

            var tokens = _tokenGenerationSubsystem.GenerateTokens(newUser);
            accessToken = tokens.accessToken;
            refreshToken = tokens.refreshToken;
            refreshExpiry = tokens.refreshExpiry;

            await _refreshTokenRepository.AddAsync(new RefreshToken
            {
                UserId = newUser.Id,
                Token = refreshToken,
                Expires = refreshExpiry,
                Revoked = null
            });
            _registrationDataStorageSubsystem.RemovePendingRegistrationData(processId);

            return (new Response
            {
                Success = true,
                StatusCode = 200,
                Message = "Реєстрацію успішно завершено.",
                ValidationErrors = null
            }, accessToken, refreshToken, newUser);
        }

        public async Task<(Response Response, string? AccessToken, string? RefreshToken)> LoginAsync(string email, string password)
        {
            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
            {
                return (new Response
                {
                    Success = false,
                    StatusCode = 400,
                    Message = "Пошта або пароль не може бути порожнім."
                }, null, null);
            }

            var user = await _userRepository.FindByEmailAsync(email);

            if (user == null || !user.IsActive)
            {
                return (new Response
                {
                    Success = false,
                    StatusCode = 401,
                    Message = "Недійсні облікові дані або користувач не активний."
                }, null, null);
            }

            var passwordCheck = await CheckPasswordAsync(user, password);
            if (!passwordCheck.Success)
            {
                return (passwordCheck, null, null);
            }

            var (accessToken, refreshToken, accessExpiry, refreshExpiry) = _tokenGenerationSubsystem.GenerateTokens(user);
            var existingRefreshToken = await _refreshTokenRepository
                .FindValidTokenByUserIdAsync(user.Id);

            if (existingRefreshToken != null)
            {
                existingRefreshToken.Token = refreshToken;
                existingRefreshToken.Expires = refreshExpiry;
                existingRefreshToken.Revoked = null;
                await _refreshTokenRepository.UpdateAsync(existingRefreshToken);
            }
            else
            {
                await _refreshTokenRepository.AddAsync(new RefreshToken
                {
                    UserId = user.Id,
                    Token = refreshToken,
                    Expires = refreshExpiry,
                    Revoked = null
                });
            }

            return (new Response
            {
                Success = true,
                StatusCode = 200,
                Message = "Авторизація успішна."
            }, accessToken, refreshToken);
        }
        
        public async Task<Response> SendPasswordResetLink(EmailDto emailDto)
        {
            var user = await _userRepository.FindByEmailAsync(emailDto.Email);
            if (user == null)
            {
                return new Response
                {
                    Success = false,
                    StatusCode = 404, 
                    Message = "Користувача з такою поштою не знайдено"
                };
            }

            var token = Guid.NewGuid().ToString();
            var expiryTime = DateTimeOffset.UtcNow.AddMinutes(2); 

            var tokenEntry = new TokenManager
            {
                Id = Guid.NewGuid(),
                Email = user.Email,
                Token = token,
                Expires = expiryTime
            };

            try
            {
                await _tokenManagerRepository.AddAsync(tokenEntry);
            }
            catch (Exception ex)
            {
                return new Response
                {
                    Success = false,
                    StatusCode = 500,
                    Message = "Виникла внутрішня помилка при генерації посилання."
                };
            }

            string link = $"https://localhost:5173/reset-password?token={token}";
            _logger.LogInformation("Надіслано посилання для скидання пароля на {Email}: {ResetLink}", emailDto.Email, link);

            await _sendMessageSubsystem.SendLinkAsync(emailDto.Email, link);

            return new Response
            {
                Success = true,
                StatusCode = 200, 
                Message = "Лист для зміни пароля надіслано"
            };
        }

        public async Task<Response> UpdatePassword(string token, UpdatePasswordDto dto)
        {
            var tokenEntry = await _tokenManagerRepository.FindByTokenAsync(token);

            if (tokenEntry == null)
            {
                return new Response
                {
                    Success = false,
                    StatusCode = 404,
                    Message = "Некоректний токен"
                };
            }

            if (tokenEntry.Expires < DateTimeOffset.UtcNow)
            {
                await _tokenManagerRepository.DeleteAsync(tokenEntry);
                return new Response
                {
                    Success = false,
                    StatusCode = 400,
                    Message = "Термін дії токена закінчився. Спробуйте знову відновити пароль."
                };
            }

            var email = tokenEntry.Email;
            var user = await _userRepository.FindByEmailAsync(email);

            if (user == null)
            {
                await _tokenManagerRepository.DeleteAsync(tokenEntry);
                return new Response
                {
                    Success = false,
                    StatusCode = 404,
                    Message = "Користувача не знайдено"
                };
            }

            if (string.IsNullOrEmpty(dto.NewPassword))
            {
                return new Response
                {
                    Success = false,
                    StatusCode = 400,
                    Message = "Новий пароль не може бути порожнім.",
                };
            }

            if (dto.NewPassword != dto.ConfirmNewPassword)
            {
                var validationErrors = new Dictionary<string, List<string>>
                {
                    { "ConfirmNewPassword", new List<string> { "Паролі не співпадають." } }
                };

                return new Response
                {
                    Success = false,
                    StatusCode = 400,
                    ValidationErrors = validationErrors
                };
            }


            var validationResult = await _validationSubsystem.ValidateUserDataAsync(user, dto.NewPassword);

            if (!validationResult.Succeeded)
            {
                var validationErrors = new Dictionary<string, List<string>>();
                foreach (var error in validationResult.Errors)
                {
                    if (error?.Description != null)
                    {
                        string key = "Password";
                        if (!validationErrors.ContainsKey(key))
                        {
                            validationErrors[key] = new List<string>();
                        }
                        validationErrors[key].Add(error.Description);
                    }
                }

                return new Response
                {
                    Success = false,
                    StatusCode = 400,
                    Message = "Не вдалося оновити пароль: Пароль не відповідає вимогам.",
                    ValidationErrors = validationErrors.Any() ? validationErrors : null
                };
            }

            try
            {
                var updateResult = await _userRepository.SetNewPasswordAsync(user, dto.NewPassword);

                if (!updateResult.Succeeded)
                {
                    var errors = updateResult.Errors.Select(e => e.Description).ToList();
                    var updateErrorsDict = new Dictionary<string, List<string>>
                    {
                        { "PasswordUpdate", errors }
                    };

                    return new Response
                    {
                        Success = false,
                        StatusCode = 400,
                        Message = "Не вдалося оновити пароль.",
                        ValidationErrors = updateErrorsDict
                    };
                }

                await _tokenManagerRepository.DeleteAsync(tokenEntry);

                return new Response
                {
                    Success = true,
                    StatusCode = 200,
                    Message = "Пароль успішно оновлено"
                };
            }
            catch (Exception ex)
            {
                return new Response
                {
                    Success = false,
                    StatusCode = 500,
                    Message = "Виникла неочікувана помилка при оновленні пароля."
                };
            }
        }

        public async Task<UserGetDto?> GetUserDtoByEmailAsync(string email)
        {
            if (string.IsNullOrEmpty(email))
            {
                return null;
            }

            var userByEmail = await _userRepository.FindByEmailAsync(email);

            if (userByEmail == null)
            {
                return null;
            }

           var userById = await _userRepository.GetByIdAsync(userByEmail.Id);

            var userDto = new UserGetDto
            {
                Id = userById.Id,
                RealName = userById.RealName,
                UserName = userById.UserName,
                Email = userById.Email,
                Group = userById.GroupId,
                IsTeacher = (userById.Role == "Teacher"),
                CoinCount = userById.CoinCount,
                EnergyCount = userById.EnergyCount,
                MarkCount = userById.MarkCount,
                Profile = userById?.Profile != null ? new ProfileDto
                {
                    Id = userById.Profile.Id,
                    AboutMyself = userById.Profile.AboutMyself,
                    InstagramLink = userById.Profile.InstagramLink,
                    GitHubLink = userById.Profile.GitHubLink,
                    LinkedInLink = userById.Profile.LinkedInLink,
                    AvatarId = userById.Profile.AvatarId,
                    BackgroundId = userById.Profile.BackgroundId,
                    ResourceId = userById.Profile.ResourceId,
                } : null
            };

            return userDto;
        }
        
        public async Task<Response> RefreshTokenAsync(HttpRequest request)
        {
            if (!request.Cookies.TryGetValue("refresh_token", out var refreshToken))
            {
                return new Response
                {
                    Success = false,
                    StatusCode = 401,
                    Message = "Refresh токен відсутній"
                };
            }

            var token = await _refreshTokenRepository.FindByTokenAsync(refreshToken);
            if (token == null || !token.IsActive)
            {
                return new Response
                {
                    Success = false,
                    StatusCode = 401,
                    Message = "Недійсний або відкликаний токен"
                };
            }

            var user = await _userRepository.GetByIdAsync(token.UserId);
            if (user == null)
            {
                return new Response
                {
                    Success = false,
                    StatusCode = 401,
                    Message = "Користувача не знайдено"
                };
            }

            var (newAccessToken, newRefreshToken, accessExpiry, refreshExpiry) = _tokenGenerationSubsystem.GenerateTokens(user);

            token.Revoked = DateTime.UtcNow;
            await _refreshTokenRepository.UpdateAsync(token);

            var newRefreshTokenEntry = new RefreshToken
            {
                Token = newRefreshToken,
                UserId = user.Id,
                Expires = refreshExpiry
            };
            await _refreshTokenRepository.AddAsync(newRefreshTokenEntry);

            return new RefreshTokenResponse
            {
                Success = true,
                StatusCode = 200,
                Message = "Токен оновлено",
                AccessToken = newAccessToken,
                NewRefreshToken = newRefreshToken,
                RefreshTokenExpires = newRefreshTokenEntry.Expires
            };
        }

        public async Task<bool> RevokeRefreshTokenAsync(string refreshTokenValue)
        {
            var refreshToken = await _refreshTokenRepository.FindByTokenAsync(refreshTokenValue);

            if (refreshToken == null)
                return false;

            refreshToken.Revoked = DateTime.UtcNow;

            await _refreshTokenRepository.UpdateAsync(refreshToken);
            return true;
        }
    }
}