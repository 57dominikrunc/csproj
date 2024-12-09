using System.CommandLine;
using System.Xml.Linq;

namespace csproj;

class Program
{
    static async Task<int> Main(string[] args)
    {
        var copyOption = new Option<FileInfo?>(
            name: "--copy",
            description: "The file to add to .csproj and set to \"Copy Always\"."
        );

        var rootCommand = new RootCommand("A tool that makes modifying .csproj files easier");
        rootCommand.AddOption(copyOption);
        rootCommand.SetHandler(
            (file) =>
            {
                CopyAlways(file!);
            },
            copyOption
        );

        return await rootCommand.InvokeAsync(args);
    }

    static void CopyAlways(FileInfo file)
    {
        DirectoryInfo parent = System.IO.Directory.GetParent(file.FullName);
        Console.WriteLine(file.FullName);
        XElement root = XElement.Load("../../../csproj.csproj");
        Console.WriteLine(root);
    }
}
