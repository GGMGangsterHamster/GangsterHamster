using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingObject : MonoBehaviour
{
    public List<Vector3> localDestinationList = new List<Vector3>();

    public float moveSpeed;

    public bool isSleep = false;

    public List<Vector3> globalDestination = new List<Vector3>();

    private bool ismoving;

    public int curdest = 0;

    private void Awake()
    {
        localDestinationList.Add(Vector3.zero);
        for (int i = localDestinationList.Count - 1; i > 0; i--)
        {
            Vector3 temp = localDestinationList[i];
            localDestinationList[i] = localDestinationList[i - 1];
            localDestinationList[i - 1] = temp;
        }

        globalDestination.Add(transform.position);

        for (int i = 1; i < localDestinationList.Count; i++)
        {
            globalDestination.Add(localDestinationList[i] + globalDestination[i - 1]);
        }
    }

    private void Start()
    {
        if(localDestinationList == null)
        {
            Log.Debug.Log("MovingObject의 목적지가 없습니다!!", Log.LogLevel.Fatal);
        }

        StartCoroutine(NextDestination());
        ismoving = true;
    }

    private void Update()
    {
        if(isSleep)
        {
            StopAllCoroutines();
            StartCoroutine(PrevDestination());
            ismoving = false;
        }
        else if(!isSleep && !ismoving)
        {
            StopAllCoroutines();
            StartCoroutine(NextDestination());
            ismoving = true;
        }
    }

    IEnumerator NextDestination()
    {
        Vector3 dir = (globalDestination[curdest] - transform.position).normalized;

        while(true)
        {
            transform.position += dir * Time.deltaTime * moveSpeed;

            if(Vector3.Distance(transform.position, globalDestination[curdest]) <= 0.2f)
            {
                transform.position = globalDestination[curdest];
                break;
            }

            yield return null;
        }


        if(curdest == globalDestination.Count - 1)
        {
            curdest--;
            StartCoroutine(PrevDestination());
        }
        else
        {
            curdest++;
            StartCoroutine(NextDestination());
        }
    }

    IEnumerator PrevDestination()
    {
        Vector3 dir = (globalDestination[curdest] - transform.position).normalized;

        while (true)
        {
            transform.position += dir * Time.deltaTime * moveSpeed;

            if (Vector3.Distance(transform.position, globalDestination[curdest]) <= 0.2f)
            {
                break;
            }

            yield return null;
        }

        if (curdest == 0)
        {
            curdest++;
            StartCoroutine(NextDestination());
        }
        else
        {
            curdest--;
            StartCoroutine(PrevDestination());
        }
    }
}
