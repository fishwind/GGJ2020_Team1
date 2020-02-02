using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;

public class ScoreTester : MonoBehaviour
{
    private HashSet<Entity> m_EntitiesNearby = new HashSet<Entity>();
    public float m_DistanceThreshold = 2f;
    public Type m_ExpectedType = null;

    private void Start() {
        PrepareExpectedComponents();
    }

    private void PrepareExpectedComponents() {
        Collider[] cols = Physics.OverlapSphere(transform.position, 4);
        foreach(Collider col in cols) {
            Entity entity = col.GetComponentInParent<Entity>();
            if(entity)
                m_EntitiesNearby.Add(entity);
        }

        foreach(Entity e in m_EntitiesNearby)
        {
            if(e is Pot) m_ExpectedType = typeof(Pot);
            else if (e is Crate) m_ExpectedType = typeof(Crate);
            else if (e is Table) m_ExpectedType = typeof(Table);
            // else if (e is CraftMat) m_ExpectedType = typeof(CraftMat);
        }
    }
    
    private void Update() {
        m_EntitiesNearby.Clear();
        Collider[] cols = Physics.OverlapSphere(transform.position, 4);
        foreach(Collider col in cols) {
            Entity entity = col.GetComponentInParent<Entity>();
            if(entity)
                m_EntitiesNearby.Add(entity);
        }
    }


    // return 0 or 1
    public float CalculateScore() {
        foreach(Entity e in m_EntitiesNearby) {
            if(e.GetType() == m_ExpectedType && Vector3.Distance(e.transform.position, transform.position) < 2
             && e.currItemState == ItemStates.Fixed) {
                return 1;
            } 
        }
        return 0;
    }
}
