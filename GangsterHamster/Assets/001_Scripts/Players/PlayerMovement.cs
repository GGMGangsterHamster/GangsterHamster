using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Commands.Movement
{
    /// <summary>
    /// wasd 움직임을 구현한 클레스.
    /// </summary>
    public class PlayerMovement : MonoBehaviour, IMoveable
    {
        /// <summary>
        /// 이동 시 호출됨. Vector3 = direction
        /// </summary>
        public event System.Action<Vector3> OnMove;

        [SerializeField] private float speed = 1.0f;

        private void Awake()
        {
            OnMove += (dir) => { };
        }

        public void MoveFoward()
        {
            Move(transform.forward);
        }

        public void MoveBackword()
        {
            Move(-transform.forward);
        }

        public void MoveLeft()
        {
            Move(-transform.right);
        }

        public void MoveRight()
        {
            Move(transform.right);
        }

        private void Move(Vector3 dir)
        {
            transform.Translate(dir * speed * Time.deltaTime);
            OnMove(dir);
        }
    }
}