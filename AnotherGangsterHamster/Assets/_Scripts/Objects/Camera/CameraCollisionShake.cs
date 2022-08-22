using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraCollisionShake : MonoBehaviour
{
    public float duration;
    public float magnitude;

    public void Shake()
    {
        Debug.Log("ºŒ¿Ã≈∑");
        StopCoroutine(Shaking());
        StartCoroutine(Shaking());
    }

    IEnumerator Shaking()
    {
        Vector3 originalPos = transform.localPosition;
        float elapsed = 0f;
        while (elapsed < duration)
        {
            float x = Random.Range(-1f, 1f) * magnitude;
            float y = Random.Range(-1f, 1f) * magnitude;

            transform.localPosition = new Vector3(x, y, originalPos.z);

            elapsed += Time.deltaTime;

            yield return null;
        }

        transform.localPosition = originalPos;
    }
}
