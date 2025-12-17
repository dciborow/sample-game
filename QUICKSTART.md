# Quick Start Guide

## 🎮 Get Playing in 5 Minutes

### Prerequisites
- Unity 2021.3.6f1 (or compatible version)
- Git LFS installed

### Setup Steps

#### 1. Open Project
```bash
# Clone the repository (if not already done)
git clone <repository-url>
cd sample-game

# Open in Unity
# File > Open Project > Select the sample-game folder
```

#### 2. Setup the Demo Scene
In Unity Editor:
1. Open `Assets/Scenes/MainScene.unity`
2. From the menu bar: **Game > Setup Demo Scene**
3. Wait for scene setup to complete (you'll see a "Demo scene setup complete!" message)
4. Press **Play** ▶️

#### 3. Start Playing!
- **WASD**: Move around
- **Space**: Dodge roll
- **Left Mouse Button**: Quick light attack
- **Right Mouse Button**: Heavy attack (more damage, longer wind-up)
- **Q**: Ground Slam (aim with mouse, damages area)

## 🎯 What You'll See

### Player (Blue Capsule)
- Moves smoothly with WASD
- Rotates to face movement direction
- Can dodge to evade attacks
- Has 3 different abilities with cooldowns

### Enemy (Red Capsule)
- Detects player when nearby
- Chases and faces player
- Attacks when in melee range
- Has health that depletes when hit

### UI (Top-Left Corner)
- Health percentage
- Current state (Idle, WindUp, Active, Recovery)
- Cooldown status for dodge and all abilities
- Control reminders

### Arena
- Large green ground plane (50x50 units)
- Gray walls around the perimeter
- Isometric camera view at 45° angle

## 🔧 Customizing Abilities

### Modify Existing Abilities
1. Navigate to `Assets/ScriptableObjects/Abilities/`
2. Select an ability asset (e.g., `LightMelee.asset`)
3. In the Inspector, adjust values:
   - **Cooldown**: Time before ability can be used again
   - **Wind Up Time**: Telegraph/preparation duration
   - **Active Time**: Effect execution window
   - **Recovery Time**: Lockout after effect
   - **Damage**: How much damage it deals
   - **Range**: Maximum effective distance

### Create New Abilities
1. Right-click in Project view
2. Create > Abilities > Ability Data (or specific type)
3. Configure the new ability
4. Add to player's Ability System component (select Player GameObject, find "Abilities" list)

## 🐛 Troubleshooting

### Scene is Empty
- Make sure you ran **Game > Setup Demo Scene**
- If menu item not visible, check that `Assets/Editor/SceneSetup.cs` exists

### Player Not Moving
- Check that CharacterController component is attached to Player
- Verify Player tag is set correctly
- Check Input settings (Edit > Project Settings > Input)

### Abilities Not Working
- Ensure Ability System component has abilities assigned
- Check cooldown hasn't triggered (wait for cooldown to finish)
- Verify enemy has "Enemy" tag

### Camera Not Following
- Check that IsometricCamera component is on the camera
- Ensure Player has "Player" tag
- Camera will auto-find player on start

### No Damage to Enemy
- Verify enemy has "Enemy" tag
- Check ability range (enemy might be too far)
- For melee attacks, ensure enemy is in front of player

## 📚 Next Steps

### Learn the System
- Read `ARCHITECTURE.md` for detailed system overview
- Read `GAME_SETUP.md` for manual setup instructions
- Review the code in `Assets/Scripts/` folders

### Extend the Game
1. **Add Animations**: Integrate Unity's Animator Controller
2. **Add VFX**: Create particle effects for abilities
3. **Add Sound**: Integrate audio clips for actions
4. **More Enemies**: Duplicate and modify the enemy
5. **New Abilities**: Create custom ability types
6. **Better UI**: Replace debug display with polished UI

### Build and Share
1. File > Build Settings
2. Add `MainScene` to "Scenes in Build"
3. Select platform (PC, Mac, Linux)
4. Click "Build" and choose output folder
5. Share the executable!

## 💡 Tips

### Design Tips
- Start with existing abilities and modify them
- Keep wind-up times short for responsive feel
- Balance cooldowns vs damage (high damage = long cooldown)
- Test ability combinations for fun combos

### Performance Tips
- Keep enemy count reasonable (<20 for smooth gameplay)
- Use object pooling if spawning many objects
- Minimize Physics.OverlapSphere calls in Update loops

### Debugging Tips
- Use the on-screen debug display (always visible in play mode)
- Check Console for any error messages
- Use Debug.DrawLine() to visualize ranges and angles
- Add breakpoints in scripts for step-through debugging

## 🎨 Visual Customization

### Change Colors
Modify materials in SceneSetup.cs or create proper materials:
```csharp
// In SceneSetup.cs
Material playerMat = new Material(Shader.Find("Universal Render Pipeline/Lit"));
playerMat.color = Color.blue; // Change this!
```

### Replace Capsules with Models
1. Import 3D models into Assets/
2. Replace CreatePrimitive calls with model instantiation
3. Ensure colliders are set up properly

### Add Particle Effects
1. Create particle system prefab
2. Assign to ability's `areaIndicatorPrefab` field
3. Prefab will auto-spawn on ability execution

## 🆘 Getting Help

### Common Questions
**Q: How do I add a 4th ability?**
A: Create a new ability asset, add to AbilitySystem's abilities list, add new input in PlayerController

**Q: Can I change the camera angle?**
A: Yes! Select Camera, adjust IsometricCamera's "Offset" and "Rotation Angle"

**Q: How do I make enemies harder?**
A: Increase enemy health, damage, move speed, or reduce detection range

**Q: Can I have multiple players?**
A: Not currently - this is single player only, but could be extended

### Resources
- Unity Documentation: https://docs.unity3d.com/
- URP Manual: https://docs.unity3d.com/Packages/com.unity.render-pipelines.universal@latest
- This project's documentation in `ARCHITECTURE.md`

---

**Happy Game Making! 🎮✨**
