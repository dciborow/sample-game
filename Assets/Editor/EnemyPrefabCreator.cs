using UnityEngine;
using UnityEditor;

/// <summary>
/// Editor script to create a minimal Enemy prefab
/// Run from Unity Editor menu: Game/Create Enemy Prefab
/// </summary>
public class EnemyPrefabCreator
{
    [MenuItem("Game/Create Enemy Prefab")]
    public static void CreateEnemyPrefab()
    {
        // Create the prefab directory if it doesn't exist
        string prefabPath = "Assets/Prefabs";
        if (!AssetDatabase.IsValidFolder(prefabPath))
        {
            AssetDatabase.CreateFolder("Assets", "Prefabs");
        }
        
        // Create a temporary GameObject for the enemy
        GameObject enemyObject = GameObject.CreatePrimitive(PrimitiveType.Capsule);
        enemyObject.name = "Enemy";
        
        // Set the tag (ensure "Enemy" tag exists in project)
        try
        {
            enemyObject.tag = "Enemy";
        }
        catch (UnityException)
        {
            Debug.LogWarning("'Enemy' tag does not exist. Please add it in Project Settings > Tags and Layers, then run this script again.");
        }
        
        // The capsule already has a CapsuleCollider, which satisfies the requirement
        CapsuleCollider collider = enemyObject.GetComponent<CapsuleCollider>();
        if (collider != null)
        {
            // Configure collider for proper interaction
            collider.isTrigger = false; // Physical collider for abilities to detect
        }
        
        // Add the Enemy component with minimal configuration
        Enemy enemyComponent = enemyObject.AddComponent<Enemy>();
        
        // Configure for minimal behavior (no AI movement or attacks as per issue constraints)
        enemyComponent.maxHealth = 50f;
        enemyComponent.detectionRange = 0f; // Disable detection
        enemyComponent.attackRange = 0f; // Disable attacking
        enemyComponent.moveSpeed = 0f; // Disable movement
        enemyComponent.attackCooldown = 999f; // Effectively disable attacks
        enemyComponent.attackDamage = 0f; // No damage
        enemyComponent.isBoss = false;
        
        // Create a simple material for the enemy
        Shader shader = Shader.Find("Universal Render Pipeline/Lit");
        if (shader == null)
        {
            // Fallback to Standard shader if URP is not available
            shader = Shader.Find("Standard");
            Debug.LogWarning("URP Lit shader not found. Using Standard shader as fallback.");
        }
        
        if (shader != null)
        {
            Material enemyMat = new Material(shader);
            enemyMat.color = Color.red;
            enemyObject.GetComponent<MeshRenderer>().material = enemyMat;
        }
        else
        {
            Debug.LogWarning("Could not find suitable shader. Using default material.");
        }
        
        // Save as prefab
        string prefabFilePath = prefabPath + "/Enemy.prefab";
        bool prefabSuccess = false;
        PrefabUtility.SaveAsPrefabAsset(enemyObject, prefabFilePath, out prefabSuccess);
        
        // Clean up the temporary GameObject
        Object.DestroyImmediate(enemyObject);
        
        if (prefabSuccess)
        {
            Debug.Log($"Enemy prefab created successfully at: {prefabFilePath}");
            Debug.Log("Prefab configuration:");
            Debug.Log("- GameObject: Capsule primitive");
            Debug.Log("- Collider: CapsuleCollider (non-trigger)");
            Debug.Log("- Component: Enemy with health system");
            Debug.Log("- Health: 50 HP");
            Debug.Log("- AI: Disabled (minimal configuration)");
            Debug.Log("- Material: Red (URP/Lit shader)");
            
            // Select the prefab in the Project window
            Object prefab = AssetDatabase.LoadAssetAtPath<Object>(prefabFilePath);
            Selection.activeObject = prefab;
            EditorGUIUtility.PingObject(prefab);
        }
        else
        {
            Debug.LogError("Failed to create Enemy prefab");
        }
        
        // Refresh the Asset Database
        AssetDatabase.Refresh();
    }
}
