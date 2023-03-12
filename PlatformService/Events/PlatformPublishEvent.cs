namespace PlatformService.Events;

public class PlatformPublishEvent
{
    public const string Event = "Platform_Published";

    public int Id { get; set; }
    public string Name { get; set; }
}