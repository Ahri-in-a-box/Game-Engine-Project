using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserInterface : MonoBehaviourSingleton<UserInterface>{

    [SerializeField]
    Player m_Player = null;

    [Header("UI")]
    [SerializeField]
    GameObject m_GameOver = null;
    [SerializeField]
    GameObject m_Restart = null;
    [SerializeField]
    UnityEngine.UI.Text m_ScoreObject = null;
    [SerializeField]
    UnityEngine.UI.Slider m_HPBar = null;
    [SerializeField]
    UnityEngine.UI.Slider m_ShieldBar = null;
    [SerializeField]
    GameObject m_ScoreBoard = null;

    [Header("Pause Menu")]
    [SerializeField]
    GameObject m_PauseMenu = null;

    new private void Awake(){
        base.Awake();
        if (!m_GameOver)
            Debug.LogError("No GameOver for UserInterface");
        if (!m_ScoreObject)
            Debug.LogError("No Score Object for UserInterface");
        if (!m_HPBar)
            Debug.LogError("No HP Bar for UserInterface");
        if (!m_ShieldBar)
            Debug.LogError("No Shield Bar for UserInterface");

        if (m_Restart){
            m_Restart.GetComponent<UnityEngine.UI.Button>().onClick.AddListener(() => GameManager.Instance.ResetLevel());
        }
        else{
            Debug.LogError("No Restart Button for UserInterface");
        }

        if(m_Player){
            m_Player.OnHPChange += HPBarManager;
            m_Player.OnScoreChange += ScoreManager;
        }else{
            Debug.LogError("No Player for UserInterface");
        }

        m_HPBar.minValue = 0;
        m_HPBar.maxValue = m_Player.MaxHP;
    }

    public void TogglePausePanel(bool pause){
        if (m_PauseMenu)
            m_PauseMenu.SetActive(pause);
    }

    private int addInPrefs(uint score){
        bool placed = false;
        int i;
        for (i = 1; i <= 10 && !placed; i++)
            if(!PlayerPrefs.HasKey(i.ToString()) || PlayerPrefs.GetInt(i.ToString()) < score)
                placed = true;

        int myPlace = i - 1;

        if(!placed)
            return 0;


        int tmp = int.Parse(score.ToString());
        bool finished = false;
        for (i--; i <= 10 && !finished; i++)
            if(PlayerPrefs.HasKey(i.ToString())){
                int val = PlayerPrefs.GetInt(i.ToString());
                PlayerPrefs.SetInt(i.ToString(), tmp);
                tmp = val;
            }
            else
                finished = true;

        if(finished)
            PlayerPrefs.SetInt((--i).ToString(), tmp);

        PlayerPrefs.Save();

        return myPlace;
    }

    private void HPBarManager(Player player){
        uint hp = player.m_ActualHealth;
        uint maxHp = player.MaxHP;
        uint shield = player.shield;

        m_HPBar.maxValue = maxHp;
        m_HPBar.value = hp;
        m_ShieldBar.maxValue = maxHp == shield ? shield + 1 : shield;
        m_ShieldBar.value = shield;

        if (hp == 0){
            if(m_ScoreBoard){
                int myPlace = addInPrefs(player.score);

                m_ScoreBoard.SetActive(true);
                UnityEngine.UI.Text[] tab = m_ScoreBoard.GetComponentsInChildren<UnityEngine.UI.Text>();
                UnityEngine.UI.Text top5 = null;
                UnityEngine.UI.Text top10 = null;

                for (int i = 0; i < tab.Length && !(top5 && top10); i++)
                    switch (tab[i].name){
                        case "Top5":
                            top5 = tab[i];
                            break;
                        case "Top5-10":
                            top10 = tab[i];
                            break;
                        default:
                            break;
                    }

                for(int i = 1; i <= 5; i++)
                    if (PlayerPrefs.HasKey(i.ToString()))
                        top5.text += myPlace == i ? $"<b>{i}:\t<i>{PlayerPrefs.GetInt(i.ToString())}</i></b>\n" : $"{i}:\t{PlayerPrefs.GetInt(i.ToString())}\n";
                for (int i = 6; i <= 10; i++)
                    if (PlayerPrefs.HasKey(i.ToString()))
                        top10.text += myPlace == i ? $"<b>{(i == 10 ? i.ToString() : $"  {i}")}:\t<i>{PlayerPrefs.GetInt(i.ToString())}</i></b>\n" : $"{(i == 10 ? i.ToString() : $"  {i}")}:\t{PlayerPrefs.GetInt(i.ToString())}\n";
            }
            else{
                m_GameOver.GetComponent<UnityEngine.UI.Text>().text = $"Game Over\nScore: {player.score}";
                m_GameOver.SetActive(true);
                m_Restart.SetActive(true);
                addInPrefs(player.score);
            }
        }
    }

    private void ScoreManager(uint score){
        m_ScoreObject.text = $"Score: {score}";
    }

    // Start is called before the first frame update
    void Start(){
        m_HPBar.value = m_Player.m_ActualHealth;
    }

    // Update is called once per frame
    void Update(){

    }

    private void OnDestroy(){
        m_Player.OnHPChange -= HPBarManager;
        m_Player.OnScoreChange -= ScoreManager;
    }
}
