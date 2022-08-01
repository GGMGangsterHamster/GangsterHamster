using UnityEngine;

namespace Characters.Player.OnGround
{  
   public class ChangePhysicsMat : MonoBehaviour
   {
      private PhysicMaterial _ground;
      private PhysicMaterial _wall;

      private void Awake()
      {
         _wall = Resources.Load<PhysicMaterial>("Materials/Physics/Wall");
         _ground = Resources.Load<PhysicMaterial>("Materials/Physics/Ground");
      }

      
      
   }
}