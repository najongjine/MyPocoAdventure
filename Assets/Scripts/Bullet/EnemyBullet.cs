using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBullet : MonoBehaviour
{
    [SerializeField]
    private bool isSlow, canRotate;

    [SerializeField]
    private bool poolBullet;

    [SerializeField]
    private float deactivateTimer = 5f;

    private Rigidbody2D myBody;

    private Animator anim;

    [SerializeField]
    private float damageAmount = 10f;

    private bool dealthDamage;

    private void Awake()
    {
        myBody = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }

    private void Start()
    {
        Invoke("DeactivateBullet", deactivateTimer);
    }

    private void FixedUpdate()
    {
        if (isSlow)
            myBody.velocity = Vector2.Lerp(myBody.velocity, Vector2.zero,
                Random.value * Time.deltaTime);

        if (canRotate)
            transform.Rotate(Vector3.forward * 60f);

    }

    private void OnDisable()
    {
        transform.rotation = Quaternion.identity;
        isSlow = false;
    }

    void DeactivateBullet()
    {
        if (poolBullet)
            gameObject.SetActive(false);
        else
            Destroy(gameObject);
    }

    public void SetIsSlow(bool slow)
    {
        isSlow = slow;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {

        if (collision.CompareTag(TagManager.BLOCKING_TAG))
        {
            myBody.velocity = Vector2.zero;
            CancelInvoke("DeactivateBullet");
            anim.SetTrigger(TagManager.EXPLODE_ANIMATION_PARAMETER);
        }

        if (collision.CompareTag(TagManager.PLAYER_TAG))
        {
            myBody.velocity = Vector2.zero;
            CancelInvoke("DeactivateBullet");
            anim.SetTrigger(TagManager.EXPLODE_ANIMATION_PARAMETER);

            if (!dealthDamage)
            {
                dealthDamage = true;
                collision.GetComponent<CharacterHealth>().TakeDamage(damageAmount);
            }

        }

    }

}
