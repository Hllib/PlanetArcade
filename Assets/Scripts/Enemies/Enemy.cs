using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Enemy : MonoBehaviour
{
    protected int health;
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

    protected virtual void Init()
    {
        this.animator = GetComponent<Animator>();
        this.spriteRenderer = GetComponent<SpriteRenderer>();
        this.player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
    }

    protected void DropLoot()
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

    protected void FlipDirection()
    {
        transform.Rotate(0f, 180f, 0f);
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

    protected virtual void CheckAttackZone()
    {

    }

    public virtual void CheckLookDirection()
    {
        float faceToTargetDistance = MathF.Abs(currentTarget.x - face.position.x);
        float backToTargetDistance = MathF.Abs(currentTarget.x - back.position.x);

        if (faceToTargetDistance > backToTargetDistance)
        {
            FlipDirection();
        }
    }

    IEnumerator Stop()
    {
        hasStoped = true;
        animator.SetBool("Walk", false);
        speed = 0;
        yield return new WaitForSeconds(3f);
        animator.SetBool("Walk", true);
        speed = tempSpeed;
    }

    protected virtual void Update()
    {
        if (GameManager.Instance.IsPlayerDead) return;
        CalculateMovement();
    }

    protected void LateUpdate()
    {
        CheckLookDirection();
    }
}
