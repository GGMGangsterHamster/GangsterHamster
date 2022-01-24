using System;
using OpenRC;
using UnityEngine;


namespace Objects.Init
{

    public class ExampleOpenRCInitScript : IInitBase
    {
        public void Depend(MonoBehaviour mono)
        {

        }

        public void Start(object param)
        {
            Log.Debug.Log("후...");
        }

        public void Stop()
        {
            Log.Debug.Log("된다!");
        }
    }
}