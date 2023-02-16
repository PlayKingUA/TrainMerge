using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BootScript : MonoBehaviour
{
    void Start()
    {
        StartCoroutine(LoadLevel());
    }

    private IEnumerator LoadLevel()
    {
        yield return null;
        SceneManager.LoadScene(sceneBuildIndex: 1);
    }

}
