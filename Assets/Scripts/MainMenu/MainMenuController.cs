using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuController : MonoBehaviour
{
    private Camera mainCam;

    [SerializeField]
    private float cameraOrthoSize = 1f, cameraYPos = 0.63f;

    private bool canChangeCameraPos;

    private Vector3 cameraTempPos;

    [SerializeField]
    private float lerpSpeed = 5f;

    [SerializeField]
    private GameObject tapButton;

    private bool canSelectCharacter;

    private void Awake()
    {
        mainCam = Camera.main;
    }

    private void Update()
    {
        ChangeCameraPosition();
        SelectCharacter();
    }

    public void TapToStartGame()
    {
        tapButton.SetActive(false);

        canChangeCameraPos = true;
    }

    void ChangeCameraPosition()
    {
        if (canChangeCameraPos)
        {
            mainCam.orthographicSize = Mathf.Lerp(mainCam.orthographicSize, cameraOrthoSize,
                lerpSpeed * Time.deltaTime);

            cameraTempPos = mainCam.transform.position;
            cameraTempPos.y = Mathf.Lerp(cameraTempPos.y, cameraYPos, lerpSpeed * Time.deltaTime);
            mainCam.transform.position = cameraTempPos;

            if (Mathf.Approximately(mainCam.transform.position.y, cameraYPos) &&
                Mathf.Approximately(mainCam.orthographicSize, cameraOrthoSize))
            {
                Debug.Log("Finished Moving The Camera");
                canChangeCameraPos = false;
                canSelectCharacter = true;
            }

        }
    }

    void SelectCharacter()
    {
        if (canSelectCharacter)
        {
            if (Input.GetMouseButtonDown(0))
            {

                RaycastHit2D hit = Physics2D.Raycast
                    (mainCam.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);

                if (hit.collider != null && hit.transform.CompareTag(TagManager.MAIN_MENU_CHARACTER_TAG))
                {
                    hit.transform.GetComponent<CharacterSelectionPlayer>().enabled = true;
                    mainCam.GetComponent<MainMenuCamera>().SetPlayerTarget(hit.transform);
                }

            }
        }
    }
}
