using Godot;

public partial class ControlPanel : ColorRect
{
    public override void _Ready()
    {
        Color = new Color("#444444");
        CustomMinimumSize = new Vector2(0, 60);
    }
}