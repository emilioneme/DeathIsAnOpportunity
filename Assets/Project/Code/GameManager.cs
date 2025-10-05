 using UnityEngine;

public class GameManager : MonoBehaviour
{
    private static GameManager _instance;
    public PlayerUpgradeData upgradeData;

    public static GameManager Instance
    {
        get
        {
            if (_instance == null)
            {
                // Try to find an existing instance in the scene
                _instance = FindFirstObjectByType<GameManager>();

                if (_instance == null)
                {
                    // Create a new GameObject if none exists
                    GameObject singletonObject = new GameObject(typeof(GameManager).Name);
                    _instance = singletonObject.AddComponent<GameManager>();
                }
            }
            return _instance;
        }
    }

    private void Awake()
    {
        // Ensure only one instance exists
        if (_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (_instance != this)
        {
            Destroy(gameObject);
        }
    }
}
