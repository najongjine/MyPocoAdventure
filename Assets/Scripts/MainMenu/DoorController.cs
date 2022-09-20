using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorController : MonoBehaviour
{
    [SerializeField]
    private Animator doorAnimatior;

    private AudioSource gateOpenSound;

    private void Awake()
    {
        gateOpenSound = GetComponent<AudioSource>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag(TagManager.MAIN_MENU_CHARACTER_TAG))
        {
            gateOpenSound.Play();
            doorAnimatior.SetBool(TagManager.OPEN_ANIMATION_PARAMETER, true);
        }
    }
}
