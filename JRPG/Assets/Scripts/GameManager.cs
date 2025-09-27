using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public GameObject[] EnemiesPrefabs;
    public Inventory Inventory { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);

            InitializeManagers();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void InitializeManagers()
    {
        Inventory = GetComponent<Inventory>();
    }

    public PlayerCharacter GetPlayer()
    {
        return PlayerCharacter.Instance;
    }
}