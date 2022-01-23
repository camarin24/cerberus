using System.IO;
using System.Threading.Tasks;

namespace Cerberus.Infrastructure.Utils.Ports;

public interface IFileManager
{
    Task UploadFile(Stream fileObject, string fileName);
    Task UploadFile(string filePath, string fileName);
}