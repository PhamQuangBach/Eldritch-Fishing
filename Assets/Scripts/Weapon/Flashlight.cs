using UnityEngine;

public class Flashlight : BaseWeapon
{
    [SerializeField]
    private LightSource lightSource;
    
    [SerializeField]
    private AudioSource lanternON;

    [SerializeField]
    private float inactiveRadius;

    [SerializeField]
    private float activeRadius;
    
    private float lightTarget;


    public override void OnPrimaryAttack() { }

    public override void OnEquip() { 
        lightSource.SetLightLevel(activeRadius);
        lanternON.Play();
    }
    public override void OnUnequip() { 
        lightSource.SetLightLevel(inactiveRadius);
    }
    public override void OnSecondaryAttack() { }

}