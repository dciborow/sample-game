using UnityEngine;
using UnityEngine.SceneManagement;

namespace SampleGame.Core
{
    /// <summary>
    /// GameFlowController owns the single-player scene progression.
    /// 
    /// Flow: Home → Adventure → Boss → Home (loop)
    /// 
    /// Scenes do not decide what comes next. All transitions are triggered by events.
    /// This is the single source of truth for the game loop.
    /// </summary>
    public class GameFlowController : MonoBehaviour
    {
        // Scene name constants
        private const string HOME_SCENE = "HomeScene";
        private const string ADVENTURE_SCENE = "AdventureScene";
        private const string BOSS_SCENE = "BossScene";
        
        private static GameFlowController _instance;
        
        private void Awake()
        {
            // Singleton pattern to persist across scenes
            if (_instance != null && _instance != this)
            {
                Destroy(gameObject);
                return;
            }
            
            _instance = this;
            DontDestroyOnLoad(gameObject);
            
            // Subscribe to scene events
            SceneEvents.OnHomeComplete += HandleHomeComplete;
            SceneEvents.OnAdventureComplete += HandleAdventureComplete;
            SceneEvents.OnBossComplete += HandleBossComplete;
        }
        
        private void OnDestroy()
        {
            // Unsubscribe from events
            SceneEvents.OnHomeComplete -= HandleHomeComplete;
            SceneEvents.OnAdventureComplete -= HandleAdventureComplete;
            SceneEvents.OnBossComplete -= HandleBossComplete;
        }
        
        private void Start()
        {
            // Start at Home if not already there
            if (SceneManager.GetActiveScene().name != HOME_SCENE)
            {
                LoadScene(HOME_SCENE);
            }
        }
        
        /// <summary>
        /// Home → Adventure
        /// Triggered when player completes home scene (e.g., selects "Start Adventure")
        /// </summary>
        private void HandleHomeComplete()
        {
            Debug.Log("[GameFlow] Home Complete → Loading Adventure");
            LoadScene(ADVENTURE_SCENE);
        }
        
        /// <summary>
        /// Adventure → Boss
        /// Triggered when adventure scene completes (e.g., player reaches the end)
        /// </summary>
        private void HandleAdventureComplete()
        {
            Debug.Log("[GameFlow] Adventure Complete → Loading Boss");
            LoadScene(BOSS_SCENE);
        }
        
        /// <summary>
        /// Boss → Home (loop back)
        /// Triggered when boss fight completes (win or lose)
        /// </summary>
        private void HandleBossComplete()
        {
            Debug.Log("[GameFlow] Boss Complete → Loading Home");
            LoadScene(HOME_SCENE);
        }
        
        /// <summary>
        /// Internal method to load scenes
        /// </summary>
        private void LoadScene(string sceneName)
        {
            SceneManager.LoadScene(sceneName);
        }
    }
}
