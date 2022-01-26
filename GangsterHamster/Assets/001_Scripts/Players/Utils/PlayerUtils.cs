using Player.Status;
using UnityEngine;


namespace Player.Utils
{
    public class PlayerUtils : Singleton<PlayerUtils>, ISingletonObject
    {
        Transform _playerTrm;

        public PlayerUtils()
        {
            _playerTrm = GameObject.FindGameObjectWithTag("PLAYER").transform;
        }

        /// <summary>
        /// 웅크린 상태로 변경
        /// </summary>
        public void SetCrouched()
        {
            PlayerStatus.Instance.IsCrouching = true;
            PlayerValues.Instance.speed = PlayerValues.CrouchSpeed;
            _playerTrm.localScale = new Vector3(_playerTrm.localScale.x, PlayerValues.PlayerCrouchYScale, _playerTrm.localScale.z);
            _playerTrm.localPosition = new Vector3(0.0f, PlayerValues.PlayerCrouchYPos, 0.0f);
            GameManager.Instance.player.GetComponent<BoxCollider>().size = new Vector3(0.45f, 1.0f, 0.08f);
            GameManager.Instance.player.GetComponent<BoxCollider>().center = new Vector3(0.0f, 0.5f, 0.0f);
        }

        /// <summary>
        /// 일어선 상태로 변경
        /// </summary>
        public void SetStanded()
        {
            PlayerStatus.Instance.IsCrouching = false;
            PlayerValues.Instance.speed = PlayerValues.WalkingSpeed;
            _playerTrm.localScale = new Vector3(_playerTrm.localScale.x, PlayerValues.PlayerYScale, _playerTrm.localScale.z);
            _playerTrm.localPosition = new Vector3(0.0f, PlayerValues.PlayerYPos, 0.0f);
            GameManager.Instance.player.GetComponent<BoxCollider>().size = new Vector3(0.45f, 1.8f, 0.08f);
            GameManager.Instance.player.GetComponent<BoxCollider>().center = new Vector3(0.0f, 0.9f, 0.0f);
        }


    }
}