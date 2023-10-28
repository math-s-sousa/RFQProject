using System.Net.Mail;
using System.Net;
using CustomRFQ.Models;

namespace CustomRFQ.Utils;

public class Mailer : IDisposable
{
    private readonly ILogger<Worker> _logger;
    public Mailer(ILogger<Worker> logger)
    {
        _logger = logger; 
    }

    public void Send(Database.Smtp smtp, string toAddress)
    {
        try
        {
            // Configurar as credenciais SMTP e o servidor de email
            var smtpClient = new SmtpClient(smtp.Server)
            {
                Port = smtp.Port,
                Credentials = new NetworkCredential(smtp.Host, smtp.Password),
                EnableSsl = smtp.SSL
            };

            // Criar uma mensagem de email
            var mensagem = new MailMessage
            {
                From = new MailAddress(smtp.Host),
                Subject = "Assunto do Email",
                Body = "Conteúdo do Email",
            };

            // Adicionar destinatários
            mensagem.To.Add("matheus.silvasousa@hotmail.com");

            // Enviar o email
            smtpClient.Send(mensagem);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message);
        }
        finally
        {
            _logger.LogInformation("Email enviado!");
        }
    }

    public void Dispose()
    {
        GC.SuppressFinalize(this);
    }
}
