# 2.5D Isometric Action Combat Game - Setup Guide

## Overview
This is a minimal, composable, data-driven 2.5D isometric action combat foundation built in Unity URP.

## Features
- **Top-down movement** with isometric camera
- **Dodge ability** with cooldown system
- **Ability system** using ScriptableObjects with timing phases:
  - Wind-up phase
  - Active phase
  - Recovery phase
  - Cooldown tracking
- **Three abilities**:
  - Light Melee Attack (quick, low damage)
  - Heavy Melee Attack (slower, high damage)
  - Ground Slam (area-of-effect, ground-targeted)
- **Input buffering** for smooth ability chaining
- **Simple enemy AI** with detection and attack behavior
- **Demo arena scene**

## Setup Instructions

### Quick Setup (Using Scene Setup Script)
1. Open Unity Editor (version 2021.3.6f1 or compatible)
2. Open the MainScene (Assets/Scenes/MainScene.unity)
3. From the Unity menu bar, select: **Game > Setup Demo Scene**
4. This will automatically create all game objects, assign components, and configure the scene

### Manual Setup (Alternative)
If you prefer to set up manually:

#### 1. Create Ground
- Create a Plane (GameObject > 3D Object > Plane)
- Set scale to (5, 1, 5)
- Tag as "Ground"

#### 2. Create Player
- Create a Capsule (GameObject > 3D Object > Capsule)
- Name it "Player" and tag as "Player"
- Position at (0, 1, 0)
- Remove CapsuleCollider
- Add CharacterController component
- Add PlayerHealth component
- Add PlayerController component
- Add AbilitySystem component
  - Assign the three ability ScriptableObjects from Assets/ScriptableObjects/Abilities/

#### 3. Create Enemy
**Option A: Use the Prefab (Recommended)**
- Drag the Enemy prefab from Assets/Prefabs/Enemy.prefab into the scene
- Position it at desired location (e.g., 5, 1, 5)
- The prefab comes pre-configured with health and death functionality

**Option B: Manual Creation**
- Create a Capsule (GameObject > 3D Object > Capsule)
- Name it "Enemy" and tag as "Enemy"
- Position at (5, 1, 5)
- Add Enemy component
- Configure Enemy settings (see ENEMY_PREFAB_USAGE.md for details)

#### 4. Create Camera
- Create an empty GameObject
- Name it "Main Camera" and tag as "MainCamera"
- Add Camera component
- Add IsometricCamera component (will auto-find player)

#### 5. Create Lighting
- Create Directional Light (GameObject > Light > Directional Light)
- Rotate to desired angle (e.g., 50, -30, 0)

## Controls
- **WASD/Arrow Keys**: Move
- **Space**: Dodge
- **Left Mouse Button**: Light Melee Attack
- **Right Mouse Button**: Heavy Melee Attack
- **Q**: Ground Slam (aim with mouse, click to activate)

## Architecture

### ScriptableObject-Based Abilities
All abilities are data-driven using ScriptableObjects with the following properties:
- `cooldown`: Time before ability can be used again
- `windUpTime`: Preparation time before ability executes
- `activeTime`: Duration of ability effect
- `recoveryTime`: Time after effect before player can act
- `damage`: Base damage value
- `range`: Maximum effective range
- `targetType`: Self, Direction, or GroundTarget

### State Machine
The player controller uses a state machine to manage ability execution:
- **Idle**: Can move and use abilities
- **WindUp**: Preparing ability (can be interrupted in future versions)
- **Active**: Ability effect occurs
- **Recovery**: Cooldown before next action

### Input Buffering
The system buffers ability inputs during the recovery phase, allowing for smooth ability chaining without frame-perfect timing.

## Extending the System

### Adding New Abilities
1. Create a new ScriptableObject by right-clicking in Project view
2. Select Create > Abilities > Ability Data (or specific type)
3. Configure timing, damage, and range values
4. Add to player's AbilitySystem component

### Creating Custom Ability Types
1. Create a new script that inherits from `AbilityData`
2. Override `OnActivate()` for setup logic
3. Override `OnExecute()` for damage/effect logic
4. Add `[CreateAssetMenu]` attribute for easy asset creation

### Adding More Enemies
1. Create a new GameObject
2. Tag as "Enemy"
3. Add Enemy component
4. Configure health, damage, and AI parameters

## Technical Details
- **Unity Version**: 2021.3.6f1
- **Render Pipeline**: Universal Render Pipeline (URP)
- **Physics**: Unity Physics (3D)
- **Input**: Unity Legacy Input System (can be upgraded to new Input System)

## Performance Considerations
- All abilities use Physics.OverlapSphere for hit detection
- Simple distance checks for AI
- Minimal allocations during gameplay
- Data-driven design allows for easy tuning and balancing

## Future Enhancements
- Animation system integration
- Visual effects for abilities
- More enemy types
- Status effects system
- Combo system
- Save/load system
- UI improvements
