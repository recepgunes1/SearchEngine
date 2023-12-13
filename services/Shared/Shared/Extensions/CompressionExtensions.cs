using System.IO.Compression;
using System.Text;

namespace Shared.Extensions;

public static class CompressionExtensions
{
    public static byte[] Compress(this string uncompressed)
    {
        using var outputMemory = new MemoryStream();
        using (var gz = new GZipStream(outputMemory, CompressionLevel.Optimal))
        {
            using (var sw = new StreamWriter(gz, Encoding.UTF8))
            {
                sw.Write(uncompressed);
            }
        }

        return outputMemory.ToArray();
    }

    public static string Decompress(this byte[] compressed)
    {
        using var inputMemory = new MemoryStream(compressed);
        using var gz = new GZipStream(inputMemory, CompressionMode.Decompress);
        using var sr = new StreamReader(gz, Encoding.UTF8);
        var sb = new StringBuilder();
        var buffer = new char[8192];
        int numRead;
        while ((numRead = sr.Read(buffer, 0, buffer.Length)) != 0) sb.Append(buffer, 0, numRead);
        return sb.ToString();
    }
}