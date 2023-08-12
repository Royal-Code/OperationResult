
using RoyalCode.OperationResults;

namespace ValidableResultSamples;

public class CreatingValidableResults
{
    public ValidableResult Create_Success()
    {
        var result = new ValidableResult();

        return result;
    }

    public ValidableResult Create_Failure()
    {
        var result = new ValidableResult();
        result += ResultMessage.Error("Error message");

        return result;
    }

    public ValidableResult Create_Failure_FromResultErros()
    {
        var errors = new ResultErrors();
        errors += ResultMessage.Error("Error message");

        var result = new ValidableResult(errors);

        return result;
    }

    public ValidableResult Validating(BookDto book)
    {
        var result = new ValidableResult();

        if (string.IsNullOrWhiteSpace(book.Title))
            result += ResultMessage.Error("Title is required");

        if (string.IsNullOrWhiteSpace(book.Author))
            result += ResultMessage.Error("Author is required");

        if (book.Pages <= 0)
            result += ResultMessage.Error("Pages must be greater than zero");

        return result;
    }
}
