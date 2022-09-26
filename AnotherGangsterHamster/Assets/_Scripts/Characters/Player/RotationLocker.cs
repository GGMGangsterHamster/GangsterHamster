using Matters.Gravity;
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Animations; 
using Objects;
using Weapons.Checkpoint;
using Weapons.Actions;
using Weapons.Actions.Broker;
using Characters.Player.Mouse;
using System;

namespace Characters.Player
{
    public class RotationLocker : MonoBehaviour
    {
        private Vector3 _lock = Vector3.zero;
        private Vector3 _gravity = Vector3.zero;
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
                _gravity = GravityManager.GetGlobalGravityDirection();
            });
        }

        float y = 0.0f;

        private void Update()
        {

            Action<float> callback = rot => {
                y += rot;
                // 4원수는 교환 법칙이 성립되지 않는대요
                // 4원수 * 곱하고자 하는 수 = local
                // 곱하고자 하는 수 * 4원수 = world
                float angle = Vector3.Angle(Vector3.zero, _gravity);
                Debug.Log(_gravity + " GRA");
                Debug.Log(angle);
                var a = new Vector3(0.0f, y, 0.0f);
                transform.eulerAngles = _lock + a;

                // TODO: 마우스 Y 를 중력 방향에 맞게만 하면 다 됨
                // 싯팔
            };

            mouseX.Execute?.Invoke(callback);
        }
    }
}