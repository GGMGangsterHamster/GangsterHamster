using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sound;

namespace Characters.Player.Move 
{
    public class MoveSound : SoundController
    {
        private MoveInputHandler _moveInputHandler;

        private void Start()
        {
            _moveInputHandler = GetComponent<MoveInputHandler>();
            _moveInputHandler.forward.Execute.AddListener(PlaySound);
            _moveInputHandler.right.Execute.AddListener(PlaySound);
            _moveInputHandler.left.Execute.AddListener(PlaySound);
            _moveInputHandler.backward.Execute.AddListener(PlaySound);
        }

        public override void PlaySound(object obj)
        {
            SoundManager.Instance.Play("PlayerMove");
        }
    }
}


