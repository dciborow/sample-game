# Encounter System Guide

## Overview
The EncounterController provides a simple, decoupled way to track when all enemies in an encounter have been defeated. This allows game flow to respond to encounter completion without needing to know combat implementation details.

## Components

### EncounterController
- **Location**: `Assets/Scripts/EncounterController.cs`
- **Purpose**: Tracks enemies and emits completion events
- **Key Features**:
  - Enemy registration and tracking
  - Automatic detection of encounter completion
  - UnityEvent for flexible response handling

### Integration with Enemy System
The Enemy component automatically notifies the EncounterController when it dies, ensuring proper tracking without manual intervention.

## Usage

### Basic Setup

1. **Create an EncounterController in your scene:**
```csharp
GameObject encounterObj = new GameObject("EncounterController");
EncounterController controller = encounterObj.AddComponent<EncounterController>();
```

2. **The EncounterController automatically discovers all enemies in the scene during Start().**
   - No manual registration needed for standard scenes
   - Enemies are automatically tracked when they spawn

3. **Set up event listeners (optional):**
In the Unity Inspector:
- Select the EncounterController GameObject
- Add listeners to the "OnEncounterComplete" event
- Configure what should happen when the encounter completes

Or in code:
```csharp
controller.OnEncounterComplete.AddListener(OnEncounterFinished);

void OnEncounterFinished()
{
    Debug.Log("All enemies defeated!");
    // Load next level, show victory screen, etc.
}
```

### Query API

```csharp
// Check how many enemies remain
int remaining = controller.GetRemainingEnemyCount();

// Check if encounter is complete
bool isComplete = controller.IsEncounterComplete();
```

## Design Philosophy

### Decoupling
The EncounterController doesn't know:
- How enemies take damage
- What abilities players have
- Combat mechanics details

This separation allows:
- Easy testing of encounter logic
- Reusable across different game modes
- Changes to combat without affecting encounter tracking

### Events Over Polling
Using UnityEvents instead of polling (checking every frame) provides:
- Better performance
- Clearer code structure
- Flexible response mechanisms
- Inspector-configurable behavior

## Example Scenarios

### Adventure Scene
```csharp
// Setup - enemies are auto-discovered
EncounterController encounter = CreateEncounter();

encounter.OnEncounterComplete.AddListener(() => {
    ShowVictoryScreen();
    UnlockNextArea();
});
```

### Boss Scene
```csharp
// Setup - boss is auto-discovered
EncounterController bossEncounter = CreateEncounter();

bossEncounter.OnEncounterComplete.AddListener(() => {
    PlayVictoryAnimation();
    AwardLoot();
    ReturnToHub();
});
```

### Manual Registration (Advanced)
For dynamic enemy spawning after the encounter starts:
```csharp
// Spawn enemy after encounter has started
Enemy newEnemy = SpawnEnemy();
encounterController.RegisterEnemy(newEnemy);
newEnemy.SetEncounterController(encounterController);
```

### Multiple Waves (Future Enhancement)
While the current implementation doesn't support waves, the foundation is in place:
```csharp
// Pseudocode for future wave system
void StartWave(int waveNumber)
{
    EncounterController wave = CreateEncounter();
    foreach (Enemy enemy in GetEnemiesForWave(waveNumber))
    {
        wave.RegisterEnemy(enemy);
    }
    
    wave.OnEncounterComplete.AddListener(() => {
        if (HasMoreWaves())
            StartWave(waveNumber + 1);
        else
            CompleteLevel();
    });
}
```

## Debug Information

The DebugDisplay component shows:
- Number of enemies remaining
- Encounter completion status

This provides real-time feedback during development and testing.

## Constraints (By Design)

The system intentionally does NOT include:
- ❌ Wave management
- ❌ Timers or time limits
- ❌ Scoring or points
- ❌ Enemy spawn management
- ❌ Difficulty scaling

These features can be added as separate systems that use the EncounterController as a foundation.

## Testing the System

### Manual Test Steps

1. **Run the demo scene** (use Game/Setup Demo Scene menu)
2. **Observe the Debug Display** showing "Enemies Remaining: 1"
3. **Defeat the enemy** using combat abilities
4. **Watch for "Encounter Complete!" message** in console
5. **Verify Debug Display** shows "Status: COMPLETE"

### Expected Behavior

✅ Enemy registration happens at scene start  
✅ Enemy count decrements when enemies die  
✅ Event fires when last enemy is defeated  
✅ Encounter cannot be completed twice  
✅ Debug display reflects current state  

## Extension Points

The system is designed for easy extension:

### Custom Completion Conditions
```csharp
// Override completion check
public class TimedEncounterController : EncounterController
{
    public float timeLimit = 60f;
    private float startTime;
    
    void Start()
    {
        base.Start();
        startTime = Time.time;
    }
    
    void Update()
    {
        if (Time.time - startTime >= timeLimit)
        {
            // Time's up - fail encounter
            OnEncounterFailed?.Invoke();
        }
    }
}
```

### Additional Events
```csharp
// Extend with more events
public UnityEvent<int> OnEnemyCountChanged;
public UnityEvent OnFirstEnemyDefeated;
public UnityEvent OnHalfwayComplete;
```

## Summary

The EncounterController provides a minimal, focused solution for tracking encounter completion. Its event-driven architecture allows game flow to respond to completion without coupling to combat details, supporting both simple adventure scenes and complex boss encounters with the same mechanism.
