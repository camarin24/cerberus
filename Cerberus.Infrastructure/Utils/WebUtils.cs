using System;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;

namespace Cerberus.Infrastructure.Utils;

public static class WebUtils
{
    public static async Task<(string, string)> DownloadFile(string url, string extension)
    {
        var uri = new Uri(url);
        Guid name = Guid.NewGuid();
        using var client = new HttpClient();
        var fileName = $"{name}.{extension}";
        var filePath = $"/tmp/{fileName}";
        await client.DownloadFileAsync(uri, filePath);
        return (filePath, fileName);
    }

    private static async Task DownloadFileAsync(this HttpClient client, Uri uri, string fileName)
    {
        await using var s = await client.GetStreamAsync(uri);
        await using var fs = new FileStream(fileName, FileMode.CreateNew);
        await s.CopyToAsync(fs);
    }
}