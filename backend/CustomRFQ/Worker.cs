using CustomRFQ.Databases;
using CustomRFQ.Models;
using CustomRFQ.Utils;
using System.Text.Json;

public class Worker : BackgroundService
{
    private readonly ILogger<Worker> _logger;
    private readonly Context _context;

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

                var employessObj = JsonSerializer.Deserialize<ServiceLayer.BusinessPartner.Value>(
                    slInstance.SLApi.Get($"BusinessPartners('{quotationObj.CardCode}')").Result.success);

                if (employessObj.ContactEmployees.Count > 0)
                {
                    foreach (var employee in employessObj.ContactEmployees)
                    {
                        if (string.IsNullOrEmpty(employee.E_Mail) || string.IsNullOrEmpty(employee.U_RSD_CustomRFQ) || employee.U_RSD_CustomRFQ == "N")
                            continue;

                        using (Mailer mailer = new(_logger))
                        {
                            Task.Run(() => mailer.Send(employee.E_Mail));
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