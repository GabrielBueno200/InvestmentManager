using Quartz;
using InvestmentManager.Shared.Utilities.Abstractions;
using InvestmentManager.Shared.Utilities.Constants;
using Job.Notification.Entities;
using Microsoft.Extensions.Options;
using MimeKit;
using Job.Notification.Settings;
using MailKit.Net.Smtp;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using MailKit.Security;

namespace Job.Notification.Jobs;

[DisallowConcurrentExecution]
public class SendEmailNotificationJob(
    IRepository<User> userRepository,
    IRepository<Product> productRepository,
    IOptions<EmailSettings> _emailSettings) : IJob
{
    private readonly IRepository<User> _userRepository = userRepository;
    private readonly IRepository<Product> _productRepository = productRepository;
    private readonly EmailSettings _emailSettings = _emailSettings.Value;

    public async Task Execute(IJobExecutionContext context)
    {
        var adminUsers = await _userRepository.FilterByAsync(user => user.Role == Role.Admin);

        var expiringProductsFilter = Builders<Product>.Filter.And(
            Builders<Product>.Filter.Gte(product => product.MaturityDate, DateTime.Today),
            Builders<Product>.Filter.Lte(product => product.MaturityDate, DateTime.Today.AddDays(7))
        );
        var expiringProducts = await _productRepository.GetFilteredAsync(expiringProductsFilter);

        await SendMailAsync(expiringProducts, adminUsers);
    }

    private async Task SendMailAsync(IEnumerable<Product> products, IEnumerable<User> adminUsers)
    {
        var expiringProductsText = string.Join(", ", products.Select(product => product.Name));

        using var client = new SmtpClient()
        {
            ServerCertificateValidationCallback = (s, c, h, e) => true
        };
        
        await client.ConnectAsync(_emailSettings.SmtpServer, _emailSettings.SmtpPort, SecureSocketOptions.None);
        await client.AuthenticateAsync(_emailSettings.SmtpUser, _emailSettings.SmtpPass);

        foreach (var adminUser in adminUsers)
        {
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress(_emailSettings.SenderName, _emailSettings.SenderEmail));
            message.To.Add(new MailboxAddress(adminUser.Username, adminUser.Email));
            message.Subject = "Notificação - Produtos próximos ao vencimento";
            message.Body = new TextPart("plainText")
            {
                Text = $"Os produtos: {expiringProductsText} estão próximos do vencimento"
            };

            await client.SendAsync(message);
        }

        await client.DisconnectAsync(true);
    }
}
