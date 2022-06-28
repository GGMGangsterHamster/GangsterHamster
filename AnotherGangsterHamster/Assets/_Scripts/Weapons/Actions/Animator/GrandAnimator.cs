using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Weapons.Actions;

namespace Weapon.Animation.GrandAnimation
{
    public class GrandAnimator : MonoBehaviour
    {
        private Transform _mainCameraTransform;
        private Transform MainCameraTransform
        {
            get
            {
                if (_mainCameraTransform == null)
                {
                    _mainCameraTransform = Camera.main.transform;
                }

                return _mainCameraTransform;
            }
        }

        private Transform chargeBar;

        private GrandSizeLevel _beforeSizeLevel;
        private GrandSizeLevel _currentSizeLevel;

        enum GrandAnimeStatus // 현재 애니메이션 스테이터스
        {
            Idle,
            Move,
            Reset,
            Using,
            Sorting
        }

        private void Awake()
        {
            chargeBar = GameObject.Find("ChargeBar").transform;
        }

        public void ResetAnime(Vector3 start, Vector3 end, float moveSpeed)
        {

        }

        public void UsingAnime(GrandSizeLevel beforeSizeLevel, GrandSizeLevel currentSizeLevel)
        {
            _beforeSizeLevel = beforeSizeLevel;
            _currentSizeLevel = currentSizeLevel;
        }

    }
}