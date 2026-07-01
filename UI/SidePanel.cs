using Godot;
using System;
using System.Collections.Generic;
using VectorATC.Core;

namespace VectorATC.UI
{
    public partial class SidePanel : ColorRect
    {
        private ItemList _trafficList;
        private List<Aircraft> _currentAircraft = new List<Aircraft>();
        private float _uiRefreshTimer; 

        public event Action<string> OnAircraftSelectedEvent;

        public override void _Ready()
        {
            Color = new Color("#222222");
            CustomMinimumSize = new Vector2(250, 0);

            // Container for tabs 
            var tabContainer = new TabContainer();
            tabContainer.SetAnchorsPreset(LayoutPreset.FullRect);
            tabContainer.AddThemeColorOverride("font_selected_color", Colors.White);
            tabContainer.AddThemeColorOverride("font_unselected_color", Colors.Gray);
            AddChild(tabContainer);

            // Tab 1: Schedule
            var schedulePanel = new RichTextLabel();
            schedulePanel.Name = "SCHEDULE"; 
            schedulePanel.BbcodeEnabled = true; 
            schedulePanel.SizeFlagsHorizontal = SizeFlags.ExpandFill;
            schedulePanel.SizeFlagsVertical = SizeFlags.ExpandFill;
            
            var margin = new MarginContainer();
            margin.Name = "SCHEDULE";
            margin.AddThemeConstantOverride("margin_top", 10);
            margin.AddThemeConstantOverride("margin_left", 10);
            margin.AddThemeConstantOverride("margin_right", 10);
            margin.AddChild(schedulePanel);
            
            tabContainer.AddChild(margin);
            
            schedulePanel.Text = "[b][color=#ffdd00]UPCOMING FLIGHTS[/color][/b]\n\n" +
                                 "[color=#00ffcc]SQP112[/color] | A320\n" +
                                 "DEP: LWO  ARR: EPWA\n" +
                                 "PAX: 154  |  FUEL: 6.2T\n" +
                                 "----------------------\n" +
                                 "[color=#00ffcc]RYR802[/color] | B738\n" +
                                 "DEP: STN  ARR: EPWA\n" +
                                 "PAX: 189  |  FUEL: 7.1T\n" +
                                 "----------------------\n" +
                                 "[color=#00ffcc]LOT331[/color] | E190\n" +
                                 "DEP: CDG  ARR: EPWA\n" +
                                 "PAX: 92   |  FUEL: 4.5T";
            
            // Tab 2: Active control traffic list
            _trafficList = new ItemList();
            _trafficList.Name = "TRAFFIC"; 
            _trafficList.SizeFlagsHorizontal = SizeFlags.ExpandFill;
            _trafficList.SizeFlagsVertical = SizeFlags.ExpandFill;
            _trafficList.AddThemeColorOverride("font_color", Colors.LightGreen);
            
            tabContainer.AddChild(_trafficList);
            _trafficList.ItemClicked += OnAircraftClicked;

            // Tab 3: Settings
            var settingsTab = new MarginContainer();
            settingsTab.Name = "SETTINGS"; 
            settingsTab.AddThemeConstantOverride("margin_top", 15);
            settingsTab.AddThemeConstantOverride("margin_left", 15);
            settingsTab.AddThemeConstantOverride("margin_right", 15);
            tabContainer.AddChild(settingsTab);

            var settingsLayout = new VBoxContainer();
            settingsTab.AddChild(settingsLayout);

            var diffLabel = new Label { Text = "SPAWN RATE (Difficulty)" };
            diffLabel.AddThemeColorOverride("font_color", Colors.LightGray);
            settingsLayout.AddChild(diffLabel);

            var diffSlider = new HSlider();
            diffSlider.MinValue = 10;
            diffSlider.MaxValue = 120;
            diffSlider.Value = 45; 
            settingsLayout.AddChild(diffSlider);
        }

        public void UpdateList(List<Aircraft> aircrafts)
        {
            _currentAircraft = aircrafts;
            _trafficList.Clear();

            for (int i = 0; i < aircrafts.Count; i++)
            {
                _trafficList.AddItem(""); 
            }
            ForceUpdateText();
        }

        private void ForceUpdateText()
        {
            for (int i = 0; i < _currentAircraft.Count; i++)
            {
                if (i < _trafficList.ItemCount)
                {
                    var plane = _currentAircraft[i];
                    string info = $"✈️ {plane.Callsign} | FL{plane.Altitude / 100} | {(int)plane.Speed}kt";
                    _trafficList.SetItemText(i, info);
                }
            }
        }

        public override void _Process(double delta)
        {
            _uiRefreshTimer += (float)delta;
            if (_uiRefreshTimer >= 0.5f)
            {
                _uiRefreshTimer = 0f;
                ForceUpdateText();
            }
        }

        private void OnAircraftClicked(long index, Vector2 atPosition, long mouseButtonIndex)
        {
            if (mouseButtonIndex == 1) 
            {
                var selectedPlane = _currentAircraft[(int)index];
                OnAircraftSelectedEvent?.Invoke(selectedPlane.Callsign);
            }
        }
    }
}