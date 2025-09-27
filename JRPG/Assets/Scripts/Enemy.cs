using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Enemy : Unit
{
    public GameObject Prefab;
    public int PrefabID;
    public int ID;
    public Item[] itemDrop;

    void Update()
    {
        if (Vector2.Distance(PlayerCharacter.Instance.transform.position, transform.position) < 2.5 && !PlayerCharacter.Instance.enemiesKilled.Contains(ID))
        {
            BattleSystem.Instance.EnemyID = PrefabID;
            Destroy(gameObject);
            PlayerCharacter.Instance.WorldPos = PlayerCharacter.Instance.transform.position;
            SceneManager.LoadScene(1);
            BattleSystem.Instance.StartBattle();
        }
        else if (PlayerCharacter.Instance.enemiesKilled.Contains(ID))
            Destroy(gameObject);
    }
}