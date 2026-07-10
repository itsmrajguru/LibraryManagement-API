using LibraryManagementAPI.DTOs;
using LibraryManagementAPI.Models;

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
        List<Book> GetAllBooks();
        List<Book> GetAvailableBooks();
        List<Book> GetIssuedBooks();
        Book? GetBookById(int id);
        Book AddBook(BookRequestDto bookDto);
        Book? UpdateBook(int id, BookRequestDto bookDto);
        bool DeleteBook(int id);
        (bool success, string message, Book? book) IssueBook(int id, string studentName);
        (bool success, string message, Book? book) ReturnBook(int id);
    }
}
