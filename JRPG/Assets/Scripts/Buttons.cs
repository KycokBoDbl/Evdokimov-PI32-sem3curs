using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Buttons : MonoBehaviour
{ 
    public void OnAttackButton()
    {
        BattleSystem.Instance.PlayerAttack();
    }
    public void OnUseItemButton()
    {
        BattleSystem.Instance.UseItem();
    }
    public void OnGuardButton()
    {
        BattleSystem.Instance.PlayerGuard();
    }
}
