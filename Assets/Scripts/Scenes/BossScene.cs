using UnityEngine;
using SampleGame.Core;

namespace SampleGame.Scenes
{
    /// <summary>
    /// Boss scene controller.
    /// Does not decide what comes next - only emits events.
    /// </summary>
    public class BossScene : MonoBehaviour
    {
        private void Start()
        {
            Debug.Log("[BossScene] Boss scene loaded");
        }
        
        private void Update()
        {
            // Simple trigger: Press Space to complete boss fight
            // This is placeholder logic - in real game this would be boss defeat/player death
            if (Input.GetKeyDown(KeyCode.Space))
            {
                Debug.Log("[BossScene] Boss fight completed");
                CompleteBoss();
            }
        }
        
        /// <summary>
        /// Called when boss fight is complete
        /// Emits event instead of deciding next scene
        /// </summary>
        private void CompleteBoss()
        {
            SceneEvents.TriggerBossComplete();
        }
    }
}
