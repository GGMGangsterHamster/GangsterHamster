using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CubeSpawner : MonoBehaviour
{
    public GameObject ACube; // <= spawn object

    public float delay;

    [Header("�̺�Ʈ")]
    public UnityEvent respawnStartEvent;
    public UnityEvent respawnEndEvent;

    private Rigidbody aCubeRigidbody;

    private bool respawning = false;

    private void Start()
    {
        aCubeRigidbody = ACube.GetComponent<Rigidbody>();
        
        if(aCubeRigidbody)
        {
            Debug.Log("���� ť�갡 �����ϴ�");
        }
    }

    public void CubeDestory()
    {
        Respawn();
    }

    private void Respawn()
    {
        Debug.Log("ť�� ������");
        respawnStartEvent?.Invoke();
        respawning = true;
        Invoke("RespawnValues", delay);
    }

    void RespawnValues()
    {
        // reset values
        ACube.transform.position = transform.position;
        ACube.transform.rotation = Quaternion.identity;
        aCubeRigidbody.velocity = Vector3.zero;
        aCubeRigidbody.angularVelocity = Vector3.zero;
        respawning = false;

        if (!ACube.activeSelf)
            ACube.SetActive(true);

        respawnEndEvent?.Invoke();
    }

    public bool GetIsRespawn()
    {
        return respawning;
    }
}
