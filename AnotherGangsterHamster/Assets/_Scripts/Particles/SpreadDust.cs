using Matters.Gravity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Particles
{
    public class SpreadDust : MonoBehaviour
    {
        ParticleSystem _particleSystem;

        bool isPlayed = false;
        private void Start()
        {
            _particleSystem = GetComponent<ParticleSystem>();
        }

        private void Update()
        {
            if(Physics.Raycast(transform.position, GravityManager.GetGlobalGravityDirection(), out RaycastHit hit) 
                && (hit.transform.CompareTag("BTYPEOBJECT") || hit.transform.CompareTag("ATYPEOBJECT")))
            {
                if(!isPlayed && Vector3.Distance(transform.position, hit.point) <= 0.2f)
                {
                    _particleSystem.Play();
                    isPlayed = true;
                }
            }
            else
            {
                isPlayed = false;
            }
        }
    }
}