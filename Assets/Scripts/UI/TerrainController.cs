using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainController : MonoBehaviour
{
    public GameObject cam;
    public List<Transform> terrains;
    public List<Vector3> startPos = new List<Vector3>();

    public bool scrollingTerrain = false;
    public float multiplier = 5f;

    private void Awake()
    {
        foreach (Transform t in terrains)
        {
            startPos.Add(t.position);
        }
    }

    private void OnDestroy()
    {
        startPos.Clear();
    }

    private void Update()
    {
        if (!scrollingTerrain) return;

        for (int i = 0; i < terrains.Count; ++i)
        {
            Vector3 v = terrains[i].transform.position;
            float dist = cam.transform.position.x * (i * 0.1f);
            terrains[i].transform.position = new Vector3(startPos[i].x - dist * multiplier, v.y, v.z);
        }
    }

}
