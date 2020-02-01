using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class CraftSupply : MonoBehaviour
{
    CraftMatType craftMatType;
    public AudioClip m_SpawnCraftMatFinClip = null;
    public Transform m_SpawnPoint;
    private AudioSource m_Asource = null;

    public GameObject prefabCraftMat;
    public float spawnCrateDist;

    private GameObject spawnedCrateRef;

    private void Start()
    {
        m_Asource = GetComponent<AudioSource>();
        SpawnNewCraftMat();
    }

    private void Update()
    {
        if(spawnedCrateRef == null)
        {
            SpawnNewCraftMat();
        }
        else if(Vector3.Distance(spawnedCrateRef.transform.position, transform.position) > spawnCrateDist)
        {
            SpawnNewCraftMat();
        }
    }

    private void OnTriggerExit(Collider col)
    {
        // Check If col is craft mat
        CraftMat craftMat = col.gameObject.GetComponentInParent<CraftMat>();
        if (!craftMat || craftMat.currItemState != ItemStates.CraftMat) return;

        // Check if craft mat is Clay
        if (craftMat.currCraftMatType == CraftMatType.Clay)
        {
            SpawnNewCraftMat();
        }
    }

    void SpawnNewCraftMat()
    {
        // Instantiate Pot
        GameObject newCraftMat = Instantiate(prefabCraftMat, transform.position, transform.rotation);
        float scale = newCraftMat.transform.localScale.x;
        newCraftMat.transform.localScale = Vector3.zero;

        spawnedCrateRef = newCraftMat;

        // throw out pot todo:
        Sequence seq = DOTween.Sequence();
        seq.Append(newCraftMat.transform.DOMove(transform.position + Vector3.up + transform.forward, 0.4f));
        seq.Join(newCraftMat.transform.DOScale(Vector3.one * scale, 0.4f));
        seq.Append(newCraftMat.transform.DOMove(m_SpawnPoint.position, 0.6f));
        seq.AppendCallback(() => { newCraftMat.GetComponent<Entity>().SetMeshColliders(true); });

        m_Asource.PlayOneShot(m_SpawnCraftMatFinClip);
    }
}
