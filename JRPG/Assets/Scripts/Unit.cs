using UnityEngine;

[System.Serializable]
public class Unit : MonoBehaviour
{
    [Header("Base Stats")]
    public string unitName;
    public int level;
    public int damage;
    public int maxHP;
    public int currentHP;
    public int currentMP;
    public int maxMP;
    public int defense;
    public int speed;
    public bool Attack;

    public bool TakeDamage(int dmg)
    {
        currentHP -= dmg;

        if (currentHP <= 0)
        {
            currentHP = 0;
            return true;
        }
        return false;
    }

    public bool MPLoss(int amount)
    {
        currentMP -= amount;
        if (currentMP <= 0)
        {
            currentMP = 0;
            return true;
        }
        return false;
    }

    public void RestoreHP(int amount)
    {
        currentHP = Mathf.Min(currentHP + amount, maxHP);
        Debug.Log($"{unitName} восстановил {amount} HP. Теперь HP: {currentHP}/{maxHP}");
    }

    public void RestoreMP(int amount)
    {
        currentMP = Mathf.Min(currentMP + amount, maxMP);
        Debug.Log($"{unitName} восстановил {amount} MP. Теперь MP: {currentMP}/{maxMP}");
    }
    public void FullHeal()
    {
        currentHP = maxHP;
        currentMP = maxMP;
    }
}