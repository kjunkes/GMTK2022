using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputHandler : MonoBehaviour
{
    public PlayerMovement playerMovement;

    private Camera cam;

    // Start is called before the first frame update
    void Start()
    {
        cam = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonUp(0))
        {
            Vector2 target = cam.ScreenToWorldPoint(Input.mousePosition);
            playerMovement.Move(new Vector2Int(Mathf.RoundToInt(target.x), Mathf.RoundToInt(target.y)));
        }
    }
}
