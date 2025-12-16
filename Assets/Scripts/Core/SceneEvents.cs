using System;

namespace SampleGame.Core
{
    /// <summary>
    /// Central event system for scene transitions.
    /// Scenes emit events instead of deciding what comes next.
    /// GameFlowController listens to these events and handles scene progression.
    /// </summary>
    public static class SceneEvents
    {
        /// <summary>
        /// Fired when the Home scene is complete and ready to proceed
        /// </summary>
        public static event Action OnHomeComplete;
        
        /// <summary>
        /// Fired when the Adventure scene is complete and ready to proceed
        /// </summary>
        public static event Action OnAdventureComplete;
        
        /// <summary>
        /// Fired when the Boss scene is complete and ready to proceed
        /// </summary>
        public static event Action OnBossComplete;
        
        /// <summary>
        /// Invoke Home completion event
        /// </summary>
        public static void TriggerHomeComplete()
        {
            OnHomeComplete?.Invoke();
        }
        
        /// <summary>
        /// Invoke Adventure completion event
        /// </summary>
        public static void TriggerAdventureComplete()
        {
            OnAdventureComplete?.Invoke();
        }
        
        /// <summary>
        /// Invoke Boss completion event
        /// </summary>
        public static void TriggerBossComplete()
        {
            OnBossComplete?.Invoke();
        }
    }
}
