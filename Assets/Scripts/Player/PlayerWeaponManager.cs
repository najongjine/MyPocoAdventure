using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWeaponManager : MonoBehaviour
{
    [SerializeField]
    private WeaponManager[] playerWeapons;

    private int weaponIndex;

    [SerializeField]
    private GameObject[] weaponBullets;

    private Vector2 targetPos;

    private Vector2 direction;

    private Camera mainCam;

    private Vector2 bulletSpawnPosition;

    private Quaternion bulletRotation;

    private CameraShake cameraShake;

    [SerializeField]
    private float cameraShakeCooldown = 0.2f;

    private void Awake()
    {
        weaponIndex = 0;
        playerWeapons[weaponIndex].gameObject.SetActive(true);

        mainCam = Camera.main;

        cameraShake = mainCam.GetComponent<CameraShake>();
    }

    private void Update()
    {
        ChangeWeapon();
    }

    public void ActivateGun(int gunIndex)
    {
        playerWeapons[weaponIndex].ActivateGun(gunIndex);
    }

    void ChangeWeapon()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            playerWeapons[weaponIndex].gameObject.SetActive(false);

            weaponIndex++;

            if (weaponIndex == playerWeapons.Length)
                weaponIndex = 0;

            playerWeapons[weaponIndex].gameObject.SetActive(true);
        }
    }

    public void Shoot(Vector3 spawnPos)
    {

        targetPos = mainCam.ScreenToWorldPoint(Input.mousePosition);

        bulletSpawnPosition = new Vector2(spawnPos.x, spawnPos.y);

        // A vector is either a point, or a direction.
        // Vector3(0, 0, 5) is a direction of 5 units along the z axis,
        // so it has a length of 5. If you normalize that, it's Vector3(0, 0, 1).
        // You would do this when you only want the direction and don't care about the length
        direction = (targetPos - bulletSpawnPosition).normalized;

        bulletRotation = Quaternion.Euler(0, 0,
            Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg);

        BulletPool.instance.FireBullet(weaponIndex, spawnPos,
            bulletRotation, direction);

        cameraShake.ShakeCamera(cameraShakeCooldown);
        /*
        GameObject newBullet = Instantiate(weaponBullets[weaponIndex],
            spawnPos, bulletRotation);

        newBullet.GetComponent<Bullet>().MoveInDirection(direction);
        */
        //playerWeapons[weaponIndex].PlayShootSound();

    }
}
