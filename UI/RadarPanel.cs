using Godot;
using VectorATC.Core;

public partial class RadarPanel : Control
{
    private Aircraft _testAircraft;

    public override void _Ready()
    {
        ClipContents = true;
        
        SizeFlagsVertical = SizeFlags.ExpandFill;
        CustomMinimumSize = new Vector2(0, 300);

        var b738 = AircraftTypeDatabase.Common[0];
        _testAircraft = new Aircraft("SQP123", b738, 100, 100, 0, 80, 5000);

        var plan = new FlightPlan("EPWA", "EPGD");
        plan.Route.Add(new Waypoint("WPT1", 600, 80));
        plan.Route.Add(new Waypoint("WPT2", 1000, 220));
        plan.Route.Add(new Waypoint("WPT3", 1400, 80));
        _testAircraft.FlightPlan = plan;
    }

    public override void _Process(double delta)
    {
        _testAircraft.Move((float)delta);
        QueueRedraw(); // says to Godot to "redraw"
    }

    public override void _Draw()
    {
        DrawRect(new Rect2(Vector2.Zero, Size), new Color("#0a0a0a"));
        
        var position = new Vector2(_testAircraft.X, _testAircraft.Y);
        DrawCircle(position, 5f, Colors.LimeGreen);
        
        DrawString(ThemeDB.FallbackFont, position + new Vector2(8, -8), 
            _testAircraft.Callsign, HorizontalAlignment.Left, -1, 14, Colors.LimeGreen);
    }
}