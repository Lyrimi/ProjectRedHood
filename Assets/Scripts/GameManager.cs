using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public Animator Transitonanimator;
    
    private void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
    }
    // Start is called before the first frame update
    public void nextScene(string SceneName)
    {
        StartCoroutine(LoadScene(SceneName));
    }

   IEnumerator LoadScene(string SceneName) 
    {
        Transitonanimator.SetTrigger("End");
        yield return new WaitForSecondsRealtime(2);
        SceneManager.LoadSceneAsync(SceneName);
        Transitonanimator.SetTrigger("Start");
        Time.timeScale = 1.0f;
        yield return new WaitForSecondsRealtime(2);

    }
}
