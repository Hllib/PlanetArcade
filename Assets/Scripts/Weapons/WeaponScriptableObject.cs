using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Weapon", menuName = "ScriptableObjects/Weapon")]
public class WeaponScriptableObject : ScriptableObject
{
    public string title;
    public Sprite sprite;
    public int damage;

    public void Shoot()
    {
        RaycastHit2D hitInfo = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
        AudioManager.Instance.PlayOneShot(FMODEvents.Instance.pistolFire, Vector3.zero);

        IDamageable hit = hitInfo.transform.gameObject.GetComponent<IDamageable>();

        if (hit != null)
        {
            hit.Damage(damage);
        }

    }
}
