using UnityEngine;
using UnityEditor;
using System.Linq;

/// <summary>
/// Helper script to set up the Fight Test scene programmatically
/// Creates a minimal combat test environment with Player, Enemy, and Camera
/// No UI, no flow, no progression - just combat testing
/// </summary>
public class FightTestSceneSetup
{
    // Configuration constants
    private const float ENEMY_MAX_HEALTH = 50f;
    private const float ENEMY_DETECTION_RANGE = 20f;
    private const float ENEMY_ATTACK_RANGE = 2f;
    private const float ENEMY_MOVE_SPEED = 2f;
    
    [MenuItem("Game/Setup Fight Test Scene")]
    public static void SetupFightTestScene()
    {
        // Clear existing scene objects except Main Camera, Global Volume, and Directional Light
        GameObject[] allObjects = Object.FindObjectsOfType<GameObject>();
        foreach (GameObject obj in allObjects)
        {
            // Keep the main camera if it exists
            if (obj.GetComponent<Camera>() != null && obj.CompareTag("MainCamera"))
            {
                continue;
            }
            // Keep Global Volume for post-processing (check for Volume component)
            if (obj.GetComponent<UnityEngine.Rendering.Volume>() != null)
            {
                continue;
            }
            // Keep Directional Light to preserve its configuration (including UniversalAdditionalLightData)
            Light light = obj.GetComponent<Light>();
            if (light != null && light.type == LightType.Directional)
            {
                continue;
            }
            Object.DestroyImmediate(obj);
        }
        
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
        Object.DestroyImmediate(player.GetComponent<CapsuleCollider>());
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
        else
        {
            Debug.LogWarning("LightMelee ability asset not found at Assets/ScriptableObjects/Abilities/LightMelee.asset");
        }
        
        if (heavyMelee != null)
        {
            abilitySystem.abilities.Add(new AbilitySystem.AbilitySlot { ability = heavyMelee });
        }
        else
        {
            Debug.LogWarning("HeavyMelee ability asset not found at Assets/ScriptableObjects/Abilities/HeavyMelee.asset");
        }
        
        if (groundSlam != null)
        {
            abilitySystem.abilities.Add(new AbilitySystem.AbilitySlot { ability = groundSlam });
        }
        else
        {
            Debug.LogWarning("GroundSlam ability asset not found at Assets/ScriptableObjects/Abilities/GroundSlam.asset");
        }
        
        // Create Material for player
        Material playerMat = new Material(Shader.Find("Universal Render Pipeline/Lit"));
        playerMat.color = Color.blue;
        player.GetComponent<MeshRenderer>().material = playerMat;
        
        // Create Enemy
        GameObject enemy = GameObject.CreatePrimitive(PrimitiveType.Capsule);
        enemy.name = "Enemy";
        enemy.tag = "Enemy";
        enemy.transform.position = new Vector3(5, 1, 0);
        Enemy enemyComponent = enemy.AddComponent<Enemy>();
        
        // Configure enemy for test using constants
        enemyComponent.maxHealth = ENEMY_MAX_HEALTH;
        enemyComponent.detectionRange = ENEMY_DETECTION_RANGE;
        enemyComponent.attackRange = ENEMY_ATTACK_RANGE;
        enemyComponent.moveSpeed = ENEMY_MOVE_SPEED;
        
        // Create Material for enemy
        Material enemyMat = new Material(Shader.Find("Universal Render Pipeline/Lit"));
        enemyMat.color = Color.red;
        enemy.GetComponent<MeshRenderer>().material = enemyMat;
        
        // Setup Camera - find existing or create new
        Camera mainCamera = Camera.main;
        GameObject cameraObj;
        
        if (mainCamera != null)
        {
            cameraObj = mainCamera.gameObject;
        }
        else
        {
            cameraObj = new GameObject("Main Camera");
            cameraObj.tag = "MainCamera";
            cameraObj.AddComponent<Camera>();
            cameraObj.AddComponent<AudioListener>();
        }
        
        // Add or get IsometricCamera component
        IsometricCamera isoCam = cameraObj.GetComponent<IsometricCamera>();
        if (isoCam == null)
        {
            isoCam = cameraObj.AddComponent<IsometricCamera>();
        }
        isoCam.target = player.transform;
        
        // Ensure Directional Light exists (should be preserved from scene)
        Light[] lights = Object.FindObjectsOfType<Light>();
        bool hasDirectionalLight = lights.Any(l => l.type == LightType.Directional);
        
        if (!hasDirectionalLight)
        {
            GameObject lightObj = new GameObject("Directional Light");
            Light light = lightObj.AddComponent<Light>();
            light.type = LightType.Directional;
            light.intensity = 2f;
            lightObj.transform.rotation = Quaternion.Euler(50, -30, 0);
        }
        
        Debug.Log("Fight Test scene setup complete! Press Play to test Player → Enemy → Death");
        
        // Mark scene as dirty
        UnityEditor.SceneManagement.EditorSceneManager.MarkSceneDirty(
            UnityEditor.SceneManagement.EditorSceneManager.GetActiveScene());
    }
}
