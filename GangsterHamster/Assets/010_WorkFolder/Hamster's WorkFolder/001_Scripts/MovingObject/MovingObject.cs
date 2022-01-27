using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Objects.Interactable;

// Stop 함수
// Start 함수

public class MovingObject : MonoBehaviour
{
    public List<Vector3> localDestinationList = new List<Vector3>();
    public float moveSpeed = 2f;
    public bool playOnAwake = false;

    private List<Vector3> globalDestination = new List<Vector3>();
    public int curdest = 0;

    private List<Transform> colObjs = new List<Transform>();

    private void Awake()
    {
        // 무조건 자신의 위치를 처음에 넣음
        localDestinationList.Add(Vector3.zero);

        for (int i = localDestinationList.Count - 1; i > 0; i--)
        {
            Vector3 temp = localDestinationList[i];
            localDestinationList[i] = localDestinationList[i - 1];
            localDestinationList[i - 1] = temp;
        }

        // local을 global로 바꾸는 작업
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

        if(playOnAwake)
        {
            StartCoroutine(NextDestination());
        }
    }

    /// <summary>
    /// 계속 반복해서 움직이는 함수
    /// </summary>
    public void StartRepeatMove()
    {
        StopAllCoroutines();
        StartCoroutine(NextDestination(true));
    }
    /// <summary>
    /// 한번만 왕복하고 멈추는 함수
    /// </summary>
    public void StartOnceRepeatMove()
    {
        StopAllCoroutines();
        StartCoroutine(NextDestination(false, true));
    }
    /// <summary>
    /// 한번만 끝을 찍고 멈추는 함수
    /// </summary>
    /// <param name="dir"> 방향 - true는 앞으로 false는 뒤로 </param>
    public void StartDontRepeatMove(bool dir)
    {
        StopAllCoroutines();

        if (dir)
        {
            StartCoroutine(NextDestination(false));
        }
        else
        {
            StartCoroutine(PrevDestination(false));
        }
    }
    /// <summary>
    /// 멈추는 함수
    /// </summary>
    public void Stop()
    {
        StopAllCoroutines();
    }
    /// <summary>
    /// 처음 자리로 돌아오게 하는 함수
    /// </summary>
    public void Comeback()
    {
        StopAllCoroutines();
        PrevDestination(false);
    }

    /// <summary>
    /// 다음 Destination으로 이동하는 코루틴
    /// Destination에 도달하면 다음 목적지를 찾거나
    /// 멈추는 일을 한다
    /// </summary>
    /// <param name="repeat"> True일 경우 계속 반복한다 </param>
    /// <param name="onceRepeat"> True일 경우 한번만 반복한다 </param>
    /// <returns></returns>
    IEnumerator NextDestination(bool repeat = true, bool onceRepeat = false)
    {
        Vector3 dir = (globalDestination[curdest] - transform.position).normalized;
        float dist = Vector3.Distance(transform.position, globalDestination[curdest]);

        while (true)
        {
            transform.position += dir * Time.deltaTime * (dist / moveSpeed);

            for(int i = 0; i < colObjs.Count; i++)
            {
                colObjs[i].position += dir * Time.deltaTime * (dist / moveSpeed);
            }

            // 목적지와 거리가 0.2이하로 될때까지 계속 반복해서 나아감
            if (Vector3.Distance(transform.position, globalDestination[curdest]) <= 0.05f)
            {
                transform.position = globalDestination[curdest];
                break;
            }

            yield return null;
        }

        if (curdest == globalDestination.Count - 1)
        {
            if (repeat)
            {
                curdest--;
                StartCoroutine(PrevDestination(onceRepeat ? false : true));
            }
        }
        else
        {
            curdest++;
            StartCoroutine(NextDestination(repeat));
        }
    }

    /// <summary>
    /// 이전 Destination으로 이동하는 코루틴
    /// Destination에 도달하면 그 다음의 목적지를 찾거나
    /// 멈추는 등의 행동을 한다
    /// </summary>
    /// <param name="repeat"> True일 경우 계속 반복 </param>
    /// <param name="onceRepeat"> True일 경우 한번만 반복 </param>
    /// <returns></returns>
    IEnumerator PrevDestination(bool repeat = true, bool onceRepeat = false)
    {
        Vector3 dir = (globalDestination[curdest] - transform.position).normalized;
        float dist = Vector3.Distance(transform.position, globalDestination[curdest]);

        // 목적지와 거리가 0.2이하로 될때까지 계속 반복해서 나아감
        while (true)
        {
            transform.position += dir * Time.deltaTime * (dist / moveSpeed);

            for (int i = 0; i < colObjs.Count; i++)
            {
                colObjs[i].position += dir * Time.deltaTime * (dist / moveSpeed);
            }

            if (Vector3.Distance(transform.position, globalDestination[curdest]) <= 0.05f)
            {
                transform.position = globalDestination[curdest];
                break;
            }

            yield return null;
        }

        if (curdest == 0)
        {
            if(repeat)
            {
                curdest++;
                StartCoroutine(NextDestination(onceRepeat ? false : true));
            }
        }
        else
        {
            curdest--;
            StartCoroutine(PrevDestination(repeat));
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.TryGetComponent<ObjectA>(out ObjectA objA) || collision.transform.CompareTag("PLAYER_BASE"))
        {
            foreach (Transform t in colObjs)
            {
                if (t == collision.transform)
                {
                    return;
                }
            }

            colObjs.Add(collision.transform);
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.transform.TryGetComponent<ObjectA>(out ObjectA objA) || collision.transform.CompareTag("PLAYER_BASE"))
        {
            colObjs.Remove(collision.transform);
        }
    }
}
