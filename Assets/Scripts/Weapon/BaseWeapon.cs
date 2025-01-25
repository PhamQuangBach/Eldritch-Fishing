using UnityEngine;


public abstract class BaseWeapon : MonoBehaviour, IWeapon
{
    public string WeaponName;
    
    public bool IsEquipped
    {
        get => gameObject.activeSelf;
    }

    public void OnEquipInternal()
    {
        gameObject.SetActive(true);
        OnEquip();
    }

    public void OnUnequipInternal()
    {
        gameObject.SetActive(false);
        OnUnequip();
    }

    public abstract void OnEquip();
    public abstract void OnUnequip();

    public abstract void OnPrimaryAttack();
    public abstract void OnSecondaryAttack();
}