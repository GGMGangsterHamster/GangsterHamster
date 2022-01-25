using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Player.Status;
using Commands.Movement;
using Player.Utils;

namespace Player.Movement
{
    /// <summary>
    /// 기본적인 움직임을 구현한 클레스
    /// </summary>
    [RequireComponent(typeof(Rigidbody))]
    public class PlayerMovement : MonoBehaviour, IMoveable, IJumpable, IMouseDeltaRecvable, ICrouchable
    {
        public Transform camTrm = null;

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
            camTrm = Camera.main.transform;
        }

        #region Movement

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

        public void Dash()
        {
            if(PlayerStatus.Instance.IsCrouching) {
                Log.Debug.Log("웅크린 상태 중 Dash 명령.");
                PlayerUtils.Instance.SetStanded();
            }

            PlayerStatus.Instance.IsRunning = !PlayerStatus.Instance.IsRunning;
            PlayerValues.Instance.speed = PlayerStatus.Instance.IsRunning ? PlayerValues.DashSpeed : PlayerValues.WalkingSpeed;
        }

        private void Move(Vector3 dir)
        {
            if(!PlayerStatus.Instance.Moveable) return;

            // Space.World 추가하는 거 수정함!
            transform.Translate(dir * PlayerValues.Instance.speed * Time.deltaTime, Space.World);
            OnMove(dir);
        }

        #endregion // Movement

        #region Mouse delta

        public void OnMouseX(float x)
        {
            camTrm.eulerAngles    += new Vector3(0, x, 0);
            transform.eulerAngles += new Vector3(0, x, 0);
        }

        public void OnMouseY(float y)
        {
            camTrm.eulerAngles += new Vector3(-y, 0, 0);
        }

        #endregion

        #region Jump

        public void Jump()
        {
            if(!PlayerStatus.Instance.OnGround || !PlayerStatus.Instance.Jumpable) return;

            if(PlayerStatus.Instance.IsCrouching) {
                Log.Debug.Log("웅크린 상태 중 Jump 명령.");
                PlayerUtils.Instance.SetStanded();
                return;
            }

            Log.Debug.Log("Have to fix PlayerMovement::Jump()", Log.LogLevel.Normal);
            rigid.velocity = Vector3.zero;

            rigid.AddForce(transform.up * jumpForce, ForceMode.Impulse);
        }

        public void OnGround()
        {
            PlayerStatus.Instance.OnGround = true;
            PlayerStatus.Instance.Jumpable = true;
        }

        #endregion // Jump

        public void Crouch()
        {
            if(!PlayerStatus.Instance.IsCrouching) {
                PlayerUtils.Instance.SetCrouched();
            }
            else {
                PlayerUtils.Instance.SetStanded();
            }
        }
    }
}