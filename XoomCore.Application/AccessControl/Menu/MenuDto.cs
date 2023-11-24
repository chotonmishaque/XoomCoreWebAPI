namespace XoomCore.Application.AccessControl.Menu;

public class MenuDto
{
    public long Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public int DisplaySequence { get; set; }
    public string Icon { get; set; }
    public EntityStatus Status { get; set; }
}
