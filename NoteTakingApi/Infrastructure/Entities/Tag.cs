using NoteTakingApi.Common.Models;

namespace NoteTakingApi.Infrastructure.Entities;

public class Tag
{
    public int Id { get; set; } 
    public string Name { get; set; }

    public ICollection<NoteTag> NoteTags { get; init; } = new List<NoteTag>();
}