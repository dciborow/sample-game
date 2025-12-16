# System Architecture: 2.5D Isometric Action Combat

## Overview
Minimal 2.5D isometric action combat with one playable loop: move, dodge, attack.

## Core Loop
1. Player moves with WASD
2. Enemy detects and chases player
3. Player dodges with Space
4. Player attacks with Left Mouse Button
5. Enemy takes damage and dies
6. Loop continues

## System Components

### Ability System

**AbilityData (ScriptableObject)**
- Base class for abilities with timing phases
- `cooldown`, `windUpTime`, `activeTime`, `recoveryTime`
- `damage` and `range`

**MeleeAbilityData**
- Cone-based melee attack
- Damages enemies in front of player

**AbilitySystem (Component)**
- State machine: Idle → WindUp → Active → Recovery
- Tracks cooldowns
- Executes abilities

### Player System

**PlayerController**
- WASD movement
- Space to dodge
- Left Mouse Button to attack
- Input buffering (0.3s)

**PlayerHealth**
- Health tracking
- Damage and death handling

**IsometricCamera**
- Follows player at 45° angle

### Enemy System

**Enemy**
- Detects and chases player
- Attacks when in range
- Has health

### Editor Tools

**SceneSetup**
- Menu: Game > Setup Demo Scene
- Creates ground, player, enemy, camera, and walls

## File Structure
```
Assets/
├── Editor/
│   └── SceneSetup.cs
├── Scripts/
│   ├── Abilities/
│   │   ├── AbilityData.cs
│   │   ├── AbilitySystem.cs
│   │   └── MeleeAbilityData.cs
│   ├── Player/
│   │   ├── PlayerController.cs
│   │   ├── PlayerHealth.cs
│   │   └── IsometricCamera.cs
│   └── Enemy/
│       └── Enemy.cs
└── ScriptableObjects/
    └── Abilities/
        └── LightMelee.asset
```
