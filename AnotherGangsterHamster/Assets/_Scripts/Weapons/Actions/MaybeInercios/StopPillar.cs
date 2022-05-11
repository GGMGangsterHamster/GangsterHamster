using Matters.Gravity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Weapons.Actions
{
    public struct BeforeRigidValue
    {
        public Vector3 velocity;
        public Vector3 angularVelocity;
        public bool useGravity;
        public RigidbodyConstraints constraints;

        public BeforeRigidValue(Vector3 velocity, Vector3 angularVelocity, bool useGravity, RigidbodyConstraints constraints)
        {
            this.velocity = velocity;
            this.angularVelocity = angularVelocity;
            this.useGravity = useGravity;
            this.constraints = constraints;
        }
    }
    public class StopPillar : MonoBehaviour
    {
        private Dictionary<GameObject, BeforeRigidValue> _colDict = new Dictionary<GameObject, BeforeRigidValue>();

        public void ObjTriggerStayEvent(GameObject obj)
        {
            if (obj.CompareTag("PLAYER_BASE")) return;
            if (obj.GetComponent<GravityAffectedObject>() == null) return;

            StopObj(obj);
        }

        public void ObjTriggerExitEvent(GameObject obj)
        {
            if (obj.CompareTag("PLAYER_BASE")) return;

            if (_colDict.ContainsKey(obj))
            {
                Debug.Log("Exited : " + obj.name);
                MoveObj(obj);
                _colDict.Remove(obj);
            }
        }

        private void StopObj(GameObject obj)
        {
            Rigidbody rigid = obj.GetComponent<Rigidbody>();

            if(rigid != null)
            {
                if(!_colDict.ContainsKey(obj))
                {
                    _colDict.Add(obj, new BeforeRigidValue(rigid.velocity, rigid.angularVelocity, rigid.useGravity, rigid.constraints));
                }

                rigid.velocity = Vector3.zero;
                rigid.angularVelocity = Vector3.zero;
                rigid.useGravity = false;
                rigid.constraints = RigidbodyConstraints.FreezeAll;
            }
        }

        private void MoveObj(GameObject obj)
        {
            BeforeRigidValue value = _colDict[obj];
            Rigidbody rigid = obj.GetComponent<Rigidbody>();

            rigid.velocity = value.velocity;
            rigid.angularVelocity = value.angularVelocity;
            rigid.useGravity = value.useGravity;
            rigid.constraints = value.constraints;
        }

        private void OnDisable()
        {
            foreach(GameObject obj in _colDict.Keys)
            {
                MoveObj(obj);
            }
        }
    }
}