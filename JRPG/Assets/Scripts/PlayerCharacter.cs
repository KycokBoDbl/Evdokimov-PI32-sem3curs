using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerCharacter : Unit
{
    public static PlayerCharacter Instance { get; private set; }

    [Header("Base Stats")]
    public string characterName = "Hero";

    [Header("Equipment Bonuses")]
    public int equipmentAttackBonus = 0;
    public int equipmentDefenseBonus = 0;

    public int TotalAttack => damage + equipmentAttackBonus;
    public int TotalDefense => defense + equipmentDefenseBonus;

    public Vector2 WorldPos;
    public List<int> enemiesKilled;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
        level = 1;
        currentHP = 100;
        maxHP = 100;
        currentMP = 50;
        maxMP = 50;
        damage = 15;
        defense = 10;
        speed = 12;
        unitName = characterName;
        WorldPos = transform.position;
    }

public new bool TakeDamage(int damage)
    {
        int actualDamage = Mathf.Max(1, damage - TotalDefense);
        return base.TakeDamage(actualDamage);
    }


    public void LevelUp()
    {
        level++;
        maxHP += 20;
        maxMP += 10;
        damage += 5;
        defense += 3;
        speed += 2;

        currentHP = maxHP;
        currentMP = maxMP;

        Debug.Log($"{characterName} достиг {level} уровня!");
    }

    public void ApplyEquipmentBonuses(EquipmentItem equipment)
    {
        equipmentAttackBonus += equipment.attackBonus;
        equipmentDefenseBonus += equipment.defenseBonus;
    }

    public void RemoveEquipmentBonuses(EquipmentItem equipment)
    {
        equipmentAttackBonus -= equipment.attackBonus;
        equipmentDefenseBonus -= equipment.defenseBonus;
    }

}