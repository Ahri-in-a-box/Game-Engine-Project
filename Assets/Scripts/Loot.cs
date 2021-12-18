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
    private GameObject m_Amplifier = null;

    [Header("Speed Setting")]
    [SerializeField, Range(0f, 15f)]
    private float m_Speed = 1f;

    private eLoot type = eLoot.NONE;
    public uint value { get; private set; } = 0;

    public delegate void d_OnLootTaken(Loot loot);
    static public event d_OnLootTaken OnLootTaken;

    private void Awake(){
        m_MainCamera = Camera.main;
        if (!m_MainCamera)
            Debug.LogError("No Camera for Loot");

        if (type != eLoot.NONE)
            ChangeVisuals();
    }

    public void SetType(int type){
        if(this.type == eLoot.NONE){
            this.type = (eLoot)type;
            ChangeVisuals();
        }
            
    }

    public int LootType => (int)type;

    public void SetValue(uint val){
        if(value == 0)
            value = val;
    }

    private void ManageLoot(GameObject loot){
        if(loot.GetInstanceID() == gameObject.GetInstanceID())
            Destroy(gameObject);
    }

    private void ChangeVisuals(){
        switch (type){
            case eLoot.HEALTH:
                if(m_Health){
                    GameObject child = Instantiate(m_Health);
                    child.transform.parent = transform;
                    child.transform.position = transform.position;
                    child.transform.rotation = transform.rotation;
                    child.transform.Rotate(0, 0, 45);
                    gameObject.GetComponent<MeshRenderer>().enabled = false;
                }
                break;
            case eLoot.SHIELD:
                if(m_Shield){
                    GameObject child = Instantiate(m_Shield);
                    child.transform.parent = transform;
                    child.transform.position = transform.position;
                    child.transform.rotation = transform.rotation;
                    child.transform.Rotate(0, 0, 45);
                    gameObject.GetComponent<MeshRenderer>().enabled = false;
                }
                break;
            case eLoot.GROWTH:
                if(m_Growth){
                    GameObject child = Instantiate(m_Growth);
                    child.transform.parent = transform;
                    child.transform.position = transform.position;
                    child.transform.rotation = transform.rotation;
                    child.transform.Rotate(0, 0, 45);
                    gameObject.GetComponent<MeshRenderer>().enabled = false;
                }
                break;
            case eLoot.AMPLIFIER:
                if(m_Amplifier){
                    GameObject child = Instantiate(m_Amplifier);
                    child.transform.parent = transform;
                    child.transform.position = transform.position;
                    child.transform.rotation = transform.rotation;
                    child.transform.Rotate(0, 0, 45);
                    gameObject.GetComponent<MeshRenderer>().enabled = false;
                }
                break;
            default:
                break;
        }
    }

    private void OnCollisionEnter(Collision collision){
        if (collision.gameObject.tag == "Player"){
            OnLootTaken?.Invoke(gameObject.GetComponent<Loot>());
            Destroy(gameObject);
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
