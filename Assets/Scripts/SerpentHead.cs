using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SerpentHead : Serpent{

    [SerializeField]
    private GameObject m_SerpentTail = null;
    [SerializeField]
    private Mesh m_LastSerpentTailModel = null;

    private List<GameObject> m_Children = new List<GameObject>();
    public delegate void d_OnDeath(GameObject enemy, GameObject destroyer);
    public event d_OnDeath OnDeath;

    private float m_DistanceMoved = 0f;
    private uint m_TailLength = 9;

    private void OnTailHit(GameObject opponent){
        m_ActualHealth--;

        if (m_ActualHealth <= 0){
            OnDeath?.Invoke(gameObject, opponent);
            Destroy(gameObject);
        }
    }

    private void OnCollisionEnter(Collision collision){
        switch(collision.gameObject.tag){
            case "Bullet":
                m_ActualHealth -= 3;
                break;
            default:
                break;
        }

        if(m_ActualHealth <= 0){
            OnDeath?.Invoke(gameObject, collision.gameObject);
            Destroy(gameObject);
        }
    }

    new void Move(){
        Vector3 dist = transform.position;
        base.Move();      
        dist = transform.position - dist;
        m_DistanceMoved += dist.magnitude;
    }

    void Spawn(){
        if(m_DistanceMoved * 1.5 > m_Children.Count + 1 && m_Children.Count < m_TailLength){
            GameObject child = Instantiate(m_SerpentTail);
            child.GetComponent<SerpentTail>().OnHit += OnTailHit;
            if(m_Children.Count == 8 && m_LastSerpentTailModel)
                child.GetComponent<MeshFilter>().mesh = m_LastSerpentTailModel;
            m_Children.Add(child);
        }
    }

    // Update is called once per frame
    void Update(){
        Move();
        Spawn();
    }

    private void OnDestroy(){
        for(int i = 0; i < m_Children.Count; i++){
            Destroy(m_Children[i]);
        }
    }
}
