# GameFlowController Usage Guide

## Quick Start

### Step 1: Setup in Unity Editor

1. **Open HomeScene**
   - Open `Assets/Scenes/HomeScene.unity`

2. **Create GameFlowController GameObject**
   - In Hierarchy: Right-click → Create Empty
   - Name it "GameFlowController"
   - Add Component → `GameFlowController` script
   - This object will persist across all scenes

3. **Configure AdventureScene**
   - Open `Assets/Scenes/AdventureScene.unity`
   - Add enemies (regular enemies, not bosses)
   - Create a trigger zone:
     - GameObject → 3D Object → Cube
     - Name it "AdventureCompleteTrigger"
     - Add Component → `AdventureCompleteTrigger` script
     - Set "Is Trigger" on Box Collider
     - Position near exit/goal
   - OR use auto-trigger:
     - Add `AdventureCompleteTrigger` to any GameObject
     - Enable "Auto Trigger On Start"
     - Set delay (e.g., 30 seconds)

4. **Configure BossScene**
   - Open `Assets/Scenes/BossScene.unity`
   - Add an enemy
   - Select the enemy GameObject
   - In Inspector: Check ☑ "Is Boss"
   - When this enemy dies, boss victory triggers

5. **Build Settings**
   - File → Build Settings
   - Add scenes in order:
     1. HomeScene
     2. AdventureScene
     3. BossScene

### Step 2: Starting the Game

There are two ways to start the game flow:

#### Option A: Direct Start (for testing)
In HomeScene, create a simple starter script:

```csharp
using UnityEngine;

public class GameStarter : MonoBehaviour
{
    void Start()
    {
        // Auto-start after 2 seconds for testing
        Invoke(nameof(StartGame), 2f);
    }
    
    void StartGame()
    {
        GameFlowController.StartGame();
    }
}
```

#### Option B: Button Press (for production)
```csharp
using UnityEngine;

public class GameStarter : MonoBehaviour
{
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            GameFlowController.StartGame();
        }
    }
}
```

## Complete Flow Example

### Scenario 1: Player Completes Everything
```
1. HomeScene loads
2. Player presses Enter → GameFlowController.StartGame()
3. → AdventureScene loads
4. Player reaches trigger zone
5. → AdventureCompleteTrigger fires event
6. → GameFlowController receives event
7. → BossScene loads
8. Player defeats boss enemy
9. → Boss dies, triggers event
10. → GameFlowController receives event
11. → HomeScene loads (loop complete!)
```

### Scenario 2: Player Dies
```
1. AdventureScene or BossScene
2. Enemy kills player
3. → PlayerHealth.Die() triggers event
4. → GameFlowController receives event
5. → HomeScene loads immediately
```

## Testing Checklist

### Test 1: Player Death in Adventure
- [ ] Start game from HomeScene
- [ ] Let enemy kill player in AdventureScene
- [ ] Verify return to HomeScene
- [ ] Check console for "[GameFlow] Player died - returning to Home"

### Test 2: Adventure Complete
- [ ] Start game from HomeScene
- [ ] Enter AdventureScene
- [ ] Trigger adventure completion (zone or timer)
- [ ] Verify transition to BossScene
- [ ] Check console for "[GameFlow] Adventure complete - transitioning to Boss"

### Test 3: Boss Victory
- [ ] Load BossScene directly (for testing)
- [ ] Ensure enemy has isBoss=true
- [ ] Defeat the boss
- [ ] Verify return to HomeScene
- [ ] Check console for "[GameFlow] Boss defeated - returning to Home"

### Test 4: Full Loop
- [ ] Start from HomeScene
- [ ] Complete adventure
- [ ] Defeat boss
- [ ] Return to HomeScene
- [ ] Repeat loop to verify it works multiple times

## Debug Commands

### Console Logs to Watch For
```
[GameEvents] Player death triggered
[GameFlow] Player died - returning to Home
[GameFlow] Loading scene: HomeScene

[GameEvents] Adventure complete triggered
[GameFlow] Adventure complete - transitioning to Boss
[GameFlow] Loading scene: BossScene

[GameEvents] Boss defeated triggered
[GameFlow] Boss defeated - returning to Home
[GameFlow] Loading scene: HomeScene
```

### Manual Testing Commands
Add these to a test script for quick testing:

```csharp
using UnityEngine;

public class GameFlowDebug : MonoBehaviour
{
    void Update()
    {
        // Press 1 to start game
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            Debug.Log("Manual: Starting game");
            GameFlowController.StartGame();
        }
        
        // Press 2 to trigger adventure complete
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            Debug.Log("Manual: Adventure complete");
            GameEvents.TriggerAdventureComplete();
        }
        
        // Press 3 to trigger boss defeated
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            Debug.Log("Manual: Boss defeated");
            GameEvents.TriggerBossDefeated();
        }
        
        // Press 4 to trigger player death
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            Debug.Log("Manual: Player death");
            GameEvents.TriggerPlayerDeath();
        }
        
        // Press H to go home
        if (Input.GetKeyDown(KeyCode.H))
        {
            Debug.Log("Manual: Return to home");
            GameFlowController.ReturnToHome();
        }
    }
}
```

## Common Issues

### Issue: "Scene 'HomeScene' couldn't be loaded"
**Solution:** Add all scenes to Build Settings (File → Build Settings → Add Open Scenes)

### Issue: Events not triggering
**Solution:** Ensure GameFlowController exists in the first scene and persists (DontDestroyOnLoad)

### Issue: Boss doesn't trigger victory
**Solution:** Make sure the Enemy component has "Is Boss" checkbox enabled

### Issue: Multiple GameFlowControllers
**Solution:** The singleton pattern prevents this, but if you see warnings, ensure only one exists in your starting scene

## Advanced Usage

### Custom Triggers
Create your own trigger conditions:

```csharp
using UnityEngine;

public class CustomAdventureTrigger : MonoBehaviour
{
    private int enemiesKilled = 0;
    public int requiredKills = 10;
    
    public void OnEnemyKilled()
    {
        enemiesKilled++;
        if (enemiesKilled >= requiredKills)
        {
            GameEvents.TriggerAdventureComplete();
        }
    }
}
```

### Multiple Boss Phases
```csharp
using UnityEngine;

public class BossController : MonoBehaviour
{
    private Enemy enemy;
    public bool isPhaseTwo = false;
    
    void Start()
    {
        enemy = GetComponent<Enemy>();
    }
    
    void Update()
    {
        // When boss health drops to 50%, enter phase 2
        if (!isPhaseTwo && enemy.GetHealthPercent() < 0.5f)
        {
            isPhaseTwo = true;
            // Increase boss stats, change behavior, etc.
        }
    }
}
```

### Tracking Progress
```csharp
public class ProgressTracker : MonoBehaviour
{
    private static int deathCount = 0;
    private static int bossKills = 0;
    
    void Awake()
    {
        GameEvents.OnPlayerDeath += OnPlayerDied;
        GameEvents.OnBossDefeated += OnBossKilled;
    }
    
    void OnPlayerDied()
    {
        deathCount++;
        Debug.Log($"Player has died {deathCount} times");
    }
    
    void OnBossKilled()
    {
        bossKills++;
        Debug.Log($"Boss defeated {bossKills} times");
    }
}
```

## Architecture Benefits

### Easy to Trace
Every scene transition goes through GameFlowController:
1. Open `GameFlowController.cs`
2. Read the event handlers
3. See exactly what triggers what
4. No need to search through multiple files

### Easy to Extend
Want to add a new scene type?
1. Add scene constant to GameFlowController
2. Create transition method
3. Add event handler
4. Wire up the trigger

### Easy to Test
All game flow logic in one place:
- Unit test the event handlers
- Mock the SceneManager
- Verify correct transitions

### Easy to Debug
All transitions logged with [GameFlow] prefix:
- Search console for "[GameFlow]"
- See complete transition history
- Identify where flow breaks
