using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    private Vector3 lastPosition;
    private Vector3 currentPosition;
    private Vector3 positionDiff;
    private int walk;

    private Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        animator = this.GetComponent<Animator>();
        lastPosition = gameObject.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        currentPosition = gameObject.transform.position;
        positionDiff = (currentPosition - lastPosition).normalized;
        lastPosition = currentPosition;

        if(positionDiff.x == 1f)
        {
            walk = 2;
        }
        else if(positionDiff.x == -1f)
        {
            walk = 4;
        }
        else if(positionDiff.y == 1f)
        {
            walk = 1;
        }
        else if(positionDiff.y == -1f)
        {
            walk = 3;
        }
        else
        {
            walk = 0;
        }

        animator.SetInteger("Walk", walk);
    }
}
