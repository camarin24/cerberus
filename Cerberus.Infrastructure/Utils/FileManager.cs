using System.IO;
using System.Threading.Tasks;
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

    public async Task UploadFile(Stream fileObject, string fileName)
    {
        var storage = await StorageClient.CreateAsync();
        await storage.UploadObjectAsync(_configuration.GetSection("GCP:BucketName").Value,
            fileName, null, fileObject);
    }

    public async Task UploadFile(string filePath, string fileName)
    {
        var storage = await StorageClient.CreateAsync();
        await using var fileStream = File.OpenRead(filePath);
        await storage.UploadObjectAsync(_configuration.GetSection("GCP:BucketName").Value, fileName,
            null, fileStream);
    }
}