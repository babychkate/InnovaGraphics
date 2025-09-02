using Microsoft.AspNetCore.Identity;

namespace InnovaGraphics.Utils
{
    public class LocalSubsystem : IdentityErrorDescriber
    {
        public override IdentityError DefaultError() =>
            new IdentityError { Code = nameof(DefaultError), Description = "Виникла невідома помилка." };

        public override IdentityError ConcurrencyFailure() =>
            new IdentityError { Code = nameof(ConcurrencyFailure), Description = "Помилка паралельного доступу, об’єкт було змінено." };

        public override IdentityError PasswordMismatch() =>
            new IdentityError { Code = nameof(PasswordMismatch), Description = "Неправильний пароль." };

        public override IdentityError InvalidToken() =>
            new IdentityError { Code = nameof(InvalidToken), Description = "Недійсний токен." };

        public override IdentityError UserLockoutNotEnabled() =>
            new IdentityError { Code = nameof(UserLockoutNotEnabled), Description = "Блокування для цього користувача не ввімкнено." };
        
        public override IdentityError InvalidEmail(string? email) =>
            new IdentityError { Code = nameof(InvalidEmail), Description = $"Електронна пошта '{email}' недійсна." };

        public override IdentityError DuplicateEmail(string? email) =>
            new IdentityError { Code = nameof(DuplicateEmail), Description = $"Електронна пошта '{email}' вже використовується." };

        public override IdentityError InvalidUserName(string? userName) =>
            new IdentityError { Code = nameof(InvalidUserName), Description = $"Ім’я користувача '{userName}' недійсне, може містити лише літери, цифри або символи '_', '-', '.'." };

        public override IdentityError DuplicateUserName(string? userName) =>
            new IdentityError { Code = nameof(DuplicateUserName), Description = $"Ім’я користувача '{userName}' вже зайнято." };

        public override IdentityError PasswordTooShort(int length) =>
            new IdentityError { Code = nameof(PasswordTooShort), Description = $"Пароль має містити щонайменше {length} символів." };

        public override IdentityError PasswordRequiresNonAlphanumeric() =>
            new IdentityError { Code = nameof(PasswordRequiresNonAlphanumeric), Description = "Пароль повинен містити хоча б один небуквено-цифровий символ." };

        public override IdentityError PasswordRequiresDigit() =>
            new IdentityError { Code = nameof(PasswordRequiresDigit), Description = "Пароль повинен містити хоча б одну цифру ('0'-'9')." };

        public override IdentityError PasswordRequiresLower() =>
            new IdentityError { Code = nameof(PasswordRequiresLower), Description = "Пароль повинен містити хоча б одну літеру в нижньому регістрі ('a'-'z')." };

        public override IdentityError PasswordRequiresUpper() =>
            new IdentityError { Code = nameof(PasswordRequiresUpper), Description = "Пароль повинен містити хоча б одну літеру у верхньому регістрі ('A'-'Z')." };

        public override IdentityError PasswordRequiresUniqueChars(int uniqueChars) =>
            new IdentityError { Code = nameof(PasswordRequiresUniqueChars), Description = $"Пароль повинен містити щонайменше {uniqueChars} унікальних символів." };
               
        public override IdentityError InvalidRoleName(string? role) =>
            new IdentityError { Code = nameof(InvalidRoleName), Description = $"Назва ролі '{role}' є недійсною." };

        public override IdentityError DuplicateRoleName(string? role) =>
            new IdentityError { Code = nameof(DuplicateRoleName), Description = $"Назва ролі '{role}' вже існує." };

        public override IdentityError UserAlreadyHasPassword() =>
            new IdentityError { Code = nameof(UserAlreadyHasPassword), Description = "Користувач вже має встановлений пароль." };

        public override IdentityError UserAlreadyInRole(string? role) =>
            new IdentityError { Code = nameof(UserAlreadyInRole), Description = $"Користувач вже перебуває в ролі '{role}'." };

        public override IdentityError UserNotInRole(string? role) =>
            new IdentityError { Code = nameof(UserNotInRole), Description = $"Користувач не перебуває в ролі '{role}'." };
    }
}