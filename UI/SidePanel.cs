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

        public event Action<string> OnAircraftSelectedEvent;

        public override void _Ready()
        {
            Color = new Color("#222222");
            CustomMinimumSize = new Vector2(250, 0);

            _trafficList = new ItemList();
            _trafficList.SetAnchorsPreset(LayoutPreset.FullRect);
            _trafficList.SizeFlagsHorizontal = SizeFlags.ExpandFill;
            _trafficList.SizeFlagsVertical = SizeFlags.ExpandFill;
            _trafficList.AddThemeColorOverride("font_color", Colors.LightGreen);
            AddChild(_trafficList);

            _trafficList.ItemSelected += OnAircraftSelected;
        }

        public void UpdateList(List<Aircraft> aircrafts)
        {
            _currentAircraft = aircrafts;
            _trafficList.Clear();

            foreach (var plane in aircrafts)
            {
                string info = $"✈️ {plane.Callsign} | FL{plane.Altitude / 100} | {plane.Speed}kt";
                _trafficList.AddItem(info);
            }
        }

        private void OnAircraftSelected(long index)
        {
            var selectedPlane = _currentAircraft[(int)index];
            OnAircraftSelectedEvent?.Invoke(selectedPlane.Callsign);
        }
    }
}