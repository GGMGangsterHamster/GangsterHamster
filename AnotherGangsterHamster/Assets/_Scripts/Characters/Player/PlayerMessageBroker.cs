using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Characters.Player
{
    public class PlayerMessageBroker : MonoBehaviour
    {
        public UnityEvent grep;
        public UnityEvent unGrep;

        //private void Start()
        //{
        //    grep.AddListener(() =>
        //    {
        //        Debug.Log("�׽�Ʈ");
        //    });


        //    unGrep.AddListener(() =>
        //    {
        //        Debug.Log("�׽�Ʈ12");
        //    });
        //}
    }
}