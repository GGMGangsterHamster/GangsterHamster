using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Gravity.Object.Management
{
    public class GravityManager : MonoSingleton<GravityManager>
    {
        /// <summary>
        /// 중력에 영향을 받는 모든 오브젝트를 가지고 있는 Dictionary
        /// </summary>
        private Dictionary<int, GravityAffectedObject> gravityAffectedObjectDictionary;
        private WaitForEndOfFrame wait = new WaitForEndOfFrame();
        private GlobalGravity globalGravity = new GlobalGravity(Vector3.down);

        #region private bool Stop;
        private bool _stop = true;
        /// <summary>
        /// 중력 작동 중지 상태
        /// </summary>
        public bool Stop {
            get { return _stop; }
            set {
                _stop = value;
            }
        }
        #endregion // private bool Stop;

        private void Start()
        {
            InitDictionary();

            Stop = false;
        }

        private void InitDictionary() // gravityAffectedObjectDictionary 초기화
        {
            GravityAffectedObject[] tempList = FindObjectsOfType<GravityAffectedObject>();

            gravityAffectedObjectDictionary = new Dictionary<int, GravityAffectedObject>();

            for (int i = 0; i < tempList.Length; ++i) { // Array => Dictionary
                tempList[i].GetComponent<Rigidbody>().useGravity = false; // 이 클레스가 중력을 대신할 것
                tempList[i].ID = i;
                gravityAffectedObjectDictionary.Add(i, tempList[i]);
            }
        }

        private void FixedUpdate()
        {
            if(Stop) return;
            Force();   
        }

        /// <summary>
        /// Dictionary 에 있는 모든 오브젝트에게 중력을 적용합니다.
        /// </summary>
        private void Force()
        {
            foreach (var obj in gravityAffectedObjectDictionary) {
                if(!obj.Value.AffectedByGlobalGravity) continue;
                obj.Value.Gravity(globalGravity.direction, globalGravity.force);
            }
        }


        #region 외부 공개 함수

        /// <summary>
        /// 전역 중력의 작용 방향을 바꿉니다.
        /// </summary>
        /// <param name="direction">중력 작용 방향</param>
        public void ChangeGlobalGravityDirection(Vector3 direction)
        {
            this.globalGravity.direction = direction.normalized;
        }

        /// <summary>
        /// 전역 중력의 작용 방향을 바꿉니다.
        /// </summary>
        /// <param name="angle">중력 작용 각도</param>
        public void ChangeGlobalGravityDirection(float angle)
        {
            Debug.Log(Quaternion.AngleAxis(angle, Vector3.one).eulerAngles.normalized);
            this.globalGravity.direction = Quaternion.AngleAxis(angle, Vector3.one).eulerAngles.normalized;
        }

        /// <summary>
        /// 전역 중력의 작용 방향을 가져옵니다.
        /// </summary>
        /// <returns>전역 중력의 방향 as Vector3</returns>
        public Vector3 GetGlobalGravityDirection()
        {
            return globalGravity.direction;
        }
    
        /// <summary>
        /// 전역 중력의 크기를 바꿉니다.
        /// </summary>
        /// <param name="force">중력 크기</param>
        public void ChangeGlobalGravityForce(float force)
        {
            this.globalGravity.force = force;
        }

        /// <summary>
        /// 전역 중력의 크기를 가져옵니다.
        /// </summary>
        /// <returns>전역 중력의 크기 as float</returns>
        public float GetGlobalGravityForce()
        {
            return globalGravity.force;
        }

        #endregion // 외부 공개 함수

    }

}