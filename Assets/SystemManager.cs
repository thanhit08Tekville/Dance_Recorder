using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SystemManager : MonoBehaviour
{
    public List<GameObject> sceneManagers;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void activateSceneManager(GameObject sceneManager)
    {
        for (int i = 0; i < sceneManagers.Count ; i++)
        {
            sceneManagers[i].GetComponent<SceneManager>().ResetScene();
            sceneManagers[i].SetActive(false);
        }
        sceneManager.SetActive(true);
    }
}
