using InnovaGraphics.Models;
using Microsoft.AspNetCore.Identity;
using System.Text.RegularExpressions;

namespace InnovaGraphics.Utils.Facade
{
    public class ValidationSubsystem
    {
        private readonly UserManager<User> _userManager;

        public ValidationSubsystem(UserManager<User> userManager)
        {
            _userManager = userManager;
        }

        public async Task<IdentityResult> ValidateUserDataAsync(User user, string password)
        {
            var errors = new List<IdentityError>();

            errors.AddRange(await ValidateStandardUserPropertiesAsync(user));
            errors.AddRange(await ValidatePasswordComplexityAsync(user, password));
            errors.AddRange(ValidateUsernameLength(user));
            errors.AddRange(ValidateRealNameLength(user));
            errors.AddRange(ValidateEmailDomainFormat(user.Email));

            if (errors.Any())
            {
                return IdentityResult.Failed(errors.ToArray());
            }

            return IdentityResult.Success;
        }

        public async Task<IEnumerable<IdentityError>> ValidateStandardUserPropertiesAsync(User user)
        {
            if (_userManager.UserValidators == null || _userManager.UserValidators.Count == 0)
            {
                return new List<IdentityError> { new IdentityError { Description = "Валідатори користувачів не налаштовані." } };
            }
            var result = await _userManager.UserValidators[0].ValidateAsync(_userManager, user);
            return result.Succeeded ? Enumerable.Empty<IdentityError>() : result.Errors;
        }

        public async Task<IEnumerable<IdentityError>> ValidatePasswordComplexityAsync(User user, string password)
        {
            if (_userManager.PasswordValidators == null || _userManager.PasswordValidators.Count == 0)
            {
                return new List<IdentityError> { new IdentityError { Description = "Засоби перевірки паролів не налаштовано." } };
            }
            var result = await _userManager.PasswordValidators[0].ValidateAsync(_userManager, user, password);
            return result.Succeeded ? Enumerable.Empty<IdentityError>() : result.Errors;
        }

        public IEnumerable<IdentityError> ValidateUsernameLength(User user)
        {
            if (string.IsNullOrEmpty(user.UserName))
            {
                yield return new IdentityError { Code = "UserNameEmpty", Description = "Username не може бути порожнім" };
            }
            else if (user.UserName.Length > 20)
            {
                yield return new IdentityError { Code = "UserNameTooLong", Description = "Username не може перевищувати 20 символів" };
            }
        }

        public IEnumerable<IdentityError> ValidateRealNameLength(User user)
        {
            if (string.IsNullOrEmpty(user.RealName))
            {
                yield return new IdentityError { Code = "RealNameEmpty", Description = "Ім'я не може бути порожнім" };
            }
            else if (user.RealName.Length > 20)
            {
                yield return new IdentityError { Code = "RealNameTooLong", Description = "Довжина імені не може перевищувати 20 символів" };
            }
            else if (user.UserName.Any(c => c >= '\u0400' && c <= '\u04FF'))
            {
                yield return new IdentityError { Code = "UserNameCyrillic", Description = "Username не повинен містити кириличні символи" };
            }
            else if (Regex.IsMatch(user.UserName, @"[^a-zA-Z0-9._-]"))
            {
                yield return new IdentityError { Code = "UserNameInvalidCharacters", Description = "Username містить недопустимі символи. Дозволені лише літери, цифри, крапка, підкреслення та дефіс." };
            }            
        }

        public IEnumerable<IdentityError> ValidateEmailDomainFormat(string email)
        {
            if (string.IsNullOrEmpty(email))
            {
                yield return new IdentityError { Code = "EmailEmpty", Description = "Пошта не може бути порожня" };
                yield break;
            }

            int atIndex = email.IndexOf('@');

            if (atIndex <= 0)
            {
                yield return new IdentityError { Code = "EmailMissingLocalPart", Description = "Перед @ має бути текст" };
                yield break;
            }

            string localPart = email.Substring(0, atIndex);

            if (localPart.Any(c => c >= '\u0400' && c <= '\u04FF'))
            {
                yield return new IdentityError { Code = "EmailCyrillic", Description = "Електронна пошта не повинна містити кириличні символи" };
            }

            if (!email.EndsWith("@lpnu.ua", StringComparison.OrdinalIgnoreCase))
            {
                yield return new IdentityError { Code = "EmailDomainInvalid", Description = "Пошта має закінчуватися '@lpnu.ua'" };
            }
        }

        public IEnumerable<IdentityError> ValidateUrlFormat(string? url, string fieldName)
        {
            if (url == null)
            {
                yield break;
            }

            if (string.IsNullOrWhiteSpace(url))
            {
                yield return new IdentityError { Code = $"{fieldName}Empty", Description = $"{fieldName} не може бути порожнім або складатися лише з пробілів." };
                yield break; 
            }

            const int MaxUrlLength = 50;

            if (url.Length > MaxUrlLength)
            {
                yield return new IdentityError { Code = $"{fieldName}TooLong", Description = $"{fieldName} не може перевищувати {MaxUrlLength} символів." };
            }
        }
        

        public IEnumerable<IdentityError> ValidateAboutMyselfLength(string? aboutMyself, string fieldName)
        {

            if (string.IsNullOrWhiteSpace(aboutMyself))
            {
                yield return new IdentityError { Code = $"{fieldName}Empty", Description = $"{fieldName} не може бути порожнім або складатися лише з пробілів." };
                yield break;
            }

            const int MinLength = 10;
            const int MaxLength = 200;

            if (aboutMyself.Length < MinLength || aboutMyself.Length > MaxLength)
            {
                yield return new IdentityError { Code = $"{fieldName}Length", Description = $"{fieldName} має бути від {MinLength} до {MaxLength} символів." };
            }
        }
    
    
    }
}