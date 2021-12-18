using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

enum Wave{
    H_LINE,
    V_LINE,
    HALF_SQUARE,
    HALF_SQ_ARE_REV,
    SQUARE,
    V,
    W,
    X,
    BOSS
};

public class EnemiesManager : MonoBehaviour{

    private const uint NB_POSS_WAVE = 8; //
    private const uint NB_POSS_LOOT = 4; //+1 with NONE
    private const uint NB_WAVE_TO_BOSS_SPAWN = 7; //A boss every 8 waves

    [Header("Spawn Settings")]
    [SerializeField, Range(0.2f, 180f)]
    private float m_Spawn_Delay = 0.2f;
    /*[SerializeField, Range(0f, 1f)]
    private float m_Spawn_Probability = 0.3f;*/

    /*[Header("Mouvements Settings")]
    [Header("Dash Settings")]
    [SerializeField, Range(0f, 10f)]
    private float m_DashProbability = 2f;
    [SerializeField, Range(0f, 1f)]
    private float m_DashPower = 0.7f;
    [SerializeField, Range(2f, 8f)]
    private float m_DashJitterRange = 5f;

    [Header("Mouvements Restrictions Settings")]
    [SerializeField, Range(0f, 1f)]
    private float m_ScreenUsage = 0.8f;
    [SerializeField, Range(0f, 8f)]
    private float m_JitterRange = 5f;

    private float m_Offset = 0;*/
    private Camera m_MainCamera = null;

    [Header("Enemy Settings")]
    [SerializeField]
    private GameObject m_EnemyObject = null;
    [SerializeField]
    private GameObject m_BossSperpentHead = null;
    [SerializeField]
    private GameObject m_Boss2Object = null;

    [Header("Loot Settings")]
    [SerializeField, Range(0f, 1f)]
    private float m_LootSpawnProbability = 0.1f;
    [SerializeField]
    private GameObject m_LootObject = null;

    private List<GameObject> m_Children = new List<GameObject>();
    private bool m_Spawned = false;
    private uint m_WavesBeforeBoss = NB_WAVE_TO_BOSS_SPAWN;

    void Awake(){
        m_MainCamera = Camera.main;

        if(!m_EnemyObject)
            Debug.LogError("No Object to spawn for EnemiesManager");

        TimeSpan time = DateTime.Now.Subtract(new DateTime(1970, 1, 1));
        UnityEngine.Random.InitState((int)time.TotalMilliseconds);
    }

    public void Died(GameObject enemy, GameObject destroyer){
        m_Children.Remove(enemy);
        if (enemy.GetComponent<Enemy>())
            enemy.GetComponent<Enemy>().OnDeath -= Died;

        if(destroyer.tag == "Bullet"){
            if(UnityEngine.Random.Range(0,1) < m_LootSpawnProbability && m_LootObject){
                GameObject child = Instantiate(m_LootObject);
                Loot comp = child.GetComponent<Loot>();
                switch ((eLoot)(Mathf.RoundToInt(UnityEngine.Random.Range(0, NB_POSS_LOOT)) + 1)){
                    case eLoot.HEALTH:
                        child.name = "Health Kit";
                        comp.SetType((int)eLoot.HEALTH);
                        comp.SetValue(1);
                        break;
                    case eLoot.SHIELD:
                        child.name = "Shield";
                        comp.SetType((int)eLoot.SHIELD);
                        comp.SetValue(5);
                        break;
                    case eLoot.GROWTH:
                        child.name = "HP Growth";
                        comp.SetType((int)eLoot.GROWTH);
                        comp.SetValue(1);
                        break;
                    case eLoot.AMPLIFIER:
                        child.name = "Amplifier";
                        comp.SetType((int)eLoot.AMPLIFIER);
                        comp.SetValue(1);
                        break;
                }
            }
        }
    }

    private void SpawnBase(){
        int i;
        int num = (int)UnityEngine.Random.Range(0, NB_POSS_WAVE - 0.001f);
        switch ((Wave)num){
            case Wave.H_LINE:
                Vector3 screenPosition = m_MainCamera.WorldToScreenPoint(transform.position);
                screenPosition.x = Screen.width / 2;

                for (i = 0; i < 5; i++){
                    m_Children.Add(Instantiate(m_EnemyObject));
                    m_Children[i].GetComponent<Enemy>().OnDeath += Died;
                }

                float pos = Mathf.Cos(Time.time);
                if (pos < -0.3){
                    //Spawn left
                    screenPosition.x = Screen.width * 0.25f;
                }else{
                    if (pos > 0.3){
                        //Spawn right
                        screenPosition.x = Screen.width * 0.75f;
                    }
                }

                Vector3 worldPosition = m_MainCamera.ScreenToWorldPoint(screenPosition);

                for (i = 0; i < 5; i++){
                    m_Children[i].transform.position = worldPosition;
                    m_Children[i].transform.position += transform.right * (2 - i) * 1.25f;
                }
                break;
            case Wave.V_LINE:
                screenPosition = m_MainCamera.WorldToScreenPoint(transform.position);
                
                for (i = 0; i < 5; i++){
                    m_Children.Add(Instantiate(m_EnemyObject));
                    m_Children[i].GetComponent<Enemy>().OnDeath += Died;
                }

                pos = Mathf.Cos(Time.time);
                if (pos < -0.6){
                    //Spawn very left
                    screenPosition.x = Screen.width * 0.2f;
                }else{
                    if (pos < -0.2){
                        //Spawn left
                        screenPosition.x = Screen.width * 0.35f;
                    }else{
                        if(pos < 0.2){
                            //Spawn center
                            screenPosition.x = Screen.width * 0.5f;
                        }
                        else{
                            if(pos < 0.6){
                                //Spawn right
                                screenPosition.x = Screen.width * 0.65f;
                            }
                            else{
                                //Spawn very right
                                screenPosition.x = Screen.width * 0.8f;
                            }
                        }
                    }
                }

                worldPosition = m_MainCamera.ScreenToWorldPoint(screenPosition);

                for (i = 0; i < 5; i++)
                {
                    m_Children[i].transform.position = worldPosition;
                    m_Children[i].transform.position += transform.up * i * 1.25f;
                }
                break;
            case Wave.HALF_SQUARE:
                screenPosition = m_MainCamera.WorldToScreenPoint(transform.position);
                screenPosition.x = Screen.width / 2;

                for(i = 0; i < 5; i++){
                    m_Children.Add(Instantiate(m_EnemyObject));
                    m_Children[i].GetComponent<Enemy>().OnDeath += Died;
                }

                pos = Mathf.Cos(Time.time);
                if(pos < -0.3){
                    //Spawn left
                    screenPosition.x = Screen.width * 0.3f;
                }else{
                    if(pos > 0.3){
                        //Spawn right
                        screenPosition.x = Screen.width * 0.7f;
                    }
                }

                worldPosition = m_MainCamera.ScreenToWorldPoint(screenPosition);

                for(i = 0; i < 5; i++){
                    m_Children[i].transform.position = worldPosition;
                    if(i != 2)
                        m_Children[i].transform.position += transform.right * (i > 2 ? 1 : -1) * 1.25f;
                    if (i == 0 || i == 4)
                        m_Children[i].transform.position += transform.up * 1.25f;
                }
                break;
            case Wave.SQUARE:
                screenPosition = m_MainCamera.WorldToScreenPoint(transform.position);
                screenPosition.x = Screen.width / 2;

                for(i = 0; i < 8; i++){
                    m_Children.Add(Instantiate(m_EnemyObject));
                    m_Children[i].GetComponent<Enemy>().OnDeath += Died;
                }

                pos = Mathf.Cos(Time.time);
                if(pos < -0.3){
                    //Spawn left
                    screenPosition.x = Screen.width * 0.3f;
                }else{
                    if(pos > 0.3){
                        //Spawn right
                        screenPosition.x = Screen.width * 0.7f;
                    }
                }

                worldPosition = m_MainCamera.ScreenToWorldPoint(screenPosition);

                for(i = 0; i < 3; i++){
                    m_Children[i].transform.position = worldPosition;
                    m_Children[i].transform.position -= transform.right * 1.25f;
                    m_Children[i].transform.position += transform.up * i * 1.25f;
                }

                for (; i < 5; i++){
                    m_Children[i].transform.position = worldPosition;
                    m_Children[i].transform.position += transform.up * (i-3) * 2 * 1.25f;
                }

                for (; i < 8; i++){
                    m_Children[i].transform.position = worldPosition;
                    m_Children[i].transform.position += transform.right * 1.25f;
                    m_Children[i].transform.position += transform.up * (i-5) * 1.25f;
                }
                break;
            case Wave.HALF_SQ_ARE_REV:
                screenPosition = m_MainCamera.WorldToScreenPoint(transform.position);
                screenPosition.x = Screen.width / 2;

                for (i = 0; i < 5; i++){
                    m_Children.Add(Instantiate(m_EnemyObject));
                    m_Children[i].GetComponent<Enemy>().OnDeath += Died;
                }

                pos = Mathf.Cos(Time.time);
                if (pos < -0.3){
                    //Spawn left
                    screenPosition.x = Screen.width * 0.3f;
                }else{
                    if (pos > 0.3){
                        //Spawn right
                        screenPosition.x = Screen.width * 0.7f;
                    }
                }

                worldPosition = m_MainCamera.ScreenToWorldPoint(screenPosition);

                for (i = 0; i < 5; i++){
                    m_Children[i].transform.position = worldPosition;
                    if (i != 2)
                        m_Children[i].transform.position += transform.right * (i > 2 ? 1 : -1) * 1.25f;
                    if (i != 0 && i != 4)
                        m_Children[i].transform.position += transform.up * 1.25f;
                }
                break;
            case Wave.V:
                screenPosition = m_MainCamera.WorldToScreenPoint(transform.position);
                screenPosition.x = Screen.width / 2;

                for(i = 0; i < 5; i++){
                    m_Children.Add(Instantiate(m_EnemyObject));
                    m_Children[i].GetComponent<Enemy>().OnDeath += Died;
                }

                pos = Mathf.Cos(Time.time);
                if(pos < -0.3){
                    //Spawn left
                    screenPosition.x = Screen.width * 0.3f;
                }else{
                    if(pos > 0.3){
                        //Spawn right
                        screenPosition.x = Screen.width * 0.7f;
                    }
                }

                worldPosition = m_MainCamera.ScreenToWorldPoint(screenPosition);

                for (i = 0; i < 5; i++){
                    m_Children[i].transform.position = worldPosition;
                    m_Children[i].transform.position += transform.right * (i - 2) * 0.79f;
                    m_Children[i].transform.position += transform.up * (i > 2 ? 1 : -1) * (i - 2) * 0.79f;
                }
                break;
            case Wave.W:
                screenPosition = m_MainCamera.WorldToScreenPoint(transform.position);
                screenPosition.x = Screen.width / 2;

                for (i = 0; i < 5; i++){
                    m_Children.Add(Instantiate(m_EnemyObject));
                    m_Children[i].GetComponent<Enemy>().OnDeath += Died;
                }

                pos = Mathf.Cos(Time.time);
                if (pos < -0.3){
                    //Spawn left
                    screenPosition.x = Screen.width * 0.3f;
                }else{
                    if (pos > 0.3){
                        //Spawn right
                        screenPosition.x = Screen.width * 0.7f;
                    }
                }

                worldPosition = m_MainCamera.ScreenToWorldPoint(screenPosition);

                for (i = 0; i < 5; i++){
                    m_Children[i].transform.position = worldPosition;
                    m_Children[i].transform.position += transform.right * (i - 2) * 0.79f;
                    m_Children[i].transform.position += transform.up * ((i + 1) % 2) * 0.79f;
                }
                break;
            case Wave.X:
                screenPosition = m_MainCamera.WorldToScreenPoint(transform.position);
                screenPosition.x = Screen.width / 2;

                for (i = 0; i < 9; i++){
                    m_Children.Add(Instantiate(m_EnemyObject));
                    m_Children[i].GetComponent<Enemy>().OnDeath += Died;
                }

                pos = Mathf.Cos(Time.time);
                if (pos < -0.3){
                    //Spawn left
                    screenPosition.x = Screen.width * 0.3f;
                }else{
                    if (pos > 0.3){
                        //Spawn right
                        screenPosition.x = Screen.width * 0.7f;
                    }
                }

                worldPosition = m_MainCamera.ScreenToWorldPoint(screenPosition);

                for(i = 0; i < 5; i++){
                    m_Children[i].transform.position = worldPosition;
                    m_Children[i].transform.position += transform.right * (i - 2) * 0.79f;
                    m_Children[i].transform.position += transform.up * (i - 2) * 0.79f;
                }

                for(; i < 7; i++){
                    m_Children[i].transform.position = worldPosition;
                    m_Children[i].transform.position -= transform.right * (i - 4) * 0.79f;
                    m_Children[i].transform.position += transform.up * (i - 4) * 0.79f;
                }

                for (; i < 9; i++){
                    m_Children[i].transform.position = worldPosition;
                    m_Children[i].transform.position += transform.right * (i - 6) * 0.79f;
                    m_Children[i].transform.position -= transform.up * (i - 6) * 0.79f;
                }
                break;
            default:
                break;
        }
    }

    private void SpawnBoss(){
        if(m_BossSperpentHead)
            m_Children.Add(Instantiate(m_BossSperpentHead));
    }

    private void Spawn(){
        /*if(!Application.isPlaying)
            return;

        if(UnityEngine.Random.Range(0f, 1f) <= m_Spawn_Probability){
            GameObject minion = Instantiate(m_EnemyObject);
            minion.transform.position = transform.position;
        }*/

        if(m_Children.Count == 0){
            if (m_Spawned)
                m_Spawned = false;
            else{
                if (m_WavesBeforeBoss > 0){
                    SpawnBase();
                    m_WavesBeforeBoss--;
                }else{
                    SpawnBoss();
                    m_WavesBeforeBoss = NB_WAVE_TO_BOSS_SPAWN;
                }
                    
            }
        }
    }

    /*private void Move(){
        float rand = UnityEngine.Random.Range(0f, 1f);
        if(rand < m_DashProbability / 100f){
            Vector3 screen_position = m_MainCamera.WorldToScreenPoint(transform.position);

            float maxX = Screen.width * (1f - ((1f - m_ScreenUsage) / 2f));
            float diffR = maxX - screen_position.x;

            float minX = Screen.width * ((1f - m_ScreenUsage) / 2f);
            float diffL = minX - screen_position.x;

            float diff = diffR > -diffL ? diffR : diffL;
            float movement = diff * m_DashPower + UnityEngine.Random.Range(-m_DashJitterRange, m_DashJitterRange);

            screen_position.x += movement;
            gameObject.transform.position = m_MainCamera.ScreenToWorldPoint(screen_position);
            m_Offset = Mathf.Acos((screen_position.x - (Screen.width / 2f)) / ((Screen.width / 2f) * m_ScreenUsage)) - Time.time % (Mathf.PI * 2); // x = arccos((y - size/2) / (lim * size/2))
        }else{
            float jitter = UnityEngine.Random.Range(-m_JitterRange, m_JitterRange);
            Vector3 screen_position = m_MainCamera.WorldToScreenPoint(transform.position);
            screen_position.x = (Screen.width / 2f) * Mathf.Cos(Time.time + m_Offset) * m_ScreenUsage + (Screen.width / 2f) + jitter; // y = size/2 * cos(x) * lim + size/2
            gameObject.transform.position = m_MainCamera.ScreenToWorldPoint(screen_position);
        }
    }*/

    // Start is called before the first frame update
    void Start(){
        InvokeRepeating("Spawn", 1f, m_Spawn_Delay);
        //InvokeRepeating("Move", 0f, 0.175f);
    }

    // Update is called once per frame
    void Update(){

    }
}
