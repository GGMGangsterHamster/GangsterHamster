using UnityEngine;
using System.Collections.Generic;
using _Core.Commands;
using Objects;


namespace Characters.Player.Actions
{
   [RequireComponent(typeof(Actions))]
   public class ActionInputHandler : MonoBehaviour
   {
      public Transform RightHandTrm = null;
      public string _path = "KeyCodes/Actions.json";

      private Dictionary<KeyCode, Command> _actionDownCommands;
      private Dictionary<KeyCode, Command> _actionUpCommands;

      // IActionable 구체화 한 클레스
      private Actions _actions;

      // 키코드
      private ActionVO _key;

      // 엑션 커멘드
      public CrouchStart  crouchStart;
      public CrouchEnd    crouchEnd;
      public Jump         jump;
      public Interaction  interaction;

      private void Awake()
      {
         _actionDownCommands  = new Dictionary<KeyCode, Command>();
         _actionUpCommands    = new Dictionary<KeyCode, Command>();
         _actions             = GetComponent<Actions>();

         crouchStart   = new CrouchStart(_actions);
         crouchEnd     = new CrouchEnd(_actions);
         jump          = new Jump(_actions);
         interaction   = new Interaction(_actions);

         RemapCommands();
      }

      public void RemapCommands()
      {
         _actionDownCommands.Clear();

         _key = Utils.JsonToVO<ActionVO>(_path);

         _actionDownCommands.Add((KeyCode)_key.Crouch,     crouchStart);
         _actionDownCommands.Add((KeyCode)_key.Jump,       jump);

         _actionUpCommands.Add((KeyCode)_key.Crouch, crouchEnd);
      }

      private void Update()
      {
         foreach (KeyCode key in _actionDownCommands.Keys)
         {
            if (Input.GetKeyDown(key))
               _actionDownCommands[key].Execute.Invoke(null);
         }

         foreach (KeyCode key in _actionUpCommands.Keys)
         {
            if (Input.GetKeyUp(key))
               _actionUpCommands[key].Execute.Invoke(null);
         }

         // 상호작용
         if (Input.GetKeyDown((KeyCode)_key.Interact))
         {
            interaction.Execute.Invoke(RightHandTrm);
         }
      }

   }
   
}