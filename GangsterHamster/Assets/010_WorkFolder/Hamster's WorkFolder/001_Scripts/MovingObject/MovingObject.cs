using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Objects.Interactable;

// Stop ???
// Start ???

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
        // ?????? ????? ????? ????? ????
        localDestinationList.Add(Vector3.zero);

        for (int i = localDestinationList.Count - 1; i > 0; i--)
        {
            Vector3 temp = localDestinationList[i];
            localDestinationList[i] = localDestinationList[i - 1];
            localDestinationList[i - 1] = temp;
        }

        // local?? global?? ???? ???
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
            Logger.Log("MovingObject?? ???????? ???????!!", LogLevel.Fatal);
        }

        if(playOnAwake)
        {
            StartCoroutine(NextDestination());
        }
    }

    /// <summary>
    /// ??? ?????? ??????? ???
    /// </summary>
    public void StartRepeatMove()
    {
        StopAllCoroutines();
        StartCoroutine(NextDestination(true));
    }
    /// <summary>
    /// ????? ?????? ????? ???
    /// </summary>
    public void StartOnceRepeatMove()
    {
        StopAllCoroutines();
        StartCoroutine(NextDestination(false, true));
    }
    /// <summary>
    /// ????? ???? ??? ????? ???
    /// </summary>
    /// <param name="dir"> ???? - true?? ?????? false?? ??? </param>
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
    /// ????? ???
    /// </summary>
    public void Stop()
    {
        StopAllCoroutines();
    }
    /// <summary>
    /// ??? ????? ??????? ??? ???
    /// </summary>
    public void Comeback()
    {
        StopAllCoroutines();
        PrevDestination(false);
    }

    /// <summary>
    /// ???? Destination???? ?????? ????
    /// Destination?? ??????? ???? ???????? ????
    /// ????? ???? ???
    /// </summary>
    /// <param name="repeat"> True?? ??? ??? ?????? </param>
    /// <param name="onceRepeat"> True?? ??? ????? ?????? </param>
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

            // ???????? ????? 0.2????? ??????? ??? ?????? ?????
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
    /// ???? Destination???? ?????? ????
    /// Destination?? ??????? ?? ?????? ???????? ????
    /// ????? ???? ???? ???
    /// </summary>
    /// <param name="repeat"> True?? ??? ??? ??? </param>
    /// <param name="onceRepeat"> True?? ??? ????? ??? </param>
    /// <returns></returns>
    IEnumerator PrevDestination(bool repeat = true, bool onceRepeat = false)
    {
        Vector3 dir = (globalDestination[curdest] - transform.position).normalized;
        float dist = Vector3.Distance(transform.position, globalDestination[curdest]);

        // ???????? ????? 0.2????? ??????? ??? ?????? ?????
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
