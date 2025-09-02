using InnovaGraphics.Interactions;
using InnovaGraphics.Models;
using InnovaGraphics.Repositories.Interfaces;
using InnovaGraphics.Services.Interfaces;
using InnovaGraphics.Utils.Facade;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.JsonPatch.Operations;

namespace InnovaGraphics.Services.Implementations
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly UserManager<User> _userManager;
        private readonly ValidationSubsystem _validationSubsystem;
        private readonly IRepository<Group> _groupRepository;
        private readonly IPurchaseRepository _purchaseRepository;

        public UserService(IUserRepository userRepository, UserManager<User> userManager,
                           ValidationSubsystem validationSubsystem, IRepository<Group> groupRepository,
                            IPurchaseRepository purchaseRepository) 
        {
            _userRepository = userRepository;
            _userManager = userManager;
            _validationSubsystem = validationSubsystem;
            _groupRepository = groupRepository;
            _purchaseRepository = purchaseRepository;
        }

        public async Task<Response> UpdateUserAsync(string userId, JsonPatchDocument<User> patchDocument)
        {
            var user = await _userRepository.GetByIdAsync(userId);

            if (user == null)
            {
                return new Response { Success = false, Message = "Користувача не знайдено." };
            }

            if (user.Profile == null)
            {
                return new Response { Success = false, Message = "Профіль користувача не завантажено або відсутній." };
            }

            bool anyUpdateSuccessful = false;

            foreach (var operation in patchDocument.Operations)
            {
                if (operation.path.Equals("/userName", StringComparison.OrdinalIgnoreCase) ||
                    operation.path.Equals("userName", StringComparison.OrdinalIgnoreCase))
                {
                    if (operation.OperationType == OperationType.Replace || operation.OperationType == OperationType.Add)
                    {
                        string newUserNameValue = operation.value?.ToString();

                        if (newUserNameValue != null)
                        {
                            var tempUserForValidation = new User { UserName = newUserNameValue };
                            var userNameValidationErrors = _validationSubsystem.ValidateUsernameLength(tempUserForValidation).ToList();

                            if (userNameValidationErrors.Any())
                            {
                                var errors = string.Join(", ", userNameValidationErrors.Select(e => e.Description));
                                return new Response { Success = false, Message = $"Помилка валідації імені користувача: {errors}" };
                            }
                            if (newUserNameValue != user.UserName)
                            {

                                var result = await _userManager.SetUserNameAsync(user, newUserNameValue);
                                if (!result.Succeeded)
                                {
                                    var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                                    return new Response { Success = false, Message = $"Не вдалося оновити ім'я користувача: {errors}" };
                                }
                                anyUpdateSuccessful = true;
                            }
                        }
                        else
                        {
                            return new Response { Success = false, Message = "Ім'я користувача не може бути встановлено як null." };
                        }
                    }
                }

                else if (operation.path.Equals("/realName", StringComparison.OrdinalIgnoreCase) ||
                         operation.path.Equals("realName", StringComparison.OrdinalIgnoreCase))
                {
                    if (operation.OperationType == OperationType.Replace || operation.OperationType == OperationType.Add)
                    {
                        string newRealNameValue = operation.value?.ToString();

                        if (newRealNameValue != null)
                        {
                            var userForValidation = new User { RealName = newRealNameValue };
                            var realNameValidationErrors = _validationSubsystem.ValidateRealNameLength(userForValidation).ToList();

                            if (realNameValidationErrors.Any())
                            {
                                var errors = string.Join(", ", realNameValidationErrors.Select(e => e.Description));
                                return new Response { Success = false, Message = $"Помилка валідації справжнього імені: {errors}" };
                            }

                            if (newRealNameValue != user.RealName)
                            {
                                user.RealName = newRealNameValue;
                                await _userRepository.UpdateAsync(user);
                                anyUpdateSuccessful = true;
                            }
                        }
                        else
                        {
                            return new Response { Success = false, Message = "Справжнє ім'я не може бути встановлено як null." };
                        }
                    }
                }
                else if (operation.path.Equals("/groupId", StringComparison.OrdinalIgnoreCase) ||
                         operation.path.Equals("groupId", StringComparison.OrdinalIgnoreCase))
                {
                    if (operation.OperationType == OperationType.Replace || operation.OperationType == OperationType.Add)
                    {
                        int? newGroupIdValue = null;

                        if (operation.value == null)
                        {
                            newGroupIdValue = null;
                        }
                        else if (int.TryParse(operation.value.ToString(), out int parsedGroupId))
                        {
                            newGroupIdValue = parsedGroupId;
                        }
                        else
                        {
                            return new Response { Success = false, Message = "Невірний формат значення для GroupId. Очікується ціле число або null." };
                        }

                        if (newGroupIdValue.HasValue)
                        {
                            Group? groupEntity = await _groupRepository.GetByIdAsync(newGroupIdValue.Value) as Group;
                            if (groupEntity == null)
                            {
                                return new Response { Success = false, Message = $"Група з ID '{newGroupIdValue.Value}' не знайдена." };
                            }
                        }

                        if (newGroupIdValue != user.GroupId)
                        {
                            user.GroupId = newGroupIdValue;
                            await _userRepository.UpdateAsync(user);
                            anyUpdateSuccessful = true;
                        }
                    }
                }
                else if (operation.path.Equals("/profile/avatarId", StringComparison.OrdinalIgnoreCase) ||
                         operation.path.Equals("profile/avatarId", StringComparison.OrdinalIgnoreCase))
                {
                    if (operation.OperationType == OperationType.Replace || operation.OperationType == OperationType.Add)
                    {
                        Guid? newAvatarIdValue = null;
                        
                        if (Guid.TryParse(operation.value.ToString(), out Guid parsedAvatarId))
                        {
                            newAvatarIdValue = parsedAvatarId;
                        }
                        else
                        {
                            return new Response { Success = false, Message = "Невірний формат значення для AvatarId. Очікується GUID." };
                        }

                        if (newAvatarIdValue.HasValue)
                        {
                            ShopItem? purchasedShopItem = await _purchaseRepository.GetPurchasedShopItemAsync(userId, newAvatarIdValue.Value);

                            if (purchasedShopItem == null)
                            {
                                return new Response { Success = false, Message = $"Користувач не придбав аватарку з ID '{newAvatarIdValue.Value}'." };
                            }

                            if (purchasedShopItem.Type != Enums.ShopItemType.Avatar)
                            {
                                return new Response { Success = false, Message = $"Придбаний ShopItem з ID '{newAvatarIdValue.Value}' не є типом 'Avatar'." };
                            }
                        }

                        if (newAvatarIdValue != user.Profile.AvatarId)
                        {
                            user.Profile.AvatarId = newAvatarIdValue;
                            await _userRepository.UpdateAsync(user);
                            anyUpdateSuccessful = true;
                        }
                    }
                }
                else if (operation.path.Equals("/profile/BackgroundId", StringComparison.OrdinalIgnoreCase) ||
                         operation.path.Equals("profile/BackgroundId", StringComparison.OrdinalIgnoreCase))
                {
                    if (operation.OperationType == OperationType.Replace || operation.OperationType == OperationType.Add)
                    {
                        Guid? newBackgroundIdValue = null;

                        if (Guid.TryParse(operation.value.ToString(), out Guid parsedBackgroundId))
                        {
                            newBackgroundIdValue = parsedBackgroundId;
                        }
                        else
                        {
                            return new Response { Success = false, Message = "Невірний формат значення для BackgroundId. Очікується GUID." };
                        }

                        if (newBackgroundIdValue.HasValue)
                        {
                            ShopItem? purchasedShopItem = await _purchaseRepository.GetPurchasedShopItemAsync(userId, newBackgroundIdValue.Value);

                            if (purchasedShopItem == null)
                            {
                                return new Response { Success = false, Message = $"Користувач не придбав фону з ID '{newBackgroundIdValue.Value}'." };
                            }

                            if (purchasedShopItem.Type != Enums.ShopItemType.Background)
                            {
                                return new Response { Success = false, Message = $"Придбаний ShopItem з ID '{newBackgroundIdValue.Value}' не є типом 'Background'." };
                            }
                        }

                        if (newBackgroundIdValue != user.Profile.BackgroundId)
                        {
                            user.Profile.BackgroundId = newBackgroundIdValue;
                            await _userRepository.UpdateAsync(user);
                            anyUpdateSuccessful = true;
                        }
                    }
                }

                else if (operation.path.Equals("/profile/InstagramLink", StringComparison.OrdinalIgnoreCase) ||
                         operation.path.Equals("profile/InstagramLink", StringComparison.OrdinalIgnoreCase))
                {
                    if (operation.OperationType == OperationType.Replace || operation.OperationType == OperationType.Add)
                    {
                        string? newInstagramLinkValue = operation.value?.ToString();

                        var instagramLinkValidationErrors = _validationSubsystem.ValidateUrlFormat(newInstagramLinkValue, "InstagramLink").ToList();

                        if (instagramLinkValidationErrors.Any()){ 
                            var errors = string.Join(", ", instagramLinkValidationErrors.Select(e => e.Description));
                            return new Response { Success = false, Message = $"Помилка валідації: {errors}"};
                        }

                        if (newInstagramLinkValue != user.Profile.InstagramLink)
                        {
                            user.Profile.InstagramLink = newInstagramLinkValue;
                            await _userRepository.UpdateAsync(user);
                            anyUpdateSuccessful = true;
                        }
                    }
                }

                else if (operation.path.Equals("/profile/GitHubLink", StringComparison.OrdinalIgnoreCase) ||
                         operation.path.Equals("profile/GitHubLink", StringComparison.OrdinalIgnoreCase))
                {
                    if (operation.OperationType == OperationType.Replace || operation.OperationType == OperationType.Add)
                    {
                        string? newGithubLinkValue = operation.value?.ToString();

                        var GithubLinkValidationErrors = _validationSubsystem.ValidateUrlFormat(newGithubLinkValue, "GithubLink").ToList();

                        if (GithubLinkValidationErrors.Any())
                        {
                            var errors = string.Join(", ", GithubLinkValidationErrors.Select(e => e.Description));
                            return new Response { Success = false, Message = $"Помилка валідації: {errors}" };
                        }

                        if (newGithubLinkValue != user.Profile.GitHubLink)
                        {
                            user.Profile.InstagramLink = newGithubLinkValue;
                            await _userRepository.UpdateAsync(user);
                            anyUpdateSuccessful = true;
                        }
                    }
                }

                else if (operation.path.Equals("/profile/LinkedInLink", StringComparison.OrdinalIgnoreCase) ||
                         operation.path.Equals("profile/LinkedInLink", StringComparison.OrdinalIgnoreCase))
                {
                    if (operation.OperationType == OperationType.Replace || operation.OperationType == OperationType.Add)
                    {
                        string? newLinkedInLinkValue = operation.value?.ToString();

                        var linkedInLinkValidationErrors = _validationSubsystem.ValidateUrlFormat(newLinkedInLinkValue, "LinkedInLink").ToList();

                        if (linkedInLinkValidationErrors.Any())
                        {
                            var errors = string.Join(", ", linkedInLinkValidationErrors.Select(e => e.Description));
                            return new Response { Success = false, Message = $"Помилка валідації: {errors}" };
                        }

                        if (newLinkedInLinkValue != user.Profile.LinkedInLink)
                        {
                            user.Profile.LinkedInLink = newLinkedInLinkValue;
                            await _userRepository.UpdateAsync(user);
                            anyUpdateSuccessful = true;
                        }
                    }
                }
                
                else if (operation.path.Equals("/profile/AboutMyself", StringComparison.OrdinalIgnoreCase) ||
                         operation.path.Equals("profile/AboutMyself", StringComparison.OrdinalIgnoreCase))
                {
                    if (operation.OperationType == OperationType.Replace || operation.OperationType == OperationType.Add)
                    {
                        string? newAboutMyselfValue = operation.value?.ToString();

                        var aboutMyselfValidationErrors = _validationSubsystem.ValidateAboutMyselfLength(newAboutMyselfValue, "AboutMyself").ToList();

                        if (aboutMyselfValidationErrors.Any())
                        {
                            var errors = string.Join(", ", aboutMyselfValidationErrors.Select(e => e.Description));
                            return new Response { Success = false, Message = $"Помилка валідації: {errors}" };
                        }

                        if (newAboutMyselfValue != user.Profile.AboutMyself)
                        {
                            user.Profile.AboutMyself = newAboutMyselfValue;
                            await _userRepository.UpdateAsync(user);
                            anyUpdateSuccessful = true;
                        }
                    }
                }
            }

            if (!patchDocument.Operations.Any())
            {
                return new Response
                {
                    Success = false,
                    Message = "Документ patch не містить операцій оновлення."
                };
            }
            else if (anyUpdateSuccessful)
            {
                return new Response { Success = true, Message = "Інформацію користувача успішно оновлено." };
            }
            else
            {
                return new Response
                {
                    Success = true,
                    Message = "Запит на оновлення оброблено, але фактичних змін не відбулося (можливо, нові значення збігаються з поточними)."
                };
            }
        }

        public async Task<Response> AddMarksAndCoinsAsync(string userId, int mark, int coin)
        {
            var user = await _userRepository.GetByIdAsync(userId);

            if (user == null)
            {
                return new Response { Success = false, Message = "Користувача не знайдено." };
            }

            try
            {
                user.MarkCount += mark;
                user.CoinCount += coin;

                await _userRepository.UpdateAsync(user);

                return new Response { Success = true};
            }
            catch (Exception ex)
            {
                return new Response { Success = false, Message = $"Помилка при додаванні балів та монет: {ex.Message}" };
            }
        }

        //public async Task<List<User>> GetUsersByIdsAsync(List<string> userIds)
        //{
        //    var users = new List<User>();
        //    foreach (var id in userIds)
        //    {
        //        var user = await _userRepository.GetByIdAsync(id);
        //        if (user != null)
        //            users.Add(user);
        //    }
        //    return users;
        //}


    }
}
