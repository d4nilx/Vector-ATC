using Godot;

public partial class WeatherPanel : ColorRect
{
    public override void _Ready()
    {
        Color = new Color("#2244aa");
        CustomMinimumSize = new Vector2(0, 80);
    }
}