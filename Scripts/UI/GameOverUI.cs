using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameOverUI : MonoBehaviour
{
    public static GameOverUI Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
        
        transform.Find("retry button").GetComponent<Button>().onClick.AddListener(() =>
        {
            GameSceneManager.Load(GameSceneManager.Scene.Game);
        });
        
        transform.Find("main menu button").GetComponent<Button>().onClick.AddListener(() =>
        {
            GameSceneManager.Load(GameSceneManager.Scene.MainMenu);
        });
        
        Hide();
    }

    public void Show()
    {
        gameObject.SetActive(true);
        
        transform.Find("waves survived text").GetComponent<TextMeshProUGUI>().SetText($"You Survived {EnemyWaveManager.Instance.GetWaveNumber()} Waves!");
    }
    
    private void Hide()
    {
        gameObject.SetActive(false);
    }
}
