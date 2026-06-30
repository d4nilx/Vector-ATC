using Godot;

public partial class Main : Control
{
    public override void _Ready()
    {
        SetAnchorsPreset(LayoutPreset.FullRect);

        var mainLayout = new HBoxContainer();
        mainLayout.SetAnchorsPreset(LayoutPreset.FullRect);
        AddChild(mainLayout);

        var leftPanel = new VBoxContainer();
        leftPanel.SizeFlagsHorizontal = SizeFlags.ExpandFill;
        mainLayout.AddChild(leftPanel);

        leftPanel.AddChild(new WeatherPanel());
        leftPanel.AddChild(new RadarPanel());
        leftPanel.AddChild(new ControlPanel());

        mainLayout.AddChild(new SidePanel());
    }
}