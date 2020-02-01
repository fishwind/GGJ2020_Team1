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

    void SpawnNewCraftMat()
    {
        // Instantiate Pot
        GameObject newCraftMat = Instantiate(prefabCraftMat, transform.position, transform.rotation);

        // Off Collider #IMPT to avoid infinte loop
        newCraftMat.GetComponent<Entity>().SetMeshColliders(false);

        spawnedCrateRef = newCraftMat;

        ShootOutObject(newCraftMat);

        m_Asource.PlayOneShot(m_SpawnCraftMatFinClip);
    }

    private void ShootOutObject(GameObject thrownObject)
    {
        float scale = thrownObject.transform.localScale.x;
        thrownObject.transform.localScale = Vector3.zero;

        Sequence seq = DOTween.Sequence();
        seq.Append(thrownObject.transform.DOMove(transform.position + Vector3.up + transform.forward, 0.4f));
        seq.Join(thrownObject.transform.DOScale(Vector3.one * scale, 0.4f));
        seq.Append(thrownObject.transform.DOMove(m_SpawnPoint.position, 0.6f));
        seq.AppendCallback(() => { thrownObject.GetComponent<Entity>().SetMeshColliders(true); });
    }
}
