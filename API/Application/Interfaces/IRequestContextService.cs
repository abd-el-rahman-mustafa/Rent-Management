namespace API.Application.Interfaces;
public interface IRequestContext
{
    string Language { get; }
    string Platform { get; }
    
    // add more header-derived props here later
}