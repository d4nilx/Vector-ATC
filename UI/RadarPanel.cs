using Godot;
using System.Collections.Generic;

public partial class RadarPanel : Control
{
    private List<Aircraft> _aircraft = new List<Aircraft>();
    private TrafficSpawner _spawner;

    public override void _Ready()
    {
        SizeFlagsVertical = SizeFlags.ExpandFill;
        CustomMinimumSize = new Vector2(0, 300);
        ClipContents = true;

        _spawner = new TrafficSpawner(Size.X, Size.Y);
    }

    public override void _Process(double delta)
    {
        _spawner.Update((float)delta, _aircraft);

        foreach (var plane in _aircraft)
            plane.Move((float)delta);

        GD.Print($"Aircraft count: {_aircraft.Count}");

        QueueRedraw();
    }

    public override void _Draw()
    {
        DrawRect(new Rect2(Vector2.Zero, Size), new Color("#0a0a0a"));

        foreach (var plane in _aircraft)
        {
            var position = new Vector2(plane.X, plane.Y);
            DrawCircle(position, 5f, Colors.LimeGreen);
            DrawString(ThemeDB.FallbackFont, position + new Vector2(8, -8),
                plane.Callsign, HorizontalAlignment.Left, -1, 14, Colors.LimeGreen);
        }
    }
}