# 🎯 FPS Framework – Unity Minor Skilled Semester Project

This repository contains a modular **First-Person Shooter Framework** built in Unity as part of the *Minor Skilled* semester at **Saxion University of Applied Sciences**. The project includes shooting mechanics, a modular weapon customization system, AI enemies, and tooling for mission design.

---

## 🚀 Project Highlights

### 🔫 Weapon System
- ✅ Fully modular weapon architecture using **ScriptableObjects**.
- ✅ Socket-based attachment system: stocks, scopes, muzzles, handguards, triggers, magazines.
- ✅ Runtime part switching with part-type filtering via UI.
- ✅ Save/load system for loadout persistence using JSON.
- ✅ Smooth procedural animation: bobbing, directional tilting, movement lerping.

### 🧠 AI System
- ✅ State machine with **Idle**, **Patrol**, **Chase**, **Attack**, and **Die** states.
- ✅ Vision-based detection with pursuit behavior.
- ✅ Enemy groups are easily referenceable for task objectives.
- ✅ Modular setup for easy expansion.

### 🧭 Mission System
- ✅ **Waypoint-driven** progression system.
- ✅ Waypoints can assign one or multiple **Tasks**:
  - Reach destination
  - Kill X enemies
  - Destroy a target
- ✅ Events are queued and evaluated globally to track mission state.
- ✅ Each waypoint triggers the next upon successful task completion.

---

## 🛠️ Custom Tooling

### 🧩 Weapon Builder UI
-  Buttons dynamically display parts for each type.
-  In-editor and in-game preview support.
-  Weapon loadouts can be saved and loaded.

### 🗺️ Mission & Waypoint Editor
-  Waypoints are assigned unique names.
-  Tasks are added per waypoint using a **custom EditorWindow**.
-  Enemies/targets are referenced and linked via inspector.

## 🧠 Technical Notes

-  All systems are **event-driven** to avoid tight coupling.
-  Each weapon part is a prefab enabled/disabled via parent tag logic.
-  Editor tooling improves productivity for mission designers.
-  Codebase follows SOLID principles for scalability and reusability.
-  Uses **static classes** for lightweight global data persistence across scenes.

---

