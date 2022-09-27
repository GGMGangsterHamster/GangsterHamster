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
        private Vector3 _lock = Vector3.zero;
        private Vector3 _gravity = Vector3.down;
        private MouseX mouseX;

        private void Start()
        {
            mouseX = new MouseX(FindObjectOfType<Mouse.Mouse>());

            CheckpointManager checkpoint
                = FindObjectOfType<CheckpointManager>();

            GravitoMessageBroker message
                = FindObjectOfType<GravitoMessageBroker>();

            message.OnUse.AddListener(() => {
                _lock = checkpoint.endCheckpoint.eulerAngles;
                _gravity = GravityManager.GetGlobalGravityDirection();
            });

            message.OnReset.AddListener(() => {
                _lock = Vector3.zero;
                _gravity = Vector3.down;
            });
        }

        float y = 0.0f;
        float yy = 0;

        private void Update()
        {

            Action<float> callback = rot => {
                y += rot;
                // 4원수는 교환 법칙이 성립되지 않는대요
                // 4원수 * 곱하고자 하는 수 = local
                // 곱하고자 하는 수 * 4원수 = world
                // Debug.Log(_lock + -_gravity * y);
                // Debug.Log(transform.eulerAngles + " ANGLE");
                // Debug.Log(_lock + " LOCK");
                // lock 한거 

                Debug.Log(_lock + (-_gravity * y));
                Quaternion target = Quaternion.Euler(-_gravity * y) * Quaternion.Euler(_lock);
                transform.rotation = target;
                // FIXME: 그라비토 능력 사용 시 각도 snap 현상
            };

            mouseX.Execute?.Invoke(callback);
        }
    }
}