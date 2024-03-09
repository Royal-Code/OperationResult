using System.Text;

namespace RoyalCode.OperationResults.TestApi.Application.ResultsModels;

public class SimpleValues
{
    public int Number { get; set; } = Random.Shared.Next(1, 100_000);

    public string Text { get; set; } = GenerateRandomText();

    private static string GenerateRandomText()
    {
        var length = Random.Shared.Next(1, 100);
        var text = new StringBuilder(length);
        for (var i = 0; i < length; i++)
        {
            text.Append((char)Random.Shared.Next(65, 90));
        }

        return text.ToString();
    }
}
