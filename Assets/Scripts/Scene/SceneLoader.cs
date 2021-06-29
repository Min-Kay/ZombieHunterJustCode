using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class SceneLoader : MonoBehaviour
{
    [Header("Change Scene")]
    [SerializeField]
    private string[] scene;
    private int index = 1;
    private float sceneDelay = 3.0f;
    private WaitForSeconds sceneWs;
    public Camera cam;

    private static SceneLoader instance = null;
    public static SceneLoader Instance
    {
        get
        {
            if (instance == null)
                return null;

            else return instance;
        }
    }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
            Destroy(this.gameObject);
        SceneManager.sceneLoaded += SetActiveScene;
        sceneWs = new WaitForSeconds(sceneDelay);
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= SetActiveScene;
    }

    public void SceneChange()
    {
        StartCoroutine(SceneLoading());
    }

    IEnumerator SceneLoading()
    {
        cam.enabled = true;
        yield return StartCoroutine(UnloadCurrentScene());
        yield return sceneWs;
        yield return StartCoroutine(LoadNewScene());
        cam.enabled = false;
        yield return sceneWs;
    }

    IEnumerator UnloadCurrentScene()
    {
        AsyncOperation unload = SceneManager.UnloadSceneAsync(SceneManager.GetActiveScene());
        while (!unload.isDone)
            yield return null;
    }

    IEnumerator LoadNewScene()
    {
        AsyncOperation load = SceneManager.LoadSceneAsync(scene[index], LoadSceneMode.Additive);
        while (!load.isDone)
            yield return null;
        index++;
    }

    private void SetActiveScene(Scene scene, LoadSceneMode mode)
    {
        SceneManager.SetActiveScene(scene);
    }

    public void ExitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
        Application.Quit();
    }
}
