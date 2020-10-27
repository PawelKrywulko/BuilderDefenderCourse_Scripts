using UnityEngine;

public class GameAssets : MonoBehaviour
{
    private static GameAssets _instance;
    public static GameAssets Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = Resources.Load<GameAssets>("Game Assets");
            }

            return _instance;
        }
    }
    
    [Header("Enemy")]
    public Transform pfEnemy;
    public Transform pfEnemyDieParticles;
    
    [Header("Building")]
    public Transform pfBuildingConstruction;
    public Transform pfBuildingPlacedParticles;
    public Transform pfBuildingDestroyedParticles;
    
    [Header("Others")]
    public Transform pfArrowProjectile;
}
