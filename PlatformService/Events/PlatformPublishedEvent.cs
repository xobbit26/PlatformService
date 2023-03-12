namespace PlatformService.Events;

public class PlatformPublishedEvent
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Event { get; set; } = "Platform_Published";
}