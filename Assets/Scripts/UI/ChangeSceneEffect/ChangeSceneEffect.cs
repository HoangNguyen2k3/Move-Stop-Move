using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeSceneEffect : MonoBehaviour
{
    [SerializeField] private GameObject[] randomEffect;
    [SerializeField] private float timeEffect;
    [SerializeField] private int num_scene;
    private int rand;

    private void Awake()
    {
        rand = Random.Range(0, randomEffect.Length);
        randomEffect[rand].SetActive(true);
        StartCoroutine(StartAppear());
    }
    private IEnumerator StartAppear()
    {
        yield return new WaitForSeconds(timeEffect);
        randomEffect[rand].SetActive(false);
    }
    private IEnumerator StartAppearAndChangeScene()
    {
        yield return new WaitForSeconds(timeEffect);
        //        randomEffect[rand].SetActive(false);
        SceneManager.LoadScene(num_scene);
    }
    public void ChangeScene()
    {
        if (Time.timeScale == 0f)
        {
            Time.timeScale = 1f;
        }
        rand = Random.Range(0, randomEffect.Length);
        randomEffect[rand].SetActive(true);
        StartCoroutine(StartAppearAndChangeScene());
    }

}
