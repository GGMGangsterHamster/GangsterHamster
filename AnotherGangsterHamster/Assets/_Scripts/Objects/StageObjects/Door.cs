using Objects.Interaction;
using UnityEngine;

namespace Objects.StageObjects
{
   public class Door : Interactable
   {
      [SerializeField] private GameObject _door;
      [SerializeField] private float _doorHeldOpenDuration = 3.0f;

      private AudioSource _doorOpenSound;

      void Awake()
      {
         _doorOpenSound = Resources.Load<AudioSource>("Audio/SoundEffect/7(UsingSound)_"); 
      }

      public void Active()
      {
         _door.SetActive(false);
         Invoke(nameof(Deactive), _doorHeldOpenDuration);
         _doorOpenSound.Play();
      }

      public void Deactive()
      {
         _door.SetActive(true);
      }

      public override void Interact()
      {
         Active();
      }

      public override void Focus() { }
      public override void DeFocus() { }
      
   }
}
