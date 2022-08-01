using Objects.StageObjects;
using UnityEngine;

namespace Objects.Materials
{
   public class ChangeColor : MonoBehaviour, IEventable
   {
      [SerializeField] private Color _active;
      [SerializeField] private Color _deactive;

      [SerializeField] public bool defaultColorStatusToActive = false;

      private MeshRenderer _mr = null;

      private void Awake()
      {
         _mr = GetComponent<MeshRenderer>();
         _mr.material.color
            = defaultColorStatusToActive ? _active : _deactive;

      }

      public void Active(GameObject other)
      {
         _mr.material.color = _active;
      }

      public void Deactive(GameObject other)
      {
         _mr.material.color = _deactive;
      }
   }
}