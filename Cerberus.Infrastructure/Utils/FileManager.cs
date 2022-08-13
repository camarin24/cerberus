using System.IO;
using System.Threading.Tasks;
using Cerberus.Infrastructure.Extensions;
using Cerberus.Infrastructure.Utils.Ports;
using Google.Cloud.Storage.V1;
using Microsoft.Extensions.Configuration;

namespace Cerberus.Infrastructure.Utils;

public class FileManager : IFileManager
{
    private readonly IConfiguration _configuration;

    public FileManager(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public async Task<string> UploadProfilePicture(string userName)
    {
        var (filePath, fileName) = await WebUtils.DownloadFile(
            string.Format(_configuration.GetSectionValue("Authorization:ProfilePictureUrl"), userName), "svg");
        var storage = await StorageClient.CreateAsync();
        await using var fileStream = File.OpenRead(filePath);
        var bucket = _configuration.GetSectionValue("GCP:BucketName");
        await storage.UploadObjectAsync(bucket, fileName, "image/svg+xml", fileStream);
        return $"https://storage.googleapis.com/{bucket}/{fileName}";
    }
}