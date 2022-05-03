using UnityEngine;
using Stages.Management;

namespace Objects.Trigger
{    
    [RequireComponent(typeof(TriggerInteractableObject))]
    public class SceneLoadTrigger : MonoBehaviour, ICollisionEventable
    {
        const string PLAYER = "PLAYER_BASE";
        [SerializeField] StageNames Load;

        public void Active(GameObject other)
        {
            StageManager.Instance.Load(Load);
        }

        public void Deactive(GameObject other) {}
    }
}