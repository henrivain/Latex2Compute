using MekLatexTranslationLibrary;

bool canContinue = true;
Console.OutputEncoding = System.Text.Encoding.UTF8;
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
    

    TranslationItem item = new(input, new());

    TranslationResult result = Translation.MakeNormalTranslation(item);
    Console.WriteLine();
    Console.WriteLine($"Translation result: {result.Result}");
    Console.WriteLine();
    Console.WriteLine($"Error codes: {result.ErrorCodes}");
    Console.WriteLine();
    return true;
}