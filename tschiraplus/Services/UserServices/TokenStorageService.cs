using System.Security.Cryptography;
using System.Text;

namespace Services.UserServices;

public class TokenStorageService
{
    //private const string FileName = "authToken.txt";
    //private static string FilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, FileName);
    private readonly string _filePath;

    public TokenStorageService(string filePath = "")
    {
        _filePath = string.IsNullOrWhiteSpace(filePath)
            ? Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "authToken.txt") 
            : filePath;
    }
    /// <summary>
    /// Encrypts a token and saves it to a file
    /// </summary>
    /// <param name="token"></param>
    public void SaveToken(string token)
    {
        var encryptedToken = EncryptToken(token);
        using var fs = new FileStream(_filePath, FileMode.Create, FileAccess.Write, FileShare.None);
        using var sw = new StreamWriter(fs);
        sw.Write(encryptedToken);
        sw.Flush();
    }

    /// <summary>
    /// Loads the saved token and decrypts it
    /// </summary>
    /// <returns>The decrypted token as string</returns>
    public string? LoadToken()
    {
        if (!File.Exists(_filePath)) return null;

        using var fs = new FileStream(_filePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
        using var sr = new StreamReader(fs);
        return DecryptToken(sr.ReadToEnd());
    }

    /// <summary>
    /// Removes the saved token
    /// </summary>
    public void RemoveToken()
    {
        if (File.Exists(_filePath)) File.Delete(_filePath);
    }

    /// <summary>
    /// Encrypts a token using AES (Advanced Encryption Standard)
    /// </summary>
    /// <param name="token"></param>
    /// <returns>The encrypted token</returns>
    private string EncryptToken(string token)
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
            sw.Flush();
            cs.FlushFinalBlock();
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
    private string DecryptToken(string encryptedToken)
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
    private byte[] GetEncryptionKey()
    {
        const string key = "YooWassupThisIsMySuperSecretKeyT";
        return Encoding.UTF8.GetBytes(key);
    }
}