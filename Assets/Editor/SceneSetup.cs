using UnityEngine;
using UnityEditor;

/// <summary>
/// Helper script to set up the demo scene programmatically
/// Can be run from Unity Editor menu
/// </summary>
public class SceneSetup
{
    [MenuItem("Game/Setup Demo Scene")]
    public static void SetupDemoScene()
    {
        // Create Ground
        GameObject ground = GameObject.CreatePrimitive(PrimitiveType.Plane);
        ground.name = "Ground";
        ground.transform.position = Vector3.zero;
        ground.transform.localScale = new Vector3(5, 1, 5);
        ground.tag = "Ground";
        
        // Create Material for ground
        Material groundMat = new Material(Shader.Find("Universal Render Pipeline/Lit"));
        groundMat.color = new Color(0.3f, 0.5f, 0.3f);
        ground.GetComponent<MeshRenderer>().material = groundMat;
        
        // Create Player
        GameObject player = GameObject.CreatePrimitive(PrimitiveType.Capsule);
        player.name = "Player";
        player.tag = "Player";
        player.transform.position = new Vector3(0, 1, 0);
        
        // Remove default collider and add CharacterController
        // DestroyImmediate(player.GetComponent<CapsuleCollider>());
        CharacterController cc = player.AddComponent<CharacterController>();
        cc.center = Vector3.zero;
        cc.radius = 0.5f;
        cc.height = 2f;
        
        // Add player components
        player.AddComponent<PlayerHealth>();
        player.AddComponent<PlayerController>();
        AbilitySystem abilitySystem = player.AddComponent<AbilitySystem>();
        
        // Load and assign abilities
        var lightMelee = AssetDatabase.LoadAssetAtPath<AbilityData>("Assets/ScriptableObjects/Abilities/LightMelee.asset");
        var heavyMelee = AssetDatabase.LoadAssetAtPath<AbilityData>("Assets/ScriptableObjects/Abilities/HeavyMelee.asset");
        var groundSlam = AssetDatabase.LoadAssetAtPath<AbilityData>("Assets/ScriptableObjects/Abilities/GroundSlam.asset");
        
        if (lightMelee != null)
        {
            abilitySystem.abilities.Add(new AbilitySystem.AbilitySlot { ability = lightMelee });
        }
        if (heavyMelee != null)
        {
            abilitySystem.abilities.Add(new AbilitySystem.AbilitySlot { ability = heavyMelee });
        }
        if (groundSlam != null)
        {
            abilitySystem.abilities.Add(new AbilitySystem.AbilitySlot { ability = groundSlam });
        }
        
        // Create Material for player
        Material playerMat = new Material(Shader.Find("Universal Render Pipeline/Lit"));
        playerMat.color = Color.blue;
        player.GetComponent<MeshRenderer>().material = playerMat;
        
        // Create Enemy Manager
        GameObject managerObj = new GameObject("EnemyManager");
        EnemyManager enemyManager = managerObj.AddComponent<EnemyManager>();
        
        // Create Enemy from prefab (instead of raw GameObject)
        GameObject enemyPrefab = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefabs/Enemy.prefab");
        GameObject enemy = null;
        Enemy enemyComponent = null;
        
        if (enemyPrefab != null)
        {
            // Instantiate the prefab
            enemy = PrefabUtility.InstantiatePrefab(enemyPrefab) as GameObject;
            
            if (enemy != null)
            {
                enemy.transform.position = new Vector3(5, 1, 5);
                enemyComponent = enemy.GetComponent<Enemy>();
                Debug.Log("Enemy created from prefab");
            }
            else
            {
                Debug.LogError("Failed to instantiate Enemy prefab. The prefab may be corrupted.");
            }
        }
        else
        {
            Debug.LogWarning("Enemy prefab not found at Assets/Prefabs/Enemy.prefab. Creating enemy manually as fallback.");
            // Fallback: Create Enemy manually (old approach)
            enemy = GameObject.CreatePrimitive(PrimitiveType.Capsule);
            enemy.name = "Enemy";
            enemy.tag = "Enemy";
            enemy.transform.position = new Vector3(5, 1, 5);
            enemyComponent = enemy.AddComponent<Enemy>();
            
            // Create Material for enemy
            Material enemyMat = new Material(Shader.Find("Universal Render Pipeline/Lit"));
            enemyMat.color = Color.red;
            enemy.GetComponent<MeshRenderer>().material = enemyMat;
        }
        
        // Register enemy with manager
        if (enemyComponent != null)
        {
            enemyManager.RegisterEnemy(enemyComponent);
        }
        
        // Create Encounter Controller (will auto-discover enemies in Start)
        GameObject encounterObj = new GameObject("EncounterController");
        EncounterController encounterController = encounterObj.AddComponent<EncounterController>();
        
        // Create Camera
        GameObject cameraObj = new GameObject("Main Camera");
        cameraObj.tag = "MainCamera";
        Camera cam = cameraObj.AddComponent<Camera>();
        cameraObj.AddComponent<IsometricCamera>();
        
        // Add debug display
        DebugDisplay debugDisplay = cameraObj.AddComponent<DebugDisplay>();
        debugDisplay.playerController = player.GetComponent<PlayerController>();
        debugDisplay.abilitySystem = player.GetComponent<AbilitySystem>();
        debugDisplay.playerHealth = player.GetComponent<PlayerHealth>();
        debugDisplay.encounterController = encounterController;
        
        // Create Light
        GameObject lightObj = new GameObject("Directional Light");
        Light light = lightObj.AddComponent<Light>();
        light.type = LightType.Directional;
        lightObj.transform.rotation = Quaternion.Euler(50, -30, 0);
        
        // Create Arena Walls
        CreateWall("Wall_North", new Vector3(0, 1, 25), new Vector3(50, 2, 1));
        CreateWall("Wall_South", new Vector3(0, 1, -25), new Vector3(50, 2, 1));
        CreateWall("Wall_East", new Vector3(25, 1, 0), new Vector3(1, 2, 50));
        CreateWall("Wall_West", new Vector3(-25, 1, 0), new Vector3(1, 2, 50));
        
        Debug.Log("Demo scene setup complete!");
        
        // Mark scene as dirty
        UnityEditor.SceneManagement.EditorSceneManager.MarkSceneDirty(
            UnityEditor.SceneManagement.EditorSceneManager.GetActiveScene());
    }
    
    private static void CreateWall(string name, Vector3 position, Vector3 scale)
    {
        GameObject wall = GameObject.CreatePrimitive(PrimitiveType.Cube);
        wall.name = name;
        wall.transform.position = position;
        wall.transform.localScale = scale;
        
        Material wallMat = new Material(Shader.Find("Universal Render Pipeline/Lit"));
        wallMat.color = new Color(0.5f, 0.5f, 0.5f);
        wall.GetComponent<MeshRenderer>().material = wallMat;
    }
}
