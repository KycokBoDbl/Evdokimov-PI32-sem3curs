using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using Cainos.PixelArtTopDown_Basic;

public enum BattleState { START, PLAYERTURN, ENEMYTURN, WON, LOST }

public class BattleSystem : MonoBehaviour
{
    public BattleState state;
    public int EnemyID;

    private GameObject enemyObj;
    private TMP_Text playerInfoText;
    private TMP_Text enemyInfoText;
    private TMP_Text lostText;
    private Slider playerHPSlider;
    private Slider enemyHPSlider;
    private Slider playerMPSlider;
    private Slider enemyMPSlider;
    public GameObject BattlePanel;
    private bool playerIsGuarding = false;

    private PlayerCharacter playerUnit;
    private Enemy enemyUnit;
    private bool inventoryOpen = false;
    public static BattleSystem Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void SetUI()
    {
        GameObject canvas = GameObject.Find("BattleCanvas");
        foreach (TMP_Text text in canvas.GetComponentsInChildren<TMP_Text>())
        {
            if (text.gameObject.CompareTag("PlayerUI"))
                playerInfoText = text;
            else if (text.gameObject.CompareTag("EnemyUI"))
                enemyInfoText = text;
            else if (text.gameObject.name.Equals("YouAreDead")) 
                lostText = text;
        }

        foreach (Slider slider in canvas.GetComponentsInChildren<Slider>())
        {
            if (slider.gameObject.CompareTag("PlayerUI"))
            {
                if (slider.gameObject.name.Equals("PlayerHPSlider"))
                    playerHPSlider = slider;
                else playerMPSlider = slider;
            }
            else if (slider.gameObject.CompareTag("EnemyUI"))
            {
                if (slider.gameObject.name.Equals("EnemyHPSlider"))
                    enemyHPSlider = slider;
                else enemyMPSlider = slider;

            }
        }
        lostText.gameObject.SetActive(false); 
        BattlePanel = canvas.transform.Find("BattlePanel").gameObject;
        if (state != BattleState.PLAYERTURN)
            BattlePanel.SetActive(false);
    }

    public void StartBattle()
    {
        if (SceneManager.GetActiveScene().Equals(SceneManager.GetSceneByBuildIndex(1)))
        {
            GameObject EnemyPrefab = GameManager.Instance.EnemiesPrefabs[EnemyID];
            enemyObj = Instantiate(EnemyPrefab);
            state = BattleState.START;
            SetUI();
            SetupBattle();
        }
    }
    void SetupBattle()
    {
        GameObject playerGO = PlayerCharacter.Instance.gameObject;
        playerGO.transform.position = new Vector2(-4, -4);
        playerGO.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;
        playerUnit = playerGO.GetComponent<PlayerCharacter>();
        playerGO.GetComponent<Animator>().SetInteger("Direction", 2);
        playerGO.GetComponent<Animator>().SetBool("IsMoving", false);
        playerGO.GetComponent<TopDownCharacterController>().enabled = false;
        SetPlayerHUD();

        if (enemyObj)
        {
            enemyUnit = enemyObj.GetComponent<Enemy>();
            enemyObj.GetComponent<Animator>().SetInteger("Direction", 3);
            enemyObj.transform.position = new Vector2(4, -4);
            SetEnemyHUD(enemyUnit);

            state = BattleState.PLAYERTURN;
            enemyHPSlider.gameObject.SetActive(true);
            enemyInfoText.gameObject.SetActive(true);
        }
    }

    void SetPlayerHUD()
    {
        if (playerInfoText)
        {
            Unit unit = PlayerCharacter.Instance;
            playerInfoText.SetText(unit.unitName + ": lvl " + unit.level);

            playerHPSlider.maxValue = unit.maxHP;
            playerHPSlider.value = unit.currentHP;
            playerMPSlider.maxValue = unit.maxMP;
            playerMPSlider.value = unit.currentMP;
        }
    }
    void SetEnemyHUD(Enemy enemy)
    {
        if (enemyInfoText)
        {
            Unit unit = enemy;
            enemyInfoText.SetText(unit.unitName + ": lvl " + unit.level);

            enemyHPSlider.maxValue = unit.maxHP;
            enemyHPSlider.value = unit.currentHP;
            enemyMPSlider.maxValue = unit.maxMP;
            enemyMPSlider.value = unit.currentMP;
        }
    }

    void SetSlidersValue(Unit unit, Slider hpSlider, Slider mpSlider)
    {
        hpSlider.value = unit.currentHP;
        mpSlider.value = unit.currentMP;
    }

    void PlayerTurn()
    {
        if (!inventoryOpen)
            BattlePanel.SetActive(true);
    }

    public void PlayerGuard()
    {
        playerIsGuarding = true;
        PlayerCharacter.Instance.RestoreHP(5);
        PlayerCharacter.Instance.RestoreMP(5);
        state = BattleState.ENEMYTURN;
    }

    public void PlayerAttack()
    {
        PlayerCharacter.Instance.GetComponent<Animator>().SetTrigger("Attack");
        bool isDead = enemyUnit.TakeDamage(playerUnit.damage);
        PlayerCharacter.Instance.MPLoss(5);
        SetSlidersValue(enemyUnit, enemyHPSlider, enemyMPSlider);
        if (isDead)
        {
            state = BattleState.WON;
            enemyInfoText.gameObject.SetActive(false);
            enemyHPSlider.gameObject.SetActive(false);
            playerUnit.enemiesKilled.Add(enemyUnit.ID);
            playerUnit.LevelUp();
            EndBattle();
        }
        else
        {
            state = BattleState.ENEMYTURN;
        }
        BattlePanel.SetActive(false);
    }

    public void UseItem()
    {
        BattlePanel.SetActive(false);
        InventoryUI.Canvas.GetComponentInChildren<InventoryUI>().ToggleInventory();
        inventoryOpen = true;
    }
    void EnemyTurn()
    {
        inventoryOpen = false;
        enemyObj.GetComponent<Animator>().SetTrigger("Attack");
        bool isDead = playerUnit.TakeDamage(playerIsGuarding? enemyUnit.damage / 2 : enemyUnit.damage);
        SetSlidersValue(playerUnit, playerHPSlider, enemyMPSlider);

        if (isDead)
        {
            state = BattleState.LOST;
            EndBattle();
        }
        else
        {
            state = BattleState.PLAYERTURN;
        }
    }

    void EndBattle()
    {

        if (state == BattleState.WON)
        {
            PlayerCharacter.Instance.transform.position = PlayerCharacter.Instance.WorldPos;
            PlayerCharacter.Instance.gameObject.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
            PlayerCharacter.Instance.GetComponent<TopDownCharacterController>().enabled = true;
            foreach (Item item in enemyUnit.itemDrop)
            {
                Inventory.Instance.AddItem(item);
            }
            SceneManager.LoadScene(0);
        }
        else if (state == BattleState.LOST)
        {
            lostText.gameObject.SetActive(true);
        }
    }

    private void Update()
    {
        if (SceneManager.GetActiveScene().Equals(SceneManager.GetSceneByBuildIndex(1)))
        {
            if (enemyObj)
            {
                SetUI();
                SetPlayerHUD();
                SetEnemyHUD(enemyUnit);
                if (state == BattleState.PLAYERTURN && !enemyUnit.Attack)
                {
                    PlayerTurn();
                }
                else if (state == BattleState.ENEMYTURN && !PlayerCharacter.Instance.Attack)
                {
                    EnemyTurn();
                }
                else if (state == BattleState.LOST)
                {
                    lostText.gameObject.SetActive(true);
                    if (Input.GetKeyDown(KeyCode.Escape))
                    {
                        PlayerCharacter.Instance.gameObject.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
                        PlayerCharacter.Instance.GetComponent<TopDownCharacterController>().enabled = true;
                        PlayerCharacter.Instance.FullHeal();
                        PlayerCharacter.Instance.transform.position = new Vector2(6, 2);
                        SceneManager.LoadScene(0);
                    }
                }
            }
            else StartBattle();
        }
    }
}