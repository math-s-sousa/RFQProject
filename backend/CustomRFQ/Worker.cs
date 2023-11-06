using CustomRFQ.Databases;
using CustomRFQ.Models;
using CustomRFQ.Utils;
using System.Text.Json;

public class Worker : BackgroundService
{
    private readonly ILogger<Worker> _logger;
    private readonly Context _context;
    private readonly Database.Smtp _smtp;

    public Worker(ILogger<Worker> logger, Context context)
    {
        _logger = logger;
        _context = context;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            Process();

            await Task.Delay(30000, stoppingToken);
        }
    }

    void Process()
    {
        try
        {
            var getNewEvents = _context._conn.Query("SELECT * FROM \"EventSender\" WHERE \"Status\" = 'N'");

            foreach (var item in getNewEvents)
            {
                var slInstance = _context._conn._dbs.Where(a => a.DB == item.DB).FirstOrDefault();

                var quotationObj = JsonSerializer.Deserialize<ServiceLayer.MarketingDocument.Value>(
                    slInstance.SLApi.Get($"PurchaseQuotations({item.DocEntry})").Result.success);

                var salesPersonObj = JsonSerializer.Deserialize<ServiceLayer.SalesPersons.Value>(
                    slInstance.SLApi.Get($"SalesPersons({quotationObj.SalesPersonCode})").Result.success);

                var employessObj = JsonSerializer.Deserialize<ServiceLayer.BusinessPartner.Value>(
                    slInstance.SLApi.Get($"BusinessPartners('{quotationObj.CardCode}')").Result.success);

                if (employessObj.ContactEmployees.Count > 0)
                {
                    foreach (var employee in employessObj.ContactEmployees)
                    {
                        if (!string.IsNullOrEmpty(employee.E_Mail) && employee.U_RSD_CustomRFQ == "Y")
                        {
                            string html = _context._conn._smtp.Body;

                            html = html.Replace("{{Vendor}}", employee.FirstName);
                            html = html.Replace("{{Saler}}", salesPersonObj.SalesEmployeeName);
                            html = html.Replace("{{Url}}", _context._conn._smtp.BaseUrl.EndsWith('/') ? 
                                _context._conn._smtp.BaseUrl + item.Guid : _context._conn._smtp.BaseUrl + '/' + item.Guid);

                            using (Mailer mailer = new(_logger))
                            {
                                Task.Run(() => mailer.Send(_context._conn._smtp, html, employee.E_Mail));
                            }

                            _context._conn.UpdateEvent(item.Guid, 'P');
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message);
        }
    }
}