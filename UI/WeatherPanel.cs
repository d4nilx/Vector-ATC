using Godot;
using VectorATC.Core; 

namespace VectorATC.UI 
{
    public partial class WeatherPanel : Control
    {
        public Main Main { get; set; }

        private HSlider _windDirSlider;
        private HSlider _windSpdSlider;
        private HSlider _qnhSlider;
        private HSlider _cloudSlider;
        private HSlider _cloudBaseSlider;

        private Label _metarLabel;

        public override void _Ready()
        {
            CustomMinimumSize = new Vector2(0, 110);
            ClipContents = true;

            var bg = new ColorRect();
            bg.Color = new Color("#0d1f2d");
            bg.SetAnchorsPreset(LayoutPreset.FullRect);
            AddChild(bg);

            var layout = new VBoxContainer();
            layout.SetAnchorsPreset(LayoutPreset.FullRect);
            AddChild(layout);

            _metarLabel = new Label();
            _metarLabel.Text = "EPWA ---/--kt Q---- 9999 SKC";
            _metarLabel.AddThemeColorOverride("font_color", new Color("#00ffcc"));
            layout.AddChild(_metarLabel);

            var slidersRow = new HBoxContainer();
            layout.AddChild(slidersRow);

            _windDirSlider = AddSlider(slidersRow, "WND DIR", 0, 359, 1);
            _windSpdSlider = AddSlider(slidersRow, "WND SPD", 0, 60, 1);
            _qnhSlider = AddSlider(slidersRow, "QNH", 940, 1040, 1);
            _cloudSlider = AddSlider(slidersRow, "CLOUD", 0, 8, 1);
            _cloudBaseSlider = AddSlider(slidersRow, "BASE ft", 500, 10000, 100);
        }

        private HSlider AddSlider(HBoxContainer parent, string labelText, float min, float max, float step)
        {
            var col = new VBoxContainer();
            col.SizeFlagsHorizontal = SizeFlags.ExpandFill;

            var label = new Label();
            label.Text = labelText;
            label.AddThemeColorOverride("font_color", Colors.LightGray);
            label.HorizontalAlignment = HorizontalAlignment.Center;
            col.AddChild(label);

            var slider = new HSlider();
            slider.MinValue = min;
            slider.MaxValue = max;
            slider.Step = step;
            slider.SizeFlagsHorizontal = SizeFlags.ExpandFill;
            slider.ValueChanged += (_) => OnSliderChanged();
            col.AddChild(slider);

            var valueLabel = new Label();
            valueLabel.Name = labelText + "_val";
            valueLabel.HorizontalAlignment = HorizontalAlignment.Center;
            valueLabel.AddThemeColorOverride("font_color", new Color("#00ffcc"));
            col.AddChild(valueLabel);

            parent.AddChild(col);
            return slider;
        }

        private void OnSliderChanged()
        {
            if (Main == null) return;

            var w = Main.Weather;
            w.WindDirection = (int)_windDirSlider.Value;
            w.WindSpeedKt = (int)_windSpdSlider.Value;
            w.QnhHpa = (int)_qnhSlider.Value;
            w.CloudCoverageOktas = (int)_cloudSlider.Value;
            w.CloudBaseft = (int)_cloudBaseSlider.Value;

            UpdateValueLabels();
            _metarLabel.Text = w.ToMetarString();
        }

        private void UpdateValueLabels()
        {
            if (Main == null) return;
            var w = Main.Weather;

            UpdateSliderValueText(_windDirSlider, $"{w.WindDirection:000}°");
            UpdateSliderValueText(_windSpdSlider, $"{w.WindSpeedKt}kt");
            UpdateSliderValueText(_qnhSlider, $"{w.QnhHpa}hPa");
            UpdateSliderValueText(_cloudSlider, $"{w.CloudCoverageOktas}/8");
            UpdateSliderValueText(_cloudBaseSlider, $"{w.CloudBaseft}ft");
        }

        private void UpdateSliderValueText(HSlider slider, string text)
        {
            var col = slider.GetParent() as VBoxContainer;
            if (col == null) return;
            var label = col.GetChild(2) as Label;
            if (label != null) label.Text = text;
        }

        public override void _Process(double delta)
        {
            if (Main != null && _metarLabel.Text.Contains("---"))
            {
                var w = Main.Weather;
                _windDirSlider.Value = w.WindDirection;
                _windSpdSlider.Value = w.WindSpeedKt;
                _qnhSlider.Value = w.QnhHpa;
                _cloudSlider.Value = w.CloudCoverageOktas;
                _cloudBaseSlider.Value = w.CloudBaseft;
                UpdateValueLabels();
                _metarLabel.Text = w.ToMetarString();
            }
        }
    }
}