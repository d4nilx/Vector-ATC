using Godot;
using System;

namespace VectorATC.UI
{
    public partial class ControlPanel : ColorRect
    {
        private LineEdit _inputField;
        
        public event Action<string> OnCommandEntered;

        public override void _Ready()
        {
            Color = new Color("#333333"); 
            CustomMinimumSize = new Vector2(0, 50); 

            var margin = new MarginContainer();
            margin.SetAnchorsPreset(LayoutPreset.FullRect);
            margin.AddThemeConstantOverride("margin_left", 15);
            margin.AddThemeConstantOverride("margin_right", 15);
            margin.AddThemeConstantOverride("margin_top", 10);
            margin.AddThemeConstantOverride("margin_bottom", 10);
            AddChild(margin);

            _inputField = new LineEdit();
            _inputField.PlaceholderText = "ATC COMMAND (e.g., SQP112 HDG 180)...";
            _inputField.SizeFlagsHorizontal = SizeFlags.ExpandFill;
            _inputField.AddThemeColorOverride("font_color", Colors.LimeGreen);
            
            margin.AddChild(_inputField);

            _inputField.TextSubmitted += ProcessInput;
        }

        public void SetPrefix(string callsign)
        {
            _inputField.Text = $"{callsign} ";
            _inputField.GrabFocus(); 
            _inputField.CaretColumn = _inputField.Text.Length; 
        }

        private void ProcessInput(string text)
        {
            if (string.IsNullOrWhiteSpace(text)) return;
            
            OnCommandEntered?.Invoke(text.ToUpper()); 
            _inputField.Clear(); 
        }
    }
}