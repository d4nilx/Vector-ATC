using Godot;

public partial class RadarPanel : ColorRect
{
    public override void _Ready()
    {
        Color = new Color("#0a0a0a");
        SizeFlagsVertical = SizeFlags.ExpandFill;
    }
}