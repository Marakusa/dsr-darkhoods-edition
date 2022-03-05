using System;
using System.IO;
using System.IO.Compression;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace DarkHoodsInstaller;

internal class Program
{
    static void Main()
    {
        var t = Task.Run(() => Run());
        t.Wait();
    }

    static async Task Run()
    {
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine();
        Console.WriteLine();
        Console.WriteLine("============================");
        Console.WriteLine();
        Console.WriteLine();
        Console.WriteLine("Dark Hoods installer");
        Console.WriteLine();
        Console.WriteLine();
        Console.WriteLine("============================");
        Console.WriteLine();
        Console.WriteLine();
        Console.ForegroundColor = ConsoleColor.Blue;

        string location = @"C:\Program Files (x86)\Steam\steamapps\common\DARK SOULS REMASTEREDs\";
        while (!File.Exists(Path.Join(location, @"\DarkSoulsRemastered.exe")))
        {
            Console.WriteLine("Please give Dark Souls: Remastered directory path (not .exe):");
            Console.Write("> ");
            location = Console.ReadLine();
        }

        Console.WriteLine($"Installing mod to '{location}'...");

        string menu = Path.Join(location, @"\menu");
        string msg = Path.Join(location, @"\msg");

        string download = "https://github.com/Marakusa/dsr-darkhoods-edition/raw/main/files/files.zip";
        using var client = new HttpClient();
        Console.WriteLine($"Downloading '{Path.Join(location, @"\files.zip")}'...");
        var result = await client.GetAsync(download);
        using (var fs = new FileStream(Path.Join(location, @"\files.zip"), FileMode.CreateNew))
        {
            await result.Content.CopyToAsync(fs);
        }

        using (ZipArchive zip = ZipFile.OpenRead(Path.Join(location, @"\files.zip")))
        {
            // here, we extract every entry, but we could extract conditionally
            // based on entry name, size, date, checkbox status, etc.  
            foreach (var e in zip.Entries)
            {
                e.ExtractToFile(Path.Join(location, e.Name), true);
            }
        }

        Console.ForegroundColor = ConsoleColor.Gray;
    }
}
