using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreTesterSpawner : MonoBehaviour
{
    public GameObject m_TesterPrefab = null;
    
    private void Start() {

        Entity currEntity = GetComponent<Entity>();
        if (currEntity is Pot)
        {
            SpawnTesterIfEntityFixed(currEntity);
        } else if (currEntity is Crate) {
            SpawnTesterIfEntityFixed(currEntity);
        } else if (currEntity is Table) {
            SpawnTesterIfEntityFixed(currEntity);
        }
    }

    void SpawnTesterIfEntityFixed(Entity entity)
    {
        if (entity.currItemState == ItemStates.Fixed)
        {
            Instantiate(m_TesterPrefab, transform.position, Quaternion.identity);
        }
    }
}
