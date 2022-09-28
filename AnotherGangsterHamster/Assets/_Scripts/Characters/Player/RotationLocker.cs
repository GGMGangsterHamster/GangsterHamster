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
        private bool executeMouseRot = true;

        private void Start()
        {
            mouseX = new MouseX(FindObjectOfType<Mouse.Mouse>());

            _callback = rot => {
                _y += rot;
                // 4원수는 교환 법칙이 성립되지 않는대요
                // 4원수 * 곱하고자 하는 수 = local
                // 곱하고자 하는 수 * 4원수 = world
                Quaternion target = Quaternion.Euler(-_gravity * _y) * _lock;
                transform.rotation = target;
            };

            if (FindObjectOfType<WeaponManagement>()?.startHandleWeapon != WeaponEnum.Gravito)
                return;
            

            CheckpointManager checkpoint
                = FindObjectOfType<CheckpointManager>();

            GravitoMessageBroker message
                = FindObjectOfType<GravitoMessageBroker>();

            message.OnLerpEnd.AddListener(() => {
                executeMouseRot = true;
            });

            message.OnUse.AddListener(() => {
                _lock = checkpoint.endCheckpoint.rotation;
                _gravity = GravityManager.GetGlobalGravityDirection();
                _y = 0.0f;
                executeMouseRot = false;
            });

            message.OnReset.AddListener(() => {
                _y = transform.eulerAngles.y;
                _lock = Quaternion.identity;
                _gravity = Vector3.down;
                executeMouseRot = false;
            });

        }


        private void Update()
        {
            if (executeMouseRot)
                mouseX.Execute?.Invoke(_callback);
        }
    }
}