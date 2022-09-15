using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterHealth : MonoBehaviour
{
    [SerializeField]
    private float maxHealth = 100f;

    [SerializeField]
    private float health;

    private Animator anim;

    private void Awake()
    {
        anim = GetComponent<Animator>();
    }

    private void Start()
    {
        health = maxHealth;
    }

    public void TakeDamage(float damageAmount)
    {

        health -= damageAmount;

        if (health <= 0f)
        {
            anim.SetTrigger(TagManager.DEATH_ANIMATION_PARAMETER);
        }

    }

    private void DestroyCharacter()
    {
        Destroy(gameObject);
    }

    public bool IsAlive()
    {
        return health > 0 ? true : false;
    }

}
