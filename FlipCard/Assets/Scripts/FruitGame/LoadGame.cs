using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

using UnitySceneManager = UnityEngine.SceneManagement.SceneManager;
public class LoadGame : MonoBehaviour
{
    public UIGameInfo UIGameInfo;
    // Start is called before the first frame update
    void Start()
    {
        UIGameInfo.ShowQuickStart("Welcom To The Game");
        StartCoroutine(LoadScene());
        


    }

    IEnumerator LoadScene()
    {
        yield return new WaitForSeconds(3.0f);
        AsyncOperation asyncLoad = UnitySceneManager.LoadSceneAsync("FruitGame");

        while (!asyncLoad.isDone)
        {
            yield return null;
        }
    }
}
