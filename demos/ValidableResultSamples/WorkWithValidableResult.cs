using Microsoft.AspNetCore.Http;
using RoyalCode.OperationResults;

namespace ValidableResultSamples;

public class WorkWithValidableResult
{
    private readonly IBookStore bookStore = default!;

    public void ValidableResult_CheckingIfIsFailure(BookDto book)
    {
        var result = Validate(book);

        var failure = result.TryGetError(out var errors);
    }

    public OperationResult ValidableResult_ReturnOperationResult(BookDto book)
    {
        var result = Validate(book);

        if (result.TryGetError(out var errors))
            return errors;

        // do something with the book

        return new();
    }

    public OperationResult<int> ValidableResult_Convert(BookDto book)
    {
        var result = Validate(book);

        return result.Convert(() => book.Pages);
    }

    public OperationResult<Guid> ValidableResult_Convert_WithParam(BookDto book)
    {
        var result = Validate(book);

        return result.Convert((store) => store.Create(book), bookStore);
    }

    public IResult ValidableResult_Match(BookDto book)
    {
        var result = Validate(book);

        return result.Match(
            () => Results.Ok("Book is valid"),
            (errors) => Results.BadRequest(errors));
    }

    private ValidableResult Validate(BookDto book)
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
