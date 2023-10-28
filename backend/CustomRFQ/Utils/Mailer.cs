using System.Net.Mail;
using System.Net;

namespace CustomRFQ.Utils;

public class Mailer : IDisposable
{
    private readonly ILogger<Worker> _logger;
    public Mailer(ILogger<Worker> logger)
    {
        _logger = logger; 
    }

    public void Send(string toAddress)
    {
        try
        {
            // Configurar as credenciais SMTP e o servidor de email
            var smtpClient = new SmtpClient("smtp.office365.com")
            {
                Port = 587,
                Credentials = new NetworkCredential("email@aqui", "senha"),
                EnableSsl = true
            };

            // Criar uma mensagem de email
            var mensagem = new MailMessage
            {
                From = new MailAddress("email@aqui"),
                Subject = "Assunto do Email",
                Body = "Conteúdo do Email",
            };

            // Adicionar destinatários
            mensagem.To.Add("email@aqui");

            // Enviar o email
            smtpClient.Send(mensagem);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message);
        }
    }

    public void Dispose()
    {
        GC.SuppressFinalize(this);
    }
}
