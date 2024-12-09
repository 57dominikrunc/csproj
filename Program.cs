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

    private static void copyAlwaysXml(XElement root, string fileName)
    {
        root.Add(
            new XText("  "),
            new XElement(
                "ItemGroup",
                new XText("\n    "),
                new XElement(
                    "None",
                    new XAttribute("Update", fileName),
                    new XText("\n      "),
                    new XElement("CopyToOutputDirectory", "Always"),
                    new XText("\n    ")
                ),
                new XText("\n  ")
            ),
            new XText("\n\n")
        );
    }

    static void CopyAlways(FileInfo file)
    {
        string cwd = Directory.GetCurrentDirectory();
        string projectName = Path.GetFileName(cwd);
        string csprojName = projectName + ".csproj";
        XElement root = XElement.Load(csprojName, LoadOptions.PreserveWhitespace);
        copyAlwaysXml(root, file.Name);
        // copyAlwaysXml(root, "sigma.mp4");
        // Console.WriteLine(root);
        Console.WriteLine($"Added \"{file.Name}\" to \"{csprojName}\"");
        root.Save(csprojName);
    }
}
