using UnityEngine;


public class Flashlight : BaseWeapon
{
    [SerializeField]
    private GameObject lightSource;

    public override void OnPrimaryAttack()
    {
        lightSource.SetActive(!lightSource.activeSelf);
    }

    public override void OnEquip() { }
    public override void OnUnequip() { }
    public override void OnSecondaryAttack() { }
}