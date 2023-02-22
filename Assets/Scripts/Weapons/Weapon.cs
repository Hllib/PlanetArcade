using System.Collections;
using System.Collections.Generic;
using System.IO.Pipes;
using UnityEngine;

public abstract class Weapon : MonoBehaviour, IGatherable
{
    protected float bulletRotationZ = 0f;
    protected float bulletRotationY = 0f;

    enum FireDirection
    {
        Left,
        Right,
        Front,
        Back,
        LB,
        RB,
        RF,
        LF
    }

    enum QuaternionsZ
    {
        Front = -90,
        Back = 90,
        LB = -215,
        RB = 45,
        RF = -45,
        LF = -145
    }

    enum QuaternionsY
    {
        Left = 180,
        Right = 0
    }

    public Inventory PlayerInventory { get; set; }
    protected int inventoryType;

    public virtual void Init()
    {
        PlayerInventory = GameObject.FindGameObjectWithTag("Player").GetComponentInChildren<Inventory>();

        if (PlayerInventory == null)
        {
            Debug.LogError("PlayerInventory is NULL::Weapon.cs");
        }
    }

    public virtual void Start()
    {
        Init();
    }

    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerInventory.GiveItem(inventoryType);
            Destroy(gameObject);
        }
    }

    private void ChooseBulletRotation(int fireDirection)
    {
        switch (fireDirection)
        {
            case (int)FireDirection.Left:
                bulletRotationY = (int)QuaternionsY.Left;
                break;
            case (int)FireDirection.Right:
                bulletRotationY = (int)QuaternionsY.Right;
                break;
            case (int)FireDirection.Front:
                bulletRotationZ = (int)QuaternionsZ.Front;
                break;
            case (int)FireDirection.Back:
                bulletRotationZ = (int)QuaternionsZ.Back;
                break;
            case (int)FireDirection.LB:
                bulletRotationZ = (int)QuaternionsZ.LB;
                break;
            case (int)FireDirection.RB:
                bulletRotationZ = (int)QuaternionsZ.RB;
                break;
            case (int)FireDirection.RF:
                bulletRotationZ = (int)QuaternionsZ.RF;
                break;
            case (int)FireDirection.LF:
                bulletRotationZ = (int)QuaternionsZ.LF;
                break;
            default: break;
        }
    }

    public virtual void Fire(int fireDirection, int ammoAmount)
    {
        ChooseBulletRotation(fireDirection);
    }
}
