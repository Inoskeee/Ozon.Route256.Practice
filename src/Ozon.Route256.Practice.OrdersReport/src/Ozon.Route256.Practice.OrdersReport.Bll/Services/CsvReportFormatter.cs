using System.Globalization;
using System.Text;
using CsvHelper;
using CsvHelper.Configuration;
using Ozon.Route256.Practice.OrdersReport.Bll.Contracts.Models;
using Ozon.Route256.Practice.OrdersReport.Bll.Contracts.Services;

namespace Ozon.Route256.Practice.OrdersReport.Bll.Services;

public class CsvReportFormatter : IReportFormatter
{
    public async Task<StringBuilder> FormatReport(List<ReportModel> reportModel)
    {
        var csvData = new StringBuilder();

        using (var stringWriter = new StringWriter(csvData))
        {
            using (var csv = new CsvWriter(stringWriter, new CsvConfiguration(CultureInfo.InvariantCulture)))
            {
                csv.WriteHeader<ReportModel>();
                await csv.NextRecordAsync();

                foreach (var order in reportModel)
                {
                    csv.WriteRecord(order);
                    await csv.NextRecordAsync();
                }
            }
        }

        return csvData;
    }
}