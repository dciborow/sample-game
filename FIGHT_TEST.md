# Fight Test Scene

## Overview

The **Fight Test** scene is a minimal combat testing environment designed to validate the core combat loop: **Player → Enemy → Death**.

This scene contains:
- ✅ Player with movement and combat abilities
- ✅ One Enemy that can be attacked and killed
- ✅ Camera that follows the player
- ❌ No UI
- ❌ No game flow/progression
- ❌ No win/loss conditions

## Purpose

This scene is specifically designed to:
1. **Prove** the combat system works end-to-end
2. **Test** player attack mechanics
3. **Validate** enemy death behavior
4. **Verify** camera following functionality

## Setup Instructions

### Setup Steps

1. Open the project in Unity Editor
2. Open the scene: `Assets/Scenes/FightTest.unity`
3. From the menu bar, select: **Game > Setup Fight Test Scene**
   - This will add the Player, Enemy, and Ground to the scene
   - The scene starts with only Camera, Light, and Global Volume
4. Press **Play** ▶️

**Note**: The scene must be set up using the editor menu item before playing. The scene file itself contains only the rendering infrastructure by design, allowing easy reset and reconfiguration.

## Controls

- **WASD**: Move the player
- **Space**: Dodge roll
- **Left Mouse Button**: Light attack
- **Right Mouse Button**: Heavy attack
- **Q**: Ground Slam (aim with mouse)

## Testing Checklist

When testing this scene, verify:

- [x] Player can move using WASD
- [x] Player rotates to face movement direction
- [x] Camera follows player smoothly
- [x] Player can attack using mouse buttons
- [x] Enemy detects and moves toward player
- [x] Enemy takes damage from player attacks
- [x] Enemy dies and is removed from scene after enough damage
- [x] Player can dodge with Space

## Scene Contents

### Player
- **Position**: (0, 1, 0)
- **Components**: 
  - CharacterController
  - PlayerController
  - PlayerHealth
  - AbilitySystem (with LightMelee, HeavyMelee, GroundSlam)
- **Visual**: Blue capsule

### Enemy
- **Position**: (5, 1, 0)
- **Components**: 
  - Enemy (with 50 HP, 20 detection range, 2 move speed)
- **Visual**: Red capsule

### Camera
- **Type**: Main Camera with IsometricCamera component
- **Behavior**: Follows player with isometric view angle (45°)

### Environment
- **Ground**: 50x50 unit green plane
- **Lighting**: Directional light at 50° angle

## Definition of Done

✅ **You can start Play mode and kill the enemy.**

## Next Steps

After validating this scene works correctly:
1. Build on this foundation to add UI
2. Implement game flow logic
3. Add progression systems
4. Create more complex encounters

## Troubleshooting

### Enemy doesn't move toward player
- Check that Player has "Player" tag
- Verify Enemy detection range is large enough

### Player attacks don't damage enemy
- Ensure Enemy has "Enemy" tag
- Check ability range settings in ScriptableObjects

### Camera doesn't follow player
- Verify Player has "Player" tag
- Check IsometricCamera component is attached to Main Camera
