using LibraryManagementAPI.DTOs;

namespace LibraryManagementAPI.Services
{
    /*Interface is nothing a fixed logic class
    that is called by diffrent services
    so that diffrent services like fakeDB or database
    can call this class to run the logic in it
    that is supported directly by the controller
    in short , we made work of the controller easy
    now the controller will only handle requests , but the 
    bussines logic is written in the services...*/
    public interface IBookService
    {
        List<BookDto> GetAllBooks();
        BookDto? GetBookById(int id);
        BookDto AddBook(CreateBookDto dto);
        BookDto? UpdateBook(int id, UpdateBookDto dto);
        bool DeleteBook(int id);
        object GetAvailability(int id);
    }
}
