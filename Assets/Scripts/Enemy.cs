using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Entity{

    private Camera m_MainCamera = null;

    [Header("Speed Setting")]
    [SerializeField, Range(0f, 15f)]
    private float m_Speed = 1f;

    public delegate void d_OnDeath(GameObject enemy, GameObject destroyer);
    public event d_OnDeath OnDeath;

    new void Awake(){
        base.Awake();
        m_MainCamera = Camera.main;
        if(!m_MainCamera)
            Debug.LogError("No Camera for Enemy");
    }

    // Start is called before the first frame update
    void Start(){

    }

    private void OnCollisionEnter(Collision collision){
        switch (collision.gameObject.tag){
            case "Player":
                if(!collision.gameObject.GetComponent<Player>().IsInv)
                    m_ActualHealth = 0;
                break;
            case "Bullet":
                m_ActualHealth--;
                break;
            default:
                break;
        }

        if(m_ActualHealth == 0){
            OnDeath?.Invoke(gameObject, collision.gameObject);
            Destroy(gameObject);
        }
    }

    void Move(){
        transform.position -= transform.up * Time.deltaTime * m_Speed;

        Vector3 screen_position = m_MainCamera.WorldToScreenPoint(transform.position);
        if (screen_position.y < 0)
            Destroy(gameObject);
    }

    // Update is called once per frame
    void Update(){
        Move();
    }
}
