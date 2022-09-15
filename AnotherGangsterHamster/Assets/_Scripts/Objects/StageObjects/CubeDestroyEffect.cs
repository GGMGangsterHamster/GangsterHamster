using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeDestroyEffect : MonoBehaviour
{
    public float fireTime = 1.5f;
    public List<GameObject> havingMaterialObjs = new List<GameObject>();

    private List<Material> defaultMaterials = new List<Material>();
    private List<Material> effectMaterials = new List<Material>();

    private void Awake()
    {
        foreach(GameObject obj in havingMaterialObjs)
        {
            MeshRenderer meshRenderer = obj.GetComponent<MeshRenderer>();

            List<Material> mats = new List<Material>();
            meshRenderer.GetMaterials(mats);

            defaultMaterials.Add(mats[0]);
            effectMaterials.Add(mats[1]);
            mats.Remove(mats[1]);
        }
    }

    public void EffectStart()
    {
        for(int i = 0; i < havingMaterialObjs.Count; i++)
        {
            MeshRenderer meshRenderer = havingMaterialObjs[i].GetComponent<MeshRenderer>();

            meshRenderer.material = effectMaterials[i];
            effectMaterials[i].SetFloat("_progress", 1);

            StartCoroutine(FiredEffect(effectMaterials[i]));
        }
    }

    IEnumerator FiredEffect(Material mat)
    {
        float timer = fireTime;

        while(timer > 0)
        {
            timer -= Time.deltaTime;

            mat.SetFloat("_progress", timer / fireTime);

            yield return null;
        }
    }

    IEnumerator SpawnEffect(Material mat)
    {
        float timer = 0;

        while (timer <= fireTime)
        {
            timer += Time.deltaTime;

            mat.SetFloat("_progress", timer / fireTime);

            yield return null;
        }
    }

    public void EffectEnd()
    {
        for (int i = 0; i < havingMaterialObjs.Count; i++)
        {
            MeshRenderer meshRenderer = havingMaterialObjs[i].GetComponent<MeshRenderer>();

            meshRenderer.material = defaultMaterials[i];
            effectMaterials[i].SetFloat("_progress", 0);

            //StartCoroutine(SpawnEffect(effectMaterials[i]));
        }
    }
}
