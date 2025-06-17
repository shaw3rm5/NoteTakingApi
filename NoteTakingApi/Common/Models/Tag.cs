namespace NoteTakingApi.Common.Models;

public class Tag
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public ICollection<Note> Notes { get; set; }
}