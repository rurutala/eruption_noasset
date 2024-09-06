using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LoadScene : MonoBehaviour
{
    [SerializeField] private GameObject _loadingUI;
    [SerializeField] private GameObject _titleUI;
    [SerializeField] private Slider _slider;

    [SerializeField] private string LoadSceneName;
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            LoadNextScene();
        }
    }

    public void LoadNextScene()
    {
        _loadingUI.SetActive(true);
        _titleUI.SetActive(false);
        StartCoroutine(LoadSceneon());
    }

    IEnumerator LoadSceneon()
    {
        AsyncOperation async = SceneManager.LoadSceneAsync(LoadSceneName);
        while (!async.isDone)
        {
            _slider.value = async.progress;
            yield return null;
        }
    }
}