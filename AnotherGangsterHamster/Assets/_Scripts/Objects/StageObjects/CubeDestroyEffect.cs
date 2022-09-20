using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeDestroyEffect : MonoBehaviour
{
    public float burnTime = 1.5f;
    public float effectSpeed = 0.54f;
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

            StartCoroutine(BurnEffect(effectMaterials[i]));
        }
    }

    IEnumerator BurnEffect(Material mat)
    {
        float timer = burnTime;

        while(timer > 0)
        {
            timer -= Time.deltaTime * effectSpeed;

            mat.SetFloat("_progress", timer / burnTime);

            yield return null;
        }
    }

    public void EffectEnd()
    {
        for (int i = 0; i < havingMaterialObjs.Count; i++)
        {
            MeshRenderer meshRenderer = havingMaterialObjs[i].GetComponent<MeshRenderer>();

            meshRenderer.material = defaultMaterials[i];
            effectMaterials[i].SetFloat("_progress", 1);

            StartCoroutine(BurnEffect(effectMaterials[i]));
        }
    }
}
