using UnityEngine;
using UnityEngine.UI;

public class MainMenuUI : MonoBehaviour
{
    private void Awake()
    {
        transform.Find("play button").GetComponent<Button>().onClick.AddListener(() =>
        {
            GameSceneManager.Load(GameSceneManager.Scene.Game);
        });
        transform.Find("quit button").GetComponent<Button>().onClick.AddListener(Application.Quit);
    }
}
