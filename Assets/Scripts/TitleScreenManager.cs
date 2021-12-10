using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleScreenManager : MonoBehaviourSingleton<TitleScreenManager>{
    // Start is called before the first frame update
    void Start(){
  
    }

    public void Play(){
        UnityEngine.SceneManagement.SceneManager.LoadScene("Game");
    }

    // Update is called once per frame
    void Update(){
        if (Input.GetKeyDown(KeyCode.Return))
            Play();
    }
}
