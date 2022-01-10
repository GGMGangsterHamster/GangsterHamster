using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Commands.Movement
{
    /// <summary>
    /// 기본적인 움직임을 구현한 클레스
    /// </summary>
    [RequireComponent(typeof(Rigidbody))]
    public class PlayerMovement : MonoBehaviour, IMoveable, IJumpable
    {
        private Rigidbody rigid;

        /// <summary>
        /// 이동 시 호출됨. Vector3 = direction<br/>
        /// 입력 키 누르는 동안 계속 호출됨
        /// </summary>
        public event System.Action<Vector3> OnMove;

        /// <summary>
        /// 점프 시 호출됨
        /// </summary>
        public event System.Action OnJump;

        [SerializeField] private float speed = 1.0f;
        [SerializeField] private float jumpForce = 3.0f;

        private void Awake()
        {
            OnMove += (dir) => { };
            OnJump += () => { };
            rigid = GetComponent<Rigidbody>();
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

        public void Jump()
        {
            Log.Debug.Log("Have to fix PlayerMovement::Jump()", Log.LogLevel.Normal);
            rigid.velocity = Vector3.zero;

            rigid.AddForce(transform.up * jumpForce, ForceMode.Impulse);
        }

        private void Move(Vector3 dir)
        {
            transform.Translate(dir * speed * Time.deltaTime);
            OnMove(dir);
        }

    }
}