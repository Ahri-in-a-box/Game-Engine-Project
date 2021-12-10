using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviourSingleton<GameManager>{

    public bool m_Pause_Status { get; protected set; } = true;
    private float m_TimeScale = 1f;

    public void ResetLevel(){
        UnityEngine.SceneManagement.SceneManager.LoadScene("Game");
    }

    public void ExitToMainMenu(){
        UnityEngine.SceneManagement.SceneManager.LoadScene("Menu");
    }

    public void Play(bool play){
        m_Pause_Status = !play;
        UserInterface.Instance.TogglePausePanel(!play);
        Time.timeScale = play ? m_TimeScale : 0;
    }

    // Start is called before the first frame update
    void Start(){
        Play(true);
    }

    // Update is called once per frame
    void Update(){

    }
}
