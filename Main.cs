using Godot;
using VectorATC.Core;

namespace VectorATC.UI 
{
    public partial class Main : Control
    {
        private WeatherEngine _weatherEngine = new WeatherEngine();
        public WeatherData Weather => _weatherEngine.Current;

        public override void _Ready()
        {
            _weatherEngine.Randomize();

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

            weatherPanel.Main = this;
            radarPanel.Main = this;

            radarPanel.OnTrafficUpdated += sidePanel.UpdateList;
            sidePanel.OnAircraftSelectedEvent += controlPanel.SetPrefix;
            controlPanel.OnCommandEntered += radarPanel.ProcessCommand;

            leftPanel.AddChild(weatherPanel);
            leftPanel.AddChild(radarPanel);
            leftPanel.AddChild(controlPanel);
            mainLayout.AddChild(sidePanel);
        }
    }
}