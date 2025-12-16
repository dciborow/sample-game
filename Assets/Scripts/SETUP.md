# Setup Instructions

## Adding Scripts to Scenes

For the Game Flow Controller to work properly, you need to add the appropriate scripts to each scene:

### HomeScene Setup

1. Open `Assets/Scenes/HomeScene.unity` in Unity Editor
2. Create an empty GameObject (Right-click in Hierarchy → Create Empty)
3. Rename it to "GameFlowController"
4. Add the `GameFlowController` component (Add Component → Scripts → Sample Game → Core → GameFlowController)
5. Create another empty GameObject and rename it to "HomeSceneController"
6. Add the `HomeScene` component to it (Add Component → Scripts → Sample Game → Scenes → HomeScene)
7. Save the scene

### AdventureScene Setup

1. Open `Assets/Scenes/AdventureScene.unity` in Unity Editor
2. Create an empty GameObject and rename it to "AdventureSceneController"
3. Add the `AdventureScene` component to it (Add Component → Scripts → Sample Game → Scenes → AdventureScene)
4. Save the scene

### BossScene Setup

1. Open `Assets/Scenes/BossScene.unity` in Unity Editor
2. Create an empty GameObject and rename it to "BossSceneController"
3. Add the `BossScene` component to it (Add Component → Scripts → Sample Game → Scenes → BossScene)
4. Save the scene

## Testing the Flow

1. Open `HomeScene.unity`
2. Press Play in Unity Editor
3. Press Space to progress through scenes:
   - Space in Home → Goes to Adventure
   - Space in Adventure → Goes to Boss
   - Space in Boss → Returns to Home (loop)
4. Check the Console for debug logs showing scene transitions

## Important Notes

- The `GameFlowController` should **only** be added to the HomeScene (initial scene)
- It uses `DontDestroyOnLoad` to persist across all scenes
- Each scene needs its own scene controller script (HomeScene, AdventureScene, or BossScene)
- Scene controllers emit events; they do not load scenes directly
- All scene progression logic is in `GameFlowController.cs`
