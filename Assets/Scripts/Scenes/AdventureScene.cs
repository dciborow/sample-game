using UnityEngine;
using SampleGame.Core;

namespace SampleGame.Scenes
{
    /// <summary>
    /// Adventure scene controller.
    /// Does not decide what comes next - only emits events.
    /// </summary>
    public class AdventureScene : MonoBehaviour
    {
        private void Start()
        {
            Debug.Log("[AdventureScene] Adventure scene loaded");
        }
        
        private void Update()
        {
            // Simple trigger: Press Space to complete adventure
            // This is placeholder logic - in real game this would be reaching end of level
            if (Input.GetKeyDown(KeyCode.Space))
            {
                Debug.Log("[AdventureScene] Player completed adventure");
                CompleteAdventure();
            }
        }
        
        /// <summary>
        /// Called when adventure is complete
        /// Emits event instead of deciding next scene
        /// </summary>
        private void CompleteAdventure()
        {
            SceneEvents.TriggerAdventureComplete();
        }
    }
}
