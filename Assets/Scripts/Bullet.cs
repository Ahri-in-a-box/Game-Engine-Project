using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour{

    private Camera m_MainCamera = null;

    [SerializeField, Range(0f, 50f)]
    private float m_Speed = 2.5f;

    public delegate void OnHitAction(uint hp);
    public static event OnHitAction OnHit;

    void Awake(){
        //Verifier si la caméra est bien set up
        m_MainCamera = Camera.main;
        if(!m_MainCamera)
            Debug.LogError("No Camera for Bullet");
    }

    private void OnCollisionEnter(Collision collision){
        if(collision.gameObject.tag == "Enemy"){
            OnHit?.Invoke(collision.gameObject.GetComponent<Entity>().m_ActualHealth);
            Destroy(gameObject);
        }
    }

    // Start is called before the first frame update
    void Start(){
        
    }

    void Move(){
        transform.position += transform.up * Time.deltaTime * m_Speed;

        Vector3 screen_position = m_MainCamera.WorldToScreenPoint(transform.position);
        if (screen_position.y > Screen.height)
            Destroy(gameObject);
    }

    // Update is called once per frame
    void Update(){
        Move();
    }
}
