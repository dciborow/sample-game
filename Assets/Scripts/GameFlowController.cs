using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Controls the single-player game loop and scene progression.
/// Manages transitions: Home → Adventure → Boss → Home
/// Scene transitions are event-driven, not polling.
/// </summary>
public class GameFlowController : MonoBehaviour
{
    // Singleton instance
    private static GameFlowController _instance;
    
    public static GameFlowController Instance
    {
        get { return _instance; }
        private set { _instance = value; }
    }
    
    // Scene names
    private const string HOME_SCENE = "HomeScene";
    private const string ADVENTURE_SCENE = "AdventureScene";
    private const string BOSS_SCENE = "BossScene";
    
    // Current game state
    private GameState currentState = GameState.Home;
    
    public enum GameState
    {
        Home,
        Adventure,
        Boss
    }
    
    void Awake()
    {
        // Singleton pattern - only one GameFlowController should exist
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        
        Instance = this;
        DontDestroyOnLoad(gameObject);
        
        // Subscribe to game events
        GameEvents.OnPlayerDeath += HandlePlayerDeath;
        GameEvents.OnAdventureComplete += HandleAdventureComplete;
        GameEvents.OnBossDefeated += HandleBossDefeated;
    }
    
    void OnDestroy()
    {
        // Unsubscribe from events
        if (Instance == this)
        {
            GameEvents.OnPlayerDeath -= HandlePlayerDeath;
            GameEvents.OnAdventureComplete -= HandleAdventureComplete;
            GameEvents.OnBossDefeated -= HandleBossDefeated;
            Instance = null;
        }
    }
    
    /// <summary>
    /// Start the game loop from Home
    /// </summary>
    public static void StartGame()
    {
        if (Instance != null)
        {
            Instance.TransitionToAdventure();
        }
    }
    
    /// <summary>
    /// Return to home screen
    /// </summary>
    public static void ReturnToHome()
    {
        if (Instance != null)
        {
            Instance.TransitionToHome();
        }
    }
    
    /// <summary>
    /// Get current game state for debugging
    /// </summary>
    public static GameState GetCurrentState()
    {
        return Instance != null ? Instance.currentState : GameState.Home;
    }
    
    // Scene transition handlers
    
    private void HandlePlayerDeath()
    {
        Debug.Log("[GameFlow] Player died - returning to Home");
        TransitionToHome();
    }
    
    private void HandleAdventureComplete()
    {
        Debug.Log("[GameFlow] Adventure complete - transitioning to Boss");
        TransitionToBoss();
    }
    
    private void HandleBossDefeated()
    {
        Debug.Log("[GameFlow] Boss defeated - returning to Home");
        TransitionToHome();
    }
    
    // Scene transition methods
    
    private void TransitionToHome()
    {
        Debug.Log($"[GameFlow] State transition: {currentState} → {GameState.Home}");
        currentState = GameState.Home;
        LoadScene(HOME_SCENE);
    }
    
    private void TransitionToAdventure()
    {
        Debug.Log($"[GameFlow] State transition: {currentState} → {GameState.Adventure}");
        currentState = GameState.Adventure;
        LoadScene(ADVENTURE_SCENE);
    }
    
    private void TransitionToBoss()
    {
        Debug.Log($"[GameFlow] State transition: {currentState} → {GameState.Boss}");
        currentState = GameState.Boss;
        LoadScene(BOSS_SCENE);
    }
    
    private void LoadScene(string sceneName)
    {
        Debug.Log($"[GameFlow] Loading scene: {sceneName}");
        SceneManager.LoadScene(sceneName);
    }
}

/// <summary>
/// Static event system for game flow triggers.
/// Scenes trigger these events, GameFlowController listens and decides what's next.
/// </summary>
public static class GameEvents
{
    // Event delegates
    public delegate void GameEventHandler();
    
    // Game flow events
    public static event GameEventHandler OnPlayerDeath;
    public static event GameEventHandler OnAdventureComplete;
    public static event GameEventHandler OnBossDefeated;
    
    // Event trigger methods
    public static void TriggerPlayerDeath()
    {
        Debug.Log("[GameEvents] Player death triggered");
        OnPlayerDeath?.Invoke();
    }
    
    public static void TriggerAdventureComplete()
    {
        Debug.Log("[GameEvents] Adventure complete triggered");
        OnAdventureComplete?.Invoke();
    }
    
    public static void TriggerBossDefeated()
    {
        Debug.Log("[GameEvents] Boss defeated triggered");
        OnBossDefeated?.Invoke();
    }
}
