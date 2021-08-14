using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.UI;

public class CloudWall : MonoBehaviour
{
    [SerializeField] private GameObject cloudWall;
    [SerializeField] private GameObject fadeImage;
    [SerializeField] private float timeUntilEnd;
    private bool executed;
    private float maxDisplayHeightAtGameplay;
    private string nextScene;
    // Start is called before the first frame update
    void Start()
    {
        maxDisplayHeightAtGameplay = 2.0f * (Mathf.Abs(Camera.main.transform.position.y)) * Mathf.Tan(Camera.main.fieldOfView * 0.5f * Mathf.Deg2Rad);
        Instantiate(cloudWall, new Vector3(0,5,0), Quaternion.Euler(0, Random.Range(0, 180), 0));
        AviationEventManagerGui.Instance.onWin += won;
        AviationEventManagerGui.Instance.onGameOver += lost;
        fadeImage.GetComponent<Image>().color = new Color(0, 0, 0, 255);
        StartCoroutine(startFade(true));
    }

    private void lost()
    {
        nextScene = "GameOverScreen";
        endScene();
    }
    private void won()
    {
        nextScene = "WinningScreen";
        endScene();
    }

    private void endScene()
    {
        if (!executed)
        {
            executed = true;
            Vector3 pos = new Vector3(0, 5, maxDisplayHeightAtGameplay / 2 + cloudWall.GetComponent<Renderer>().bounds.size.x/1.75f);
            Instantiate(cloudWall, pos, Quaternion.Euler(0, Random.Range(0, 180), 0));
            fadeImage.SetActive(true);
            StartCoroutine(startFade(false));
        }
    }

    IEnumerator startFade(bool fadeAway)
    {
        if (fadeAway)
        {
            fadeImage.SetActive(true);
            for (float i = 1; i >= 0; i -= Time.deltaTime)
            {
                fadeImage.GetComponent<Image>().color = new Color(0, 0, 0, i);
                yield return null;
            }
            fadeImage.SetActive(false);
        }
        else
        {
            yield return new WaitForSeconds(timeUntilEnd);
            fadeImage.SetActive(true);
            for (float i = 0; i <= 1; i += Time.deltaTime)
            {
                fadeImage.GetComponent<Image>().color = new Color(0, 0, 0, i);
                yield return null;
            }
            SceneManager.LoadScene(nextScene);
        }
    }
}
