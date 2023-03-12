namespace CommandService.Events;

public class PlatformPublishedEvent
{
    public const string Event = "Platform_Published";

    public int Id { get; set; }
    public string Name { get; set; }
}