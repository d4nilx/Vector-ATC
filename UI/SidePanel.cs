using Godot;

public partial class SidePanel : ColorRect
{
    public override void _Ready()
    {
        Color = new Color("#222222");
        CustomMinimumSize = new Vector2(250, 0);
    }
}