using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SerpentTail : Serpent{

    public delegate void d_OnHit(GameObject opponent);
    public event d_OnHit OnHit;

    private void OnCollisionEnter(Collision collision){
        switch (collision.gameObject.tag){
            case "Bullet":
                OnHit?.Invoke(collision.gameObject);
                break;
            default:
                break;
        }
    }

    // Update is called once per frame
    void Update(){
        Move();
    }
}
