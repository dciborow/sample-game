# Game Flow Documentation

## Overview
This document describes the complete single-player game loop implementation.

## Architecture

### GameFlowController
**Location:** `Assets/Scripts/Core/GameFlowController.cs`

The GameFlowController is the single source of truth for scene progression. It:
- Owns all scene transitions
- Implements the game loop: **Home → Adventure → Boss → Home** (repeat)
- Uses event-driven architecture (no polling)
- Persists across scene loads

### Scene Flow

```
┌─────────────────────────────────────────────┐
│                                             │
│  ┌──────┐      ┌───────────┐      ┌──────┐ │
└─>│ Home │─────>│ Adventure │─────>│ Boss │─┘
   └──────┘      └───────────┘      └──────┘
```

1. **Home Scene**: Entry point, player starts adventure
2. **Adventure Scene**: Main gameplay, player progresses through level
3. **Boss Scene**: Boss fight, completion loops back to Home

### Event System
**Location:** `Assets/Scripts/Core/SceneEvents.cs`

Scenes communicate completion through events, not by loading scenes directly:
- `OnHomeComplete`: Fired when home scene is ready to proceed
- `OnAdventureComplete`: Fired when adventure is complete
- `OnBossComplete`: Fired when boss fight ends

### Scene Controllers
**Location:** `Assets/Scripts/Scenes/`

Each scene has a controller that:
- Manages local scene logic
- Emits completion events
- **Does NOT decide what scene comes next**

Scene controllers:
- `HomeScene.cs`: Home scene controller
- `AdventureScene.cs`: Adventure scene controller
- `BossScene.cs`: Boss scene controller

## Tracing the Game Loop

To follow the complete run path, read `GameFlowController.cs`:

1. Start: GameFlowController loads Home scene
2. Home scene emits `OnHomeComplete` → GameFlowController loads Adventure
3. Adventure scene emits `OnAdventureComplete` → GameFlowController loads Boss
4. Boss scene emits `OnBossComplete` → GameFlowController loads Home (loop)

## Constraints Met

✅ **No UI**: Scene transitions use keyboard input (placeholder for actual game logic)  
✅ **No Save Data**: No persistence, pure game loop  
✅ **No Procedural Logic**: Static scene progression  
✅ **Traceable**: Entire flow visible in GameFlowController.cs  
✅ **Event-Driven**: All transitions triggered by events, not polling  

## Usage

The GameFlowController must be present in the initial scene. Once active, it:
1. Automatically starts at Home scene
2. Listens for scene completion events
3. Loads the next scene in the sequence
4. Loops back to Home after Boss

## Testing the Flow

1. Run the game from any scene
2. Press Space in each scene to trigger completion
3. Observe the scene transitions in console logs
4. Verify the loop: Home → Adventure → Boss → Home
