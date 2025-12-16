using UnityEngine;
using SampleGame.Core;

namespace SampleGame.Scenes
{
    /// <summary>
    /// Home scene controller.
    /// Does not decide what comes next - only emits events.
    /// </summary>
    public class HomeScene : MonoBehaviour
    {
        private void Start()
        {
            Debug.Log("[HomeScene] Home scene loaded");
        }
        
        private void Update()
        {
            // Simple trigger: Press Space to start adventure
            // This is placeholder logic - in real game this would be UI button click
            if (Input.GetKeyDown(KeyCode.Space))
            {
                Debug.Log("[HomeScene] Player initiated adventure start");
                CompleteHome();
            }
        }
        
        /// <summary>
        /// Called when home scene is complete
        /// Emits event instead of deciding next scene
        /// </summary>
        private void CompleteHome()
        {
            SceneEvents.TriggerHomeComplete();
        }
    }
}
