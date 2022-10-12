using Characters.Player.OnGround;
using Objects.InteractableObjects;
using UnityEngine;
using UnityEngine.Events;

namespace Objects.StageObjects.CollisionEventable
{
    [RequireComponent(typeof(CollisionInteractableObject))]
    public class Glass : MonoBehaviour, IEventable
    {
        public float MaximunKineticEnergy = 10.0f;
        public UnityEvent OnBreak;

        CollisionInteractableObject _colInteractable;

        private void Awake()
        {
            _colInteractable = GetComponent<CollisionInteractableObject>();
        }

        public void Active(GameObject other)
        {
            if (other.TryGetComponent<Rigidbody>(out var rigid))
            {
                Debug.Log(_colInteractable.colVelocity.magnitude * rigid.mass);
                if (MaximunKineticEnergy < _colInteractable.colVelocity.magnitude * rigid.mass)
                {
                    other.GetComponentInChildren<OnGround>()?.ExitGround();
                    OnBreak.Invoke();
                    gameObject.SetActive(false);
                }
            }
        }

        public void Deactive(GameObject other) { }
    }
}