using Characters.Player.Bridge;
using UnityEngine;

namespace _Core
{
   public class BridgeManager : MonoBehaviour
   {
      private void Awake()
      {
         // TODO: 모노싱글톤은 아닌데 Dontdestroyonload 인 오브젝트의 중복 체크
      }

      private void Start()
      {
         new ValueActionBridge();
      }
   }
}