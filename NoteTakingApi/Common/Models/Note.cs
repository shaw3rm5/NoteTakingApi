namespace NoteTakingApi.Common.Models;

public class Note
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public string Title { get; private set; }
    public string Content { get; private set; }
    public bool IsDeleted { get; private set; }
    public DateTimeOffset CreatedAt { get; private set; }
    public DateTimeOffset UpdatedAt { get; private set; }
    public ICollection<Tag> Tags { get; set; }
    
    public Note()
    {
    }

    public static Note Create(int userId, string title, string content)
    {
        return new Note
        {
            UserId = userId,
            Title = title,
            Content = content,
            IsDeleted = false,
            CreatedAt = DateTimeOffset.UtcNow,
        };
    }

    public void Update(string title, string content)
    {
        if (IsDeleted)
            //TODO: throw custom exception
            return;

        Title = title;
        Content = content;
        UpdatedAt = DateTimeOffset.UtcNow;
    }
    
    public void Delete() =>
        IsDeleted = true;
    
}