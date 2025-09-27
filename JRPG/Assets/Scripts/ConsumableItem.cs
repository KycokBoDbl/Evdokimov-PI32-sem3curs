using UnityEngine;

[CreateAssetMenu(fileName = "New Consumable", menuName = "Inventory/Consumable")]
public class ConsumableItem : Item
{
    public int hpRestore;
    public int mpRestore;
    public override void Use()
    {
        base.Use();
        ApplyEffects();
    }
    public void ApplyEffects()
    {
        if (hpRestore > 0)
            PlayerCharacter.Instance.RestoreHP(hpRestore);

        if (mpRestore > 0)
            PlayerCharacter.Instance.RestoreMP(mpRestore);
    }
}