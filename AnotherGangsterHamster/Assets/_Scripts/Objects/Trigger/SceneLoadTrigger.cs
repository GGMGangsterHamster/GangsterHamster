using UnityEngine;
using Stages.Management;

namespace Objects.Trigger
{    
    [RequireComponent(typeof(TriggerInteractableObject))]
    public class SceneLoadTrigger : MonoBehaviour, ICollisionEventable
    {
        const string PLAYER = "PLAYER_BASE";
        public string LoadTarget; 

        public void Active(GameObject other)
        {
            StageManager.Instance.Load(LoadTarget);
        }

        public void Deactive(GameObject other) {}
    }
}