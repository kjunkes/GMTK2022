using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class InputHandler : MonoBehaviour
{
    public PlayerMovement playerMovement;

    private GameLoop gameLoop;
    private Camera cam;

    // Start is called before the first frame update
    void Start()
    {
        gameLoop = FindObjectOfType<GameLoop>();
        cam = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonUp(0) && this.gameLoop.CanWalk() && !IsMouseOverUIElement())
        {
            Vector2 target = cam.ScreenToWorldPoint(Input.mousePosition);
            playerMovement.InitiateMove(new Vector2Int(Mathf.RoundToInt(target.x), Mathf.RoundToInt(target.y)));
        }
    }

    //Checks if the mouse is currently hovering over UI
    private bool IsMouseOverUIElement()
    {
        PointerEventData eventData = new PointerEventData(EventSystem.current);
        eventData.position = Input.mousePosition;
        List<RaycastResult> raycastResults = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventData, raycastResults);

        for (int i = 0; i < raycastResults.Count; i++)
        {
            RaycastResult curRaycastResult = raycastResults[i];

            if (curRaycastResult.gameObject.layer == LayerMask.NameToLayer("UI"))
                {
                return true;
            }
        }

        return false;
    }
}
