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
        //        Debug.Log("테스트");
        //    });


        //    unGrep.AddListener(() =>
        //    {
        //        Debug.Log("테스트12");
        //    });
        //}
    }
}