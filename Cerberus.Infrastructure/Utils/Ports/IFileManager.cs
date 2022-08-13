using System.Threading.Tasks;

namespace Cerberus.Infrastructure.Utils.Ports;

public interface IFileManager
{
   Task<string> UploadProfilePicture(string userName);
}