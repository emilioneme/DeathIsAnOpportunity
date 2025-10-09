 using UnityEngine;

public class GameManager : MonoBehaviour
{
    private static GameManager gameManager;

    [Header("Settings")]
    public float masterVolume = 0.5f;
    public float musicVolume = 0.5f;
    public float sensitivity = .5f;

    [Space]
    [Header("Currency")]
    [SerializeField]
    public int soulCount = 0;

    [Space]
    [Header("Health")]
    public int maxHealth = 100;
    public int startHealth = 100;

    [Space]
    [Header("Movement")]
    [SerializeField]
    public int walkSpeed = 1;

    [SerializeField]
    public int jumpForce = 0;

    [SerializeField]
    public int airJumps = 0;

    [SerializeField]
    public int dashCount = 0;

    [Space]
    [Header("Combat")]

    [SerializeField]
    public float fireRate = 1;

    [SerializeField]
    public int damage = 50;


    public static GameManager Instance
    {
        get
        {
            if (gameManager == null)
            {
                // Try to find an existing instance in the scene
                gameManager = FindFirstObjectByType<GameManager>();

                if (gameManager == null)
                {
                    // Create a new GameObject if none exists
                    GameObject singletonObject = new GameObject(typeof(GameManager).Name);
                    gameManager = singletonObject.AddComponent<GameManager>();
                }
            }
            return gameManager;
        }
    }

    private void Awake()
    {
        // Ensure only one instance exists
        if (gameManager == null)
        {
            gameManager = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (gameManager != this)
        {
            Destroy(gameObject);
        }
    }
}
