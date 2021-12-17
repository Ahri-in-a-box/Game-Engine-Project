using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entity : MonoBehaviour{

    public uint m_ActualHealth { get; protected set; } = 0;

    [Header("Health Setting")]
    [SerializeField, Range(1, 5000)]
    protected uint m_MaxHealth = 3;

    public uint MaxHP => m_MaxHealth;

    protected void Awake(){
        m_ActualHealth = m_MaxHealth;
    }

    // Start is called before the first frame update
    void Start(){

    }

    // Update is called once per frame
    void Update(){

    }
}
