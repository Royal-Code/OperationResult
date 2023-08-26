namespace ValidableResultSamples;

public interface IBookStore
{
    /// <summary>
    /// Creates a new book and returns the id.
    /// </summary>
    /// <param name="book"></param>
    /// <returns></returns>
    Guid Create(BookDto book);
}