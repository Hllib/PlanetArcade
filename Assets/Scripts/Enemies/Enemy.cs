using Cinemachine.Utility;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public abstract class Enemy : MonoBehaviour
{
    [SerializeField]
    protected int health;
    [SerializeField]
    protected float speed;
    [SerializeField]
    protected Transform pointA, pointB;
    protected bool hasStoped;

    protected Vector3 currentTarget;
    protected Vector3 previousTarget;

    protected Animator animator;
    protected SpriteRenderer spriteRenderer;
    protected Player player;

    protected bool isInCombat;
    protected bool isDead;

    [SerializeField]
    protected Image healthBarImg;
    [SerializeField]
    protected GameObject damageReceivedTextPrefab;

    [SerializeField]
    protected GameObject lootPrefab;
    protected float tempSpeed;

    [SerializeField]
    protected Transform face;
    [SerializeField]
    protected Transform back;
    [SerializeField]
    protected Transform leftMapBorder;

    public enum LookDirection
    {
        Right,
        Left
    }
    protected int lookDirection;

    public virtual void Init()
    {
        this.animator = GetComponent<Animator>();
        this.spriteRenderer = GetComponent<SpriteRenderer>();
        this.player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
    }

    protected virtual void DropLoot()
    {
        Instantiate(lootPrefab, transform.position, Quaternion.identity);
    }

    private void Start()
    {
        Init();
    }

    protected void UpdateHealthBar(float percentage)
    {
        healthBarImg.fillAmount = percentage / 100;
    }

    protected void ShowFloatingDamage(int damage, Color color)
    {
        GameObject damageText = Instantiate(damageReceivedTextPrefab, transform.position, Quaternion.identity) as GameObject;
        damageText.GetComponent<TextMeshProUGUI>().text = damage.ToString();
        damageText.GetComponent<TextMeshProUGUI>().color = color;
        damageText.transform.SetParent(gameObject.GetComponentInChildren<Canvas>().GetComponentsInChildren<Image>()[1].transform);
    }

    public void FlipDirection()
    {
        transform.Rotate(0f, 180f, 0f);
        this.lookDirection = lookDirection == (int)LookDirection.Left ? (int)LookDirection.Right : (int)LookDirection.Left; 
    }

    public virtual void CalculateMovement()
    {
        if (transform.position == pointA.position)
        {
            currentTarget = pointB.position;
            previousTarget = pointB.position;
            if (!hasStoped)
            {
                StartCoroutine(Stop());
            }
        }
        else if (transform.position == pointB.position)
        {
            currentTarget = pointA.position;
            previousTarget = pointA.position;
            if (!hasStoped)
            {
                StartCoroutine(Stop());
            }
        }
        else
        {
            hasStoped = false;
        }

        transform.position = Vector3.MoveTowards(transform.position, currentTarget, speed * Time.deltaTime);
    }

    public virtual void CheckLookDirection()
    {
        var faceToBorder = Mathf.Abs(leftMapBorder.position.x - face.position.x);
        var backToBorder = Mathf.Abs(leftMapBorder.position.x - back.position.x);
        if (faceToBorder < backToBorder)
        {
            this.lookDirection = (int)LookDirection.Left;
        }
        else
        {
            this.lookDirection = (int)LookDirection.Right;
        }

        var distanceToPlayer = player.transform.position - this.transform.position;

        if (distanceToPlayer.x > 0 && isInCombat)
        {
            if (this.lookDirection == (int)LookDirection.Left)
            {
                FlipDirection();
            }
        }
        else if (distanceToPlayer.x < 0 && isInCombat)
        {
            if(this.lookDirection == (int)LookDirection.Right)
            {
                FlipDirection();
            }
        }
    }

    IEnumerator Stop()
    {
        hasStoped = true;
        FlipDirection();
        animator.SetBool("Walk", false);
        speed = 0;
        yield return new WaitForSeconds(3f);
        animator.SetBool("Walk", true);
        speed = tempSpeed;
    }

    protected virtual void Update()
    {
        CalculateMovement();
    }

    protected void LateUpdate()
    {
        CheckLookDirection();
    }
}
