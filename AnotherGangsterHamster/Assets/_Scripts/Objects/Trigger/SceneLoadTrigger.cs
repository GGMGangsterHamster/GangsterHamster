using UnityEngine;
using Stages.Management;
using Objects.InteractableObjects;

namespace Objects.Trigger
{    
    [RequireComponent(typeof(TriggerInteractableObject))]
    public class SceneLoadTrigger : MonoBehaviour, IEventable
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