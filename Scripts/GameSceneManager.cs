using UnityEngine.SceneManagement;

public static class GameSceneManager
{
    public enum Scene
    {
        MainMenu,
        Game,
    }
    
    public static void Load(Scene scene)
    {
        SceneManager.LoadScene((int) scene);
    }
}
