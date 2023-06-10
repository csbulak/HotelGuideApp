using RabbitMQ.Client;
using Report.API.Dtos;
using System.Text;

namespace Report.API.Services;

public class RabbitMqService
{
    private readonly string _hostName;
    private readonly string _reportQueueName;
    private readonly string _reportStatusQueueName;

    public RabbitMqService(string hostName, string reportQueueName, string reportStatusQueueName)
    {
        _hostName = hostName;
        _reportQueueName = reportQueueName;
        _reportStatusQueueName = reportStatusQueueName;
    }

    public Dtos.Report CreateReport(ReportRequest request)
    {
        var report = new Dtos.Report
        {
            UUID = Guid.NewGuid().ToString(),
            RequestedDate = DateTime.Now,
            Status = "Hazırlanıyor"
            // Raporun diğer bilgilerini buraya ekleyin
        };

        PublishReportRequest(report, request);

        return report;
    }

    public Dtos.Report GetReportStatus(string uuid)
    {
        // Rapora ait durum bilgisini almak için RabbitMQ'dan veya başka bir kaynaktan sorgulama yapın
        // uuid parametresi ile raporun durumunu bulun

        // Örnek olarak sadece birkaç durum döndürelim:
        if (uuid == "12345")
        {
            return new Dtos.Report
            {
                UUID = "12345",
                RequestedDate = DateTime.Now.AddDays(-1),
                Status = "Tamamlandı"
                // Raporun diğer bilgilerini buraya ekleyin
            };
        }
        else if (uuid == "67890")
        {
            return new Dtos.Report
            {
                UUID = "67890",
                RequestedDate = DateTime.Now.AddHours(-2),
                Status = "Hazırlanıyor"
                // Raporun diğer bilgilerini buraya ekleyin
            };
        }

        return null; // Belirtilen UUID ile rapor bulunamadı
    }

    private void PublishReportRequest(Dtos.Report report, ReportRequest request)
    {
        var factory = new ConnectionFactory
        {
            HostName = _hostName
        };

        using (var connection = factory.CreateConnection())
        using (var channel = connection.CreateModel())
        {
            channel.QueueDeclare(queue: _reportQueueName, durable: false, exclusive: false, autoDelete: false, arguments: null);

            var body = Encoding.UTF8.GetBytes(request.ToString());

            channel.BasicPublish(exchange: "", routingKey: _reportQueueName, basicProperties: null, body: body);
        }
    }
}

