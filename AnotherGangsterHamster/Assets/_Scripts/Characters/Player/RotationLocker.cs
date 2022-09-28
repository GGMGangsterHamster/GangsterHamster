using Matters.Gravity;
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Animations; 
using Objects;
using Weapons.Checkpoint;
using Weapons.Actions;
using Weapons.Actions.Broker;
using Characters .Player.Mouse;
using System;

namespace Characters.Player
{
    public class RotationLocker : MonoBehaviour
    {
        private Quaternion _lock = Quaternion.identity;
        private Vector3 _gravity = Vector3.down;
        private MouseX mouseX;
        private float _y = 0.0f;

        private Action<float> _callback;

        private void Start()
        {
            mouseX = new MouseX(FindObjectOfType<Mouse.Mouse>());

            CheckpointManager checkpoint
                = FindObjectOfType<CheckpointManager>();

            GravitoMessageBroker message
                = FindObjectOfType<GravitoMessageBroker>();

            message.OnUse.AddListener(() => {
                _lock = checkpoint.endCheckpoint.rotation;
                _gravity = GravityManager.GetGlobalGravityDirection();
                _y = 0.0f;
                // TODO: 돌아갈 때 툭 튀는거만 해결하면 됨


            });

            message.OnReset.AddListener(() => {
                _lock = Quaternion.identity;
                _gravity = Vector3.down;
            });

            _callback = rot => {
                _y += rot;
                // 4원수는 교환 법칙이 성립되지 않는대요
                // 4원수 * 곱하고자 하는 수 = local
                // 곱하고자 하는 수 * 4원수 = world

                Quaternion target = Quaternion.Euler(-_gravity * _y) * _lock;
                transform.rotation = target;
                // FIXME: 그라비토 능력 사용 시 각도 snap 현상 => 위에 식이 Gravito Lerp 끝난 식과 다름
            };
        }


        private void Update()
        {
            mouseX.Execute?.Invoke(_callback);
        }
    }
}