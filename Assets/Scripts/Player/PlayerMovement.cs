using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : CharacterMovement
{

    private float moveX, moveY;

    private Camera mainCam;

    private Vector2 mousePosition;
    private Vector2 direction;
    private Vector3 tempScale;

    private Animator anim;

    
    private PlayerWeaponManager playerWeaponManager;
    /*
    private CharacterHealth playerHealth;
    */

    protected override void Awake()
    {
        base.Awake();

        mainCam = Camera.main;

        anim = GetComponent<Animator>();

        playerWeaponManager = GetComponent<PlayerWeaponManager>();

    }

    private void Start()
    {
        //playerHealth = GetComponent<CharacterHealth>();
    }

    private void FixedUpdate()
    {

        //if (!playerHealth.IsAlive())
           // return;

        moveX = Input.GetAxisRaw(TagManager.HORIZONTAL_AXIS);
        moveY = Input.GetAxisRaw(TagManager.VERTICAL_AXIS);

        HandlePlayerTurning();

        HandleMovement(moveX, moveY);
    }

    void HandlePlayerTurning()
    {
        mousePosition = mainCam.ScreenToWorldPoint(Input.mousePosition);

        direction = new Vector2(mousePosition.x - transform.position.x,
            mousePosition.y - transform.position.y).normalized;

        //Vector holds 2 pieces of information - a point in space and a magnitude.
        //    The magnitude is the length of the line formed between(0, 0, 0) and the
        //    point in space.If you "normalize" a vector, the result is a line that
        //    starts at(0, 0, 0) and "points" to your original point in space.
        //    If you were to take the length of this "pointer" it would equal 1 unit length

        HandlePlayerAnimation(direction.x, direction.y);

    }

    void HandlePlayerAnimation(float x, float y)
    {

        x = Mathf.RoundToInt(x);
        y = Mathf.RoundToInt(y);

        tempScale = transform.localScale;

        if (x > 0)
            tempScale.x = Mathf.Abs(tempScale.x);
        else if (x < 0)
            tempScale.x = -Mathf.Abs(tempScale.x);

        transform.localScale = tempScale;

        x = Mathf.Abs(x);

        anim.SetFloat(TagManager.FACE_X_ANIMATION_PARAMETER, x);
        anim.SetFloat(TagManager.FACE_Y_ANIMATION_PARAMETER, y);

        ActivateWeaponForSide(x, y);
    }

    void ActivateWeaponForSide(float x, float y)
    {
        // side
        if (x == 1f && y == 0f)
            playerWeaponManager.ActivateGun(0);

        // up
        if (x == 0f && y == 1f)
            playerWeaponManager.ActivateGun(1);

        // down
        if (x == 0f && y == -1f)
            playerWeaponManager.ActivateGun(2);

        // side up
        if (x == 1f && y == 1f)
            playerWeaponManager.ActivateGun(3);

        // side down
        if (x == 1f && y == -1f)
            playerWeaponManager.ActivateGun(4);
    }
}
