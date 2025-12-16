# 2.5D Isometric Action Combat Demo

Minimal playable demo: move, dodge, attack.

## Quick Start

1. Open project in Unity 2021.3.6f1 or compatible
2. Open `Assets/Scenes/MainScene.unity`
3. Menu: **Game > Setup Demo Scene**
4. Press Play

## Controls

- **WASD**: Move
- **Space**: Dodge
- **Left Mouse Button**: Attack

## What You'll See

- Blue capsule (Player) - moves and attacks
- Red capsule (Enemy) - chases and attacks
- Green ground with gray walls
- Isometric camera view

## Architecture

See [ARCHITECTURE.md](ARCHITECTURE.md) for system details.

## Extending

Create new abilities:
1. Right-click in Project
2. Create > Abilities > Ability Data
3. Configure values
4. Add to player's Ability System

See `MeleeAbilityData.cs` for reference implementation.
