using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class OptionsUI : MonoBehaviour
{
    [SerializeField] private SoundManager soundManager;
    [SerializeField] private MusicManager musicManager;

    private TextMeshProUGUI _soundVolumeText;
    private TextMeshProUGUI _musicVolumeText;
    
    private void Awake()
    {
        _soundVolumeText = transform.Find("sound options/sound volume text").GetComponent<TextMeshProUGUI>();
        _musicVolumeText = transform.Find("music options/music volume text").GetComponent<TextMeshProUGUI>();
        
        transform.Find("sound options/sound increase button").GetComponent<Button>().onClick.AddListener(() =>
        {
            soundManager.IncreaseVolume();
            UpdateText();
        });
        transform.Find("sound options/sound decrease button").GetComponent<Button>().onClick.AddListener(() =>
        {
            soundManager.DecreaseVolume();
            UpdateText();
        });
        transform.Find("music options/music increase button").GetComponent<Button>().onClick.AddListener(() =>
        {
            musicManager.IncreaseVolume();
            UpdateText();
        });
        transform.Find("music options/music decrease button").GetComponent<Button>().onClick.AddListener(() =>
        {
            musicManager.DecreaseVolume();
            UpdateText();
        });
        transform.Find("main menu button").GetComponent<Button>().onClick.AddListener(() =>
        {
            Time.timeScale = 1f;
            GameSceneManager.Load(GameSceneManager.Scene.MainMenu);
        });
    }

    private void Start()
    {
        gameObject.SetActive(false);
        UpdateText();
        
        var edgeScrolling = transform.Find("edge scrolling toggle").GetComponent<Toggle>();
        edgeScrolling.onValueChanged.AddListener(isSet =>
        {
            CameraHandler.Instance.SetEdgeScrolling(isSet);
        });
        edgeScrolling.SetIsOnWithoutNotify(CameraHandler.Instance.GetEdgeScrolling());
    }

    private void UpdateText()
    {
        _soundVolumeText.SetText(Mathf.RoundToInt(soundManager.GetVolume() * 10).ToString());
        _musicVolumeText.SetText(Mathf.RoundToInt(musicManager.GetVolume() * 10).ToString());
    }

    public void ToggleVisible()
    {
        gameObject.SetActive(!gameObject.activeSelf);
        Time.timeScale = gameObject.activeSelf ? 0 : 1;
    }
}
