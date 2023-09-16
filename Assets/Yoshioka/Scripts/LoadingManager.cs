using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using DG.Tweening;

public class LoadingManager : SingletonMonoBehaviour<LoadingManager>
{
    public GameObject fadeCanvas;

    private GameObject _fadeCanvas;
    private Image _blackScreen;

    public void LoadScene(string sceneName, float length)
    {
        _fadeCanvas = Instantiate(fadeCanvas);
        _blackScreen = _fadeCanvas.transform.GetChild(0).GetComponent<Image>();
        DontDestroyOnLoad(_fadeCanvas);

        _blackScreen.DOFade(1,0.4f).SetEase(Ease.InQuad).OnComplete(() =>
        {
            SceneManager.LoadScene("Loading");

            _blackScreen.DOFade(0,0.4f).SetEase(Ease.InQuad).OnComplete(() =>
            {
                StartCoroutine(Loading(sceneName,length));
            });

        });
    }

    IEnumerator Loading(string sceneName, float length)
    {
        yield return new WaitForSeconds(length);

        _blackScreen.DOFade(1,0.4f).SetEase(Ease.InQuad).OnComplete(() =>
        {
            SceneManager.LoadScene(sceneName);

            _blackScreen.DOFade(0,0.4f).SetEase(Ease.InQuad).OnComplete(() =>
            {
                Destroy(_fadeCanvas);
            });

        });
    }
}
