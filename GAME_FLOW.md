# Game Flow Documentation

## Overview
The game flow is managed by a single `GameFlowController` component that orchestrates scene transitions in a clear, traceable manner.

## Scene Progression
The game follows a simple loop:

```
Home → Adventure → Boss → Home
```

## Architecture

### GameFlowController
**Location:** `Assets/Scripts/GameFlowController.cs`

This is the single source of truth for game progression. It:
- Owns all scene transitions
- Listens to game events (event-driven, not polling)
- Knows the complete flow: Home → Adventure → Boss → Home
- Scenes do NOT decide what comes next

### GameEvents
**Location:** `Assets/Scripts/GameFlowController.cs` (same file)

Static event system that scenes use to signal completion:
- `OnPlayerDeath` - Triggered when player dies
- `OnAdventureComplete` - Triggered when adventure is finished
- `OnBossDefeated` - Triggered when boss is defeated

## Event Flow

### Player Death
```
PlayerHealth.Die() 
  → GameEvents.TriggerPlayerDeath() 
  → GameFlowController.HandlePlayerDeath() 
  → TransitionToHome()
```

### Adventure Complete
```
AdventureCompleteTrigger.TriggerAdventureComplete() 
  → GameEvents.TriggerAdventureComplete() 
  → GameFlowController.HandleAdventureComplete() 
  → TransitionToBoss()
```

### Boss Defeated
```
Enemy.Die() [if isBoss=true]
  → GameEvents.TriggerBossDefeated() 
  → GameFlowController.HandleBossDefeated() 
  → TransitionToHome()
```

## Setup Instructions

### 1. Add GameFlowController to Scene
In your starting scene (e.g., HomeScene):
1. Create an empty GameObject named "GameFlowController"
2. Add the `GameFlowController` component
3. The controller persists across scenes (DontDestroyOnLoad)

### 2. Scene Configuration

#### HomeScene
- Entry point of the game
- Can have a way to start the game (calls `GameFlowController.StartGame()`)

#### AdventureScene
- Regular enemies
- Place an `AdventureCompleteTrigger` component in the scene
- Configure the trigger (e.g., trigger zone, timer, or enemy count)

#### BossScene
- Place an enemy with `isBoss = true`
- When the boss dies, it triggers the boss defeated event

### 3. Build Settings
Add all three scenes to Build Settings in this order:
1. HomeScene
2. AdventureScene  
3. BossScene

## Components

### GameFlowController
- **Purpose:** Orchestrates scene flow
- **Persistence:** Lives across all scenes
- **Events:** Subscribes to all game events
- **Methods:**
  - `StartGame()` - Begin the adventure
  - `ReturnToHome()` - Return to home screen

### AdventureCompleteTrigger
- **Purpose:** Signal when adventure is complete
- **Usage:** Place in AdventureScene
- **Options:**
  - Auto-trigger after delay
  - Trigger on player collision
  - Call `TriggerAdventureComplete()` manually

### Enemy (Boss Mode)
- **Purpose:** Regular enemy with optional boss flag
- **Boss Flag:** Set `isBoss = true` for boss enemies
- **Behavior:** Triggers boss defeated event on death

## Tracing the Flow

You can trace the entire game loop in a single file (`GameFlowController.cs`):

1. **Game Start:** `StartGame()` → `TransitionToAdventure()`
2. **Adventure → Boss:** Event handler → `HandleAdventureComplete()` → `TransitionToBoss()`
3. **Boss → Home:** Event handler → `HandleBossDefeated()` → `TransitionToHome()`
4. **Death → Home:** Event handler → `HandlePlayerDeath()` → `TransitionToHome()`

## Design Principles

### ✓ Event-Driven
- All transitions triggered by events, not polling
- No Update() loops checking conditions
- Clear cause-and-effect relationships

### ✓ Single Responsibility
- GameFlowController ONLY manages scene flow
- Scenes trigger events, don't decide next scene
- Clear separation of concerns

### ✓ Traceable
- Complete loop visible in one file
- No hidden scene transitions
- Debug logging for all transitions

### ✓ Simple
- No UI dependencies
- No save data
- No procedural logic
- Just scene progression

## Testing the Flow

### Manual Testing
1. **Player Death Flow:**
   - Enter AdventureScene
   - Let enemies kill the player
   - Verify return to HomeScene

2. **Adventure Complete Flow:**
   - Enter AdventureScene
   - Trigger the adventure complete event
   - Verify transition to BossScene

3. **Boss Victory Flow:**
   - Enter BossScene
   - Defeat the boss enemy
   - Verify return to HomeScene

### Debug Logging
All transitions are logged with `[GameFlow]` prefix:
- `[GameFlow] Player died - returning to Home`
- `[GameFlow] Adventure complete - transitioning to Boss`
- `[GameFlow] Boss defeated - returning to Home`
- `[GameFlow] Loading scene: <SceneName>`

## Extension Points

### Adding New Events
```csharp
// In GameEvents
public static event GameEventHandler OnNewEvent;
public static void TriggerNewEvent() { OnNewEvent?.Invoke(); }

// In GameFlowController.Awake()
GameEvents.OnNewEvent += HandleNewEvent;

// Add handler
private void HandleNewEvent() { /* transition logic */ }
```

### Adding New Scenes
1. Update scene constants in GameFlowController
2. Add transition methods
3. Wire up event handlers
4. Update this documentation

## Constraints Met

✓ **Loop is traceable in one file** - All flow logic in GameFlowController.cs  
✓ **No UI** - Pure scene management, no UI dependencies  
✓ **No save data** - State lives only in memory  
✓ **No procedural logic** - Fixed progression path  
✓ **Event-driven transitions** - No polling  
✓ **Scenes don't decide next** - GameFlowController owns all decisions  
✓ **Can follow run path without opening combat code** - Flow is independent of combat implementation
