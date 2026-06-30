<div align="center">
  
# ✈️ Vector ATC

*A 2D air traffic control simulator built from scratch, aiming to recreate the real workflow of an air traffic controller.*

![Godot Engine](https://img.shields.io/badge/Godot_4.x-478CBF?style=for-the-badge&logo=godotengine&logoColor=white)
![C#](https://img.shields.io/badge/C%23-239120?style=for-the-badge&logo=c-sharp&logoColor=white)
![Status](https://img.shields.io/badge/Status-Active_Development-success?style=for-the-badge)

</div>

## 🎯 Why this exists

Most ATC simulators are either browser toys with no depth, or commercial-grade products like *Tower!3D Pro* and *VRC* that require years of development and real-world aeronautical licensing deals. 

**Vector ATC** sits right in between: a serious, from-scratch simulation of controller logic and radar work. It is built as a learning project and a long-term personal product — not just a one-week clone. The goal isn't to simply "fake" an ATC experience. It's to model the actual mechanics: aircraft state, flight plans, controller commands, and conflict logic, the exact same way a real radar system would track them (just without the certification overhead).

---

## 🚀 Current Status

The project is in early, active development. Architecture comes first, visual polish comes later.

### ✅ Done:
- **Project Scaffolded:** Godot 4 (Mono/C#) environment set up. UI layout split into independent panel components (Weather, Radar, Controls, Side Info).
- **Domain Core Implemented:** Core logic built entirely independent of the game engine (`Aircraft`, `AircraftType`, `FlightPlan`, `Airport`, `Waypoint`, `WeatherData`).
- **Physics & Math:** Basic aircraft kinematics (heading-based movement) successfully verified against expected trigonometric outputs.

### ⏳ In Progress / Next Up:
- **Radar Rendering:** Drawing aircraft as live blips with data tags on a `Camera2D`-driven scope (pannable & zoomable).
- **Traffic Generation:** Randomized, procedural traffic scaled to airport size.
- **Dynamic Weather:** Realistic weather panel (Wind, QNH, Visibility).
- **Command Parser:** Text-based controller instructions (e.g., `LOT123 turn left heading 270`).
- **Time Controls:** Pause and simulation speed manipulation.

---

## 🗺️ Roadmap

| Phase  | Focus |
|:------:| :--- |
| **0**  | UI scaffolding — panel layout matching the planned interface. |
| **1**  | Domain core — aircraft, flight plans, airports, weather (engine-independent). |
| **2**  | Radar rendering — live blips, movement, pan/zoom. |
| **3**  | Traffic generation + dynamic weather panel. |
| **4**  | Text command parser for controller instructions. |
| **5**  | Simulation controls — pause, time scale, UI polish. |
| **6+** | Real SID/STAR procedures, role separation (Ground/Delivery/Tower/Approach), voice input/output. |

---

## 🛠️ Tech Stack

- **Engine:** Godot 4.x (Forward+ renderer)
- **Language:** C# (.NET / Mono) — *Zero GDScript used.*
- **Architecture:** Engine-independent domain core (`Core/`) strictly separated from presentation (`UI/`). Game logic is fully testable without running the engine.
- **Data:** JSON-based airport/waypoint definitions (no paid AIRAC dependency at this stage).

---

## 📂 Project Structure

```text
VectorATC/
├── Core/          # Pure C# domain logic — no Godot dependency
│   ├── Aircraft.cs
│   ├── AircraftType.cs
│   ├── Airport.cs
│   ├── FlightPlan.cs
│   ├── Waypoint.cs
│   └── WeatherData.cs
├── UI/            # Godot Control nodes — presentation layer only
│   ├── WeatherPanel.cs
│   ├── RadarPanel.cs
│   ├── ControlPanel.cs
│   └── SidePanel.cs
├── Main.cs        # Composition root — wires panels together
└── main.tscn 
```

## 👨‍💻 Author

**Daniil Zhdanov (d4nilx)** *Built in public. Solo project.*

<sub><sup>this project is being developed with long-term commercial intent.</sup></sub>
