using System.Security.Cryptography;
using System.Text;
using BCrypt.Net; // Добавьте эту директиву

namespace CarRecyclingWeb.Extensions
{
    public static class PasswordHasher
    {
        public static string HashPassword(string password)
        {
            // BCrypt.HashPassword генерирует соль автоматически
            // и возвращает хеш, включающий эту соль.
            return BCrypt.Net.BCrypt.HashPassword(password);
        }

        public static bool VerifyPassword(string password, string hashedPassword)
        {
            // BCrypt.VerifyPassword проверяет пароль с данным хешем.
            // Он извлекает соль из хеша и использует ее для проверки.
            return BCrypt.Net.BCrypt.Verify(password, hashedPassword);
        }
    }
}