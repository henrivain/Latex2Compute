using Latex2Compute;


Console.OutputEncoding = System.Text.Encoding.UTF8;
Console.WriteLine("Translation Lib version: " + 
    typeof(TranslationItem).Assembly.GetName().Version?.ToString() ?? "NULL");

while (true)
{
    if (RunTranslationProcess() is false)
    {
        break;
    }
}

Console.WriteLine("exit");
Console.ReadKey();





static bool RunTranslationProcess()
{
    Console.Write("Give input. Leave empty to translate or type exit to quit >\n");
    var input = ReadInput();
    if (input is null)
    {
        return false;
    }

    TranslationArgs args = TranslationArgs.GetDefault();
    args.TargetSystem = TargetSystem.Matlab;

    TranslationItem item = new(input, args);

    TranslationResult result = LatexTranslation.Translate(item);
    Console.WriteLine();
    Console.WriteLine($"Translation result: {result.Result}");
    Console.WriteLine();
    Console.WriteLine($"Error codes: {result.ErrorFlags.ToErrorString()}");
    Console.WriteLine();
    return true;
}

static string? ReadInput()
{
    List<string> lines = new();
    while (true)
    {
        string? line = Console.ReadLine();
        if (line is "quit")
        {
            return null;
        }
        if (string.IsNullOrEmpty(line))
        {
            return string.Join("", lines);
        }
        lines.Add(line);
    }
}