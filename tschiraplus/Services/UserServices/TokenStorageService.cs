using System.Security.Cryptography;
using System.Text;

namespace Services.UserServices;

public class TokenStorageService
{
    private const string FileName = "authToken.txt";
    private static readonly string FilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, FileName);

    /// <summary>
    /// Encrypts a token and saves it to a file
    /// </summary>
    /// <param name="token"></param>
    public static void SaveToken(string token)
    {
        var encryptedToken = EncryptToken(token);
        File.WriteAllText(FilePath, encryptedToken);
    }

    /// <summary>
    /// Loads the saved token and decrypts it
    /// </summary>
    /// <returns>The decrypted token as string</returns>
    public static string? LoadToken()
    {
        if (!File.Exists(FilePath)) return null;

        var encryptedToken = File.ReadAllText(FilePath);
        return DecryptToken(encryptedToken);
    }

    /// <summary>
    /// Removes the saved token
    /// </summary>
    public static void RemoveToken()
    {
        if (File.Exists(FilePath)) File.Delete(FilePath);
    }

    /// <summary>
    /// Encrypts a token using AES (Advanced Encryption Standard)
    /// </summary>
    /// <param name="token"></param>
    /// <returns>The encrypted token</returns>
    private static string EncryptToken(string token)
    {
        var key = GetEncryptionKey();
        using var aes = Aes.Create();
        aes.Key = key;
        aes.GenerateIV();
        var iv = aes.IV;

        using var encryptor = aes.CreateEncryptor(aes.Key, iv);
        using var ms = new MemoryStream();
        using var cs = new CryptoStream(ms, encryptor, CryptoStreamMode.Write);
        using (var sw = new StreamWriter(cs))
        {
            sw.Write(token);
        }

        var encryptedData = ms.ToArray();
        var result = new byte[iv.Length + encryptedData.Length];
        Buffer.BlockCopy(iv, 0, result, 0, iv.Length);
        Buffer.BlockCopy(encryptedData, 0, result, iv.Length, encryptedData.Length);

        return Convert.ToBase64String(result);
    }

    /// <summary>
    /// Decrypts a token using AES (Advanced Encryption Standard)
    /// </summary>
    /// <param name="encryptedToken"></param>
    /// <returns>The decrypted token</returns>
    private static string DecryptToken(string encryptedToken)
    {
        var fullCipher = Convert.FromBase64String(encryptedToken);
        var iv = new byte[16];
        var cipherText = new byte[fullCipher.Length - iv.Length];
        
        Buffer.BlockCopy(fullCipher, 0, iv, 0, iv.Length);
        Buffer.BlockCopy(fullCipher, iv.Length, cipherText, 0, cipherText.Length);

        var key = GetEncryptionKey();
        using var aes = Aes.Create();
        aes.Key = key;
        aes.IV = iv;

        using var decryptor = aes.CreateDecryptor(aes.Key, aes.IV);
        using var ms = new MemoryStream(cipherText);
        using var cs = new CryptoStream(ms, decryptor, CryptoStreamMode.Read);
        using var sr = new StreamReader(cs);
        return sr.ReadToEnd();
    }

    /// <summary>
    /// Returns the encryption key as byte array
    /// </summary>
    /// <returns>The encryption key as byte array</returns>
    private static byte[] GetEncryptionKey()
    {
        const string key = "YooWassupThisIsMySuperSecretKeyT";
        return Encoding.UTF8.GetBytes(key);
    }
}