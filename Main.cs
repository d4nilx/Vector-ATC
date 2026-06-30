using Godot;
using VectorATC.Core;

namespace VectorATC.UI 
{
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

            var weatherPanel = new WeatherPanel();
            var radarPanel = new RadarPanel();
            var controlPanel = new ControlPanel();
            var sidePanel = new SidePanel();

            radarPanel.OnTrafficUpdated += sidePanel.UpdateList;

            leftPanel.AddChild(weatherPanel);
            leftPanel.AddChild(radarPanel);
            leftPanel.AddChild(controlPanel);

            mainLayout.AddChild(sidePanel);
        }
    }
}