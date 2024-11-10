using MekLatexTranslationLibrary;
using System.Net.WebSockets;

bool canContinue = true;

Console.OutputEncoding = System.Text.Encoding.UTF8;
Console.WriteLine("Translation Lib version: " + 
    typeof(TranslationItem).Assembly.GetName().Version?.ToString() ?? "NULL");
while (canContinue)
{
    canContinue = RunTranslationProcess();
}
Console.WriteLine("exit");
Console.ReadKey();


static bool RunTranslationProcess()
{
    Console.Write("leave empty to exit or give input: ");
    var input = Console.ReadLine();
    if (string.IsNullOrWhiteSpace(input)) return false;
   


    TranslationItem item = new(input, new()
    {
        UnitTranslationMode = UnitTranslationMode.Translate,
    });

    TranslationResult result = LatexTranslation.Translate(item);
    Console.WriteLine();
    Console.WriteLine($"Translation result: {result.Result}");
    Console.WriteLine();
    Console.WriteLine($"Error codes: {result.ErrorFlags.ToErrorString()}");
    Console.WriteLine();
    return true;
}