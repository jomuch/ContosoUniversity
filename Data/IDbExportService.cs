using System.Xml.Linq;

namespace ContosoUniversity.Data
{
    public interface IDbExportService
    {
        XDocument ExportToXml();
    }
}