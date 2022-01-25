using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Commands.Movement
{
    /// <summary>
    /// 기본적인 wasd + shift 움직임을 이용하기 위한 Interface
    /// </summary>
    public interface IMoveable
    {
        public void MoveFoward();
        public void MoveBackword();
        public void MoveLeft();
        public void MoveRight();
        public void Dash();
    }
}