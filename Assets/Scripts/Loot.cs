using System.Collections;
using System.Collections.Generic;
using UnityEngine;

enum eLoot{
    NONE,
    HEALTH,
    SHIELD,
    GROWTH,
    AMPLIFIER
};

public class Loot : MonoBehaviour{

    private Camera m_MainCamera = null;

    [Header("Loot Settings")]
    [SerializeField]
    private GameObject m_Health = null;
    [SerializeField]
    private GameObject m_Shield = null;
    [SerializeField]
    private GameObject m_Growth = null;
    [SerializeField]
    private GameObject m_Aplifier = null;

    [Header("Speed Setting")]
    [SerializeField, Range(0f, 15f)]
    private float m_Speed = 1f;

    private eLoot type = eLoot.NONE;
    public uint value { get; private set; } = 0;

    public void SetType(int type){
        if(this.type == eLoot.NONE)
            this.type = (eLoot)type;
    }

    public int LootType => (int)type;

    public void SetValue(uint val){
        if(value == 0)
            value = val;
    }

    private void OnCollisionEnter(Collision collision){
        switch(collision.gameObject.tag){
            case "Player":
                Destroy(gameObject);
                break;
            default:
                break;
        }
    }

    void Move(){
        transform.position -= transform.up * Time.deltaTime * m_Speed;

        Vector3 screen_position = m_MainCamera.WorldToScreenPoint(transform.position);
        if (screen_position.y < 0)
            Destroy(gameObject);
    }

    // Start is called before the first frame update
    void Start(){
        
    }

    // Update is called once per frame
    void Update(){
        Move();
    }
}
