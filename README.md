# Unity URP Sample Game
---
***A minimal 2.5D isometric action combat demo for Unity URP (Universal Render Pipeline).***

## What is this?

A simple, clean Unity game template that demonstrates:
- Player movement and combat abilities
- Enemy AI with health system
- Isometric camera
- Ability system with cooldowns

This is designed to be **easy to understand** and **easy to extend**.

## Getting Started

### Prerequisites
- Unity 2021.3.6f1 (or compatible version)
- Git LFS installed

### Quick Start

1. **Open the project in Unity**
   ```bash
   # Clone the repository
   git clone <repository-url>
   cd sample-game
   
   # Open in Unity Editor
   # File > Open Project > Select the sample-game folder
   ```

2. **Setup the demo scene**
   - Open `Assets/Scenes/FightTest.unity`
   - From the menu bar: **Game > Setup Fight Test Scene**
   - Press **Play** ▶️

3. **Play the game!**
   - **WASD**: Move around
   - **Space**: Dodge roll
   - **Left Mouse Button**: Light attack
   - **Right Mouse Button**: Heavy attack
   - **Q**: Ground Slam (aim with mouse)

For more details, see the **[Quick Start Guide](QUICKSTART.md)**.

## What You'll See

- **Player** (blue capsule): Moves, attacks, dodges
- **Enemy** (red capsule): Chases and attacks player
- **Combat**: Hit the enemy with abilities until it dies

## Customizing

All abilities are data-driven ScriptableObjects:
- Navigate to `Assets/ScriptableObjects/Abilities/`
- Select an ability (e.g., `LightMelee.asset`)
- Adjust cooldown, damage, range, etc. in the Inspector

## Project Structure

```
Assets/
├── Editor/
│   └── FightTestSceneSetup.cs  # Scene setup utility
├── Scripts/
│   ├── Abilities/              # Ability system
│   ├── Player/                 # Player controller, health, camera
│   ├── Enemy/                  # Enemy AI
│   └── EncounterController.cs  # Tracks enemy deaths
├── ScriptableObjects/
│   └── Abilities/              # Ability data assets
└── Scenes/
    └── FightTest.unity         # Demo scene
```

## Git LFS

This template uses **[Git LFS](https://git-lfs.github.com)** for large files. Make sure to install Git LFS before cloning.

## References

- Official Unity .gitignore: https://github.com/github/gitignore/blob/main/Unity.gitignore
- Unity Documentation: https://docs.unity3d.com/
- URP Manual: https://docs.unity3d.com/Packages/com.unity.render-pipelines.universal@latest

---

**Happy Game Making! 🎮✨**
