using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;

using Player.Status;
using Commands.Movement;
using Player.Utils;
using Objects.Utils;
using Gravity.Object.Management;

namespace Player.Movement
{
    /// <summary>
    /// 기본적인 움직임을 구현한 클레스
    /// </summary>
    [RequireComponent(typeof(Rigidbody))]
    public class PlayerMovement : MonoBehaviour, IMoveable, IJumpable, IMouseDeltaRecvable, ICrouchable
    {
        
        [Header("바닥과 플레이어 거리")]
        [SerializeField] private float _groundDistance;


        private Transform camTrm = null;
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
            if (!PlayerStatus.Instance.Moveable) return;
            PlayerMoveDelta.Instance.AddYDelta(PlayerValues.Instance.speed);
        }

        public void MoveBackword()
        {
            if (!PlayerStatus.Instance.Moveable) return;
            PlayerMoveDelta.Instance.AddYDelta(-PlayerValues.Instance.speed);
        }

        public void MoveLeft()
        {
            if (!PlayerStatus.Instance.Moveable) return;
            PlayerMoveDelta.Instance.AddXDelta(-PlayerValues.Instance.speed);
        }

        public void MoveRight()
        {
            if (!PlayerStatus.Instance.Moveable) return;
            PlayerMoveDelta.Instance.AddXDelta(PlayerValues.Instance.speed);
        }

        public void Dash()
        {
            if(PlayerStatus.Instance.IsCrouching) {
                Logger.Log("웅크린 상태 중 Dash 명령.");
                PlayerUtils.Instance.SetStanded();
            }

            PlayerStatus.Instance.IsRunning = !PlayerStatus.Instance.IsRunning;
            PlayerValues.Instance.speed = PlayerStatus.Instance.IsRunning ? PlayerValues.DashSpeed : PlayerValues.WalkingSpeed;
        }

        #endregion // Movement

        #region Mouse delta

        public void OnMouseX(float x)
        {
            // 4원수는 교환 법칙이 성립되지 않는대요
            // 4원수 * 곱하고자 하는 수 = local
            // 곱하고자 하는 수 * 4원수 = world
            transform.rotation = transform.rotation * Quaternion.Euler(0.0f, x * PlayerValues.Instance.mouseSpeed, 0.0f);
        }

        float rotY = 0.0f;
        public void OnMouseY(float y, bool includingMouseSpeed = true)
        {
            rotY += -y * (includingMouseSpeed ? PlayerValues.Instance.mouseSpeed : 1);
            rotY = Mathf.Clamp(rotY, -90f, 90f);

            camTrm.transform.localRotation = Quaternion.Euler(rotY, 0.0f, 0.0f);
        }
        #endregion

        #region Jump

        public void Jump()
        {
            if(!GroundChecker.Instance.CheckGround(this.transform, _groundDistance) || !PlayerStatus.Instance.Jumpable) return;

            if(PlayerStatus.Instance.IsCrouching) {
                Logger.Log("웅크린 상태 중 Jump 명령.");
                PlayerUtils.Instance.SetStanded();
                return;
            }

            // Log.Debug.Log("Have to fix PlayerMovement::Jump()", Log.LogLevel.Normal);
            Vector3 force = new Vector3(1.0f, Mathf.Sqrt(2.0f * 9.8f * PlayerValues.JumpHeight), 1.0f); // TODO: Cache?
            Vector3 gravityDir = GravityManager.Instance.GetGlobalGravityDirection();

            force.x *= gravityDir.x;
            force.y *= gravityDir.y;
            force.z *= gravityDir.z;

            Debug.Log(-force);

            rigid.velocity = -force;

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