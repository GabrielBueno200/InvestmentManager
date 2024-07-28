namespace Api.Auth.Application.Interfaces;

public interface IEncryptorService
{
    string Encrypt(string text);
    string Decrypt(string text);
}
