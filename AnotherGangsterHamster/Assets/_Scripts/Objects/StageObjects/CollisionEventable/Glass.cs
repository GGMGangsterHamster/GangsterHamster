using Characters.Player.OnGround;
using Objects.InteractableObjects;
using UnityEngine;
using UnityEngine.Events;
using System.Collections;

namespace Objects.StageObjects.CollisionEventable
{
    [RequireComponent(typeof(CollisionInteractableObject))]
    public class Glass : MonoBehaviour, IEventable
    {
        [SerializeField] private MeshRenderer meshRenderer;
        [SerializeField] private BoxCollider boxCollider;

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
                    OnBreak?.Invoke();
                    other.GetComponentInChildren<OnGround>()?.ExitGround();
                    meshRenderer.enabled = false;
                    boxCollider.enabled = false;
                    
                }
            }
        }

        public void Deactive(GameObject other) { }
    }
}