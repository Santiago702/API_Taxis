using BCrypt.Net;

namespace APITaxiV2.Funciones
{
    public static class Encriptado
    {
        private const int WorkFactor = 12;

        // Método para generar hash (ya lo tenías)
        public static string HashPassword(string password)
        {
            return BCrypt.Net.BCrypt.HashPassword(password, WorkFactor);
        }

        // Nuevo método para verificar contraseña
        public static (bool Success, string Message) VerifyPassword(string inputPassword, string storedHash)
        {
            // 1. Validar si el storedHash es un formato BCrypt válido
            if (string.IsNullOrEmpty(storedHash) || !storedHash.StartsWith("$2a$"))
            {
                return (false, "El hash almacenado no es válido. No es un hash BCrypt.");
            }

            // 2. Verificar la contraseña
            try
            {
                bool isValid = BCrypt.Net.BCrypt.Verify(inputPassword, storedHash);
                return (isValid, isValid ? "¡Contraseña válida!" : "Contraseña incorrecta.");
            }
            catch (SaltParseException ex)
            {
                return (false, $"Error al verificar el hash: {ex.Message}");
            }

        }
    }
}
