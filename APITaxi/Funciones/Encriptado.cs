using BCrypt.Net;
using System.Security.Cryptography;
using System.Text;

namespace APITaxiV2.Funciones
{
    public static class Encriptado
    {
        private const int WorkFactor = 12;




        public static string HashPassword(string password)
        {
            return BCrypt.Net.BCrypt.HashPassword(password, WorkFactor);
        }


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

        private static readonly string clave = "T8#kB1z!Wq9@V7e$Zx4&C2m^L0*Na6Yr"; // 32 caracteres

        public static string Encriptar(string textoPlano)
        {
            byte[] claveBytes = Encoding.UTF8.GetBytes(clave);
            byte[] iv = new byte[16]; // Vector de inicialización (puede ser fijo o aleatorio)

            using (Aes aes = Aes.Create())
            {
                aes.Key = claveBytes;
                aes.IV = iv;

                using (MemoryStream ms = new MemoryStream())
                using (CryptoStream cs = new CryptoStream(ms, aes.CreateEncryptor(), CryptoStreamMode.Write))
                {
                    byte[] textoBytes = Encoding.UTF8.GetBytes(textoPlano);
                    cs.Write(textoBytes, 0, textoBytes.Length);
                    cs.FlushFinalBlock();
                    return Convert.ToBase64String(ms.ToArray());
                }
            }
        }

        public static string Desencriptar(string textoCifrado)
        {
            byte[] claveBytes = Encoding.UTF8.GetBytes(clave);
            byte[] iv = new byte[16];

            using (Aes aes = Aes.Create())
            {
                aes.Key = claveBytes;
                aes.IV = iv;

                using (MemoryStream ms = new MemoryStream())
                using (CryptoStream cs = new CryptoStream(ms, aes.CreateDecryptor(), CryptoStreamMode.Write))
                {
                    byte[] textoBytes = Convert.FromBase64String(textoCifrado);
                    cs.Write(textoBytes, 0, textoBytes.Length);
                    cs.FlushFinalBlock();
                    return Encoding.UTF8.GetString(ms.ToArray());
                }
            }
        }
    }
}
