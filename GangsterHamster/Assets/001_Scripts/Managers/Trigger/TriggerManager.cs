using System;
using System.Collections.Generic;
using UnityEngine;

namespace Objects.Trigger
{
    /// <summary>
    /// 트리거 관리 클래스
    /// </summary>
    public class TriggerManager : Singleton<TriggerManager>, ISingletonObject
    {
        /// <summary>
        /// 맵에 존재하는 모든 트리거
        /// </summary>
        Dictionary<int, ITrigger> _triggerDictionary = new Dictionary<int, ITrigger>();

        #region 등록

        /// <summary>
        /// 트리거를 등록합니다.
        /// </summary>
        /// <param name="trigger">등록할 트리거</param>
        public void AddTrigger(ITrigger trigger, bool suppressDuplicateWarning = false)
        {
            if(_triggerDictionary.ContainsValue(trigger))
            {
                Logger.Log("TriggerManager > Trigger 중복 등록. 의도된 것이라면 suppressDuplicateWarning 을 true 로 전달하세요.", LogLevel.Warning);
            }

            trigger.ID = _triggerDictionary.Count;

            _triggerDictionary.Add(trigger.ID, trigger);
        }

        /// <summary>
        /// 트리거를 등록 해제합니다.
        /// </summary>
        /// <param name="trigger">등록 해제할 트리거</param>
        public void RemoveTrigger(ITrigger trigger)
        {
            if(!TriggerAssigned(trigger.ID)) return;

            _triggerDictionary.Remove(trigger.ID);
        }
        /// <summary>
        /// 트리거를 등록 해제합니다.
        /// </summary>
        /// <param name="id">등록 해제할 트리거의 id</param>
        public void RemoveTrigger(int id)
        {
            if(!TriggerAssigned(id)) return;

            _triggerDictionary.Remove(id);
        }

        #endregion // 등록

        #region 활성화

        /// <summary>
        /// 트리거를 활성화합니다.
        /// </summary>
        /// <param name="trigger">활성화 할 트리거</param>
        public void ActivateTrigger(ITrigger trigger)
        {
            if(!TriggerAssigned(trigger.ID)) return;

            trigger.Activated = true;
        }
        /// <summary>
        /// 트리거를 활성화합니다.
        /// </summary>
        /// <param name="id">활성화 할 트리거의 id</param>
        public void ActivateTrigger(int id)
        {
            if(!TriggerAssigned(id)) return;

            _triggerDictionary[id].Activated = true;
        }

        /// <summary>
        /// 트리거를 비활성화합니다.
        /// </summary>
        /// <param name="trigger">비활성화 할 트리거</param>
        public void DeactivateTrigger(ITrigger trigger)
        {
            if(!TriggerAssigned(trigger.ID)) return;

            trigger.Activated = false;
        }
        /// <summary>
        /// 트리거를 비활성화합니다.
        /// </summary>
        /// <param name="id">비활성화 할 트리거의 id</param>
        public void DeactivateTrigger(int id)
        {
            if(!TriggerAssigned(id)) return;

            _triggerDictionary[id].Activated = false;
        }

        #endregion // 활성화

        /// <summary>
        /// 트리거를 가져옵니다.
        /// </summary>
        /// <param name="id">가져올 트리거의 ID</param>
        /// <returns>null when dictionary doess not contains request id</returns>
        public ITrigger GetTrigger(int id)
        {
            if(!TriggerAssigned(id)) return null;

            return _triggerDictionary[id];
        }

        /// <summary>
        /// 트리거를 교채합니다
        /// </summary>
        /// <param name="id">교채할 트리거 ID</param>
        /// <param name="trigger">앞으로 사용할 트리거</param>
        public void ReplaceTrigger(int id, ITrigger trigger)
        {
            if(!TriggerAssigned(id)) return;

            _triggerDictionary[id] = trigger;
        }

        #region 이벤트

        /// <summary>
        /// 트리거에 이벤트를 추가합니다.
        /// </summary>
        /// <param name="id">대상 트리거 id</param>
        /// <param name="callback">추가할 이벤트<br/>OnTrigger(작동시킨 오브젝트);</param>
        public void AddEvent(int id, Action<GameObject> callback)
        {
            if(!TriggerAssigned(id)) return;

            _triggerDictionary[id].OnTrigger += callback;
        }
        /// <summary>
        /// 트리거에 이벤트를 추가합니다.
        /// </summary>
        /// <param name="trigger">대상 트리거</param>
        /// <param name="callback">추가할 이벤트<br/>OnTrigger(작동시킨 오브젝트);</param>
        public void AddEvent(ITrigger trigger, Action<GameObject> callback)
        {
            if(!TriggerAssigned(trigger.ID)) return;

            trigger.OnTrigger += callback;
        }

        /// <summary>
        /// 트리거에 이벤트를 제거합니다.
        /// </summary>
        /// <param name="id">대상 트리거 id</param>
        /// <param name="callback">제거할 이벤트</param>
        public void RemoveEvent(int id, Action<GameObject> callback)
        {
            if(!TriggerAssigned(id)) return;

            _triggerDictionary[id].OnTrigger -= callback;
        }
        /// <summary>
        /// 트리거에 이벤트를 제거합니다.
        /// </summary>
        /// <param name="trigger">대상 트리거</param>
        /// <param name="callback">제거할 이벤트</param>
        public void RemoveEvent(ITrigger trigger, Action<GameObject> callback)
        {
            if(!TriggerAssigned(trigger.ID)) return;

            trigger.OnTrigger -= callback;
        }

        #endregion // 이벤트

        /// <summary>
        /// 트리거가 등록되었는지 확인
        /// </summary>
        /// <param name="id">확인할 트리거의 id</param>
        /// <returns>false when not assigned</returns>
        public bool TriggerAssigned(int id)
        {
            if(!_triggerDictionary.ContainsKey(id)) 
            {
                Logger.Log($"TriggerManager > 존재하지 않는 트리거 ID{id}", LogLevel.Fatal);
                return false;
            }

            return true;
        }

    }
}