using System.Collections;
using System.Collections.Generic;
using UnityEngine;

enum Phase{
    HMOVE,
    ROTATION,
    VMOVE,
    PHASE2,
}

public class Serpent : Entity{

    private Camera m_MainCamera = null;
    private float m_Speed = 100f;

    private Phase m_Phase = Phase.HMOVE;
    private bool m_Direction = true;
    private float m_StartVertical = 0f;

    new void Awake(){
        base.Awake();
        m_MainCamera = Camera.main;
        if (!m_MainCamera)
            Debug.LogError("No Camera for Enemy");
    }

    // Start is called before the first frame update
    void Start(){

    }

    Vector3 Phase1(Vector3 screen_position){
        switch(m_Phase){
            case Phase.HMOVE:
                screen_position.x += m_Speed * Time.deltaTime * (m_Direction ? 1 : -1);

                if((screen_position.x >= 0.95f * Screen.width && m_Direction) || (screen_position.x <= 0.05f * Screen.width && !m_Direction)){
                    m_StartVertical = screen_position.y;
                    m_Phase = Phase.ROTATION;
                }
                break;
            case Phase.VMOVE:
                screen_position.y -= m_Speed * Time.deltaTime;

                if(m_StartVertical - screen_position.y > 100){
                    m_StartVertical = 0;
                    m_Phase = Phase.ROTATION;
                }
                break;
            case Phase.ROTATION:
                transform.Rotate(transform.forward, (m_Direction ? -1 : 1));

                float angle = (transform.rotation.eulerAngles.z + 180f) % 360;
                if((angle < 0.5f || angle > 359.5f) && m_StartVertical != 0f)
                    m_Phase = Phase.VMOVE;

                angle = (transform.rotation.eulerAngles.z - 90f * (m_Direction ? 1 : -1)) % 360;
                if((angle < 0.5f || angle > 359.5f) && m_StartVertical == 0){
                    m_Direction = !m_Direction;
                    m_Phase = Phase.HMOVE;
                }
                break;
        }

        if (screen_position.y < Screen.height * 0.1f)
            m_Phase = Phase.PHASE2;

        return screen_position;
    }

    Vector3 Phase2(Vector3 screen_position){
        if(screen_position.y >= Screen.height * 0.94f){
            m_Phase = Phase.HMOVE;
            m_Direction = screen_position.x < Screen.width / 2f;
            transform.eulerAngles = new Vector3(0, 0, 90 * (m_Direction ? -1 : 1));
            return screen_position;
        }

        if(screen_position.x <= Screen.width * 0.05f || screen_position.x >= Screen.width * 0.95f){
            screen_position.y += m_Speed * Time.deltaTime;
            transform.eulerAngles = new Vector3(0, 0, 0);
        }else{
            if(screen_position.x < Screen.width / 2f){
                screen_position.y -= m_Speed * Time.deltaTime;
                transform.eulerAngles = new Vector3(0, 0, 90);
            }else{
                screen_position.y += m_Speed * Time.deltaTime;
                transform.eulerAngles = new Vector3(0, 0, -90);
            }
        }

        return screen_position;
    }

    protected void Move(){
        Vector3 screen_position = m_MainCamera.WorldToScreenPoint(transform.position);
        screen_position = m_Phase != Phase.PHASE2 ? Phase1(screen_position) : Phase2(screen_position);
        gameObject.transform.position = m_MainCamera.ScreenToWorldPoint(screen_position);
    }

    // Update is called once per frame
    void Update(){

    }
}
