using System.Text;
using System.Security.Cryptography;
using Api.Auth.Infrastructure.Settings;
using Microsoft.Extensions.Options;
using Api.Auth.Application.Interfaces;

namespace Api.Auth.Infrastructure.Services;

public class AesEncryptorService(IOptions<AesSettings> aesSettings) : IEncryptorService
{
    private readonly byte[] _key = Convert.FromBase64String(aesSettings.Value.Key);
    private readonly byte[] _iv = Convert.FromBase64String(aesSettings.Value.Iv);

    public string Encrypt(string text)
    {
        using var aes = Aes.Create();
        aes.Key = _key;
        aes.IV = _iv;

        var encryptor = aes.CreateEncryptor(aes.Key, aes.IV);

        using var ms = new MemoryStream();
        using var cs = new CryptoStream(ms, encryptor, CryptoStreamMode.Write);
        using var sw = new StreamWriter(cs);

        sw.Write(text);
        sw.Close();

        return Convert.ToBase64String(ms.ToArray());
    }

    public string Decrypt(string text)
    {
        using var aes = Aes.Create();
        aes.Key = _key;
        aes.IV = _iv;

        var decryptor = aes.CreateDecryptor(aes.Key, aes.IV);

        using var ms = new MemoryStream(Convert.FromBase64String(text));
        using var cs = new CryptoStream(ms, decryptor, CryptoStreamMode.Read);
        using var sr = new StreamReader(cs);

        return sr.ReadToEnd();
    }
}
