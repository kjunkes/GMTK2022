using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    public float SPEED = 3f;
    public int WALKING_RANGE = 2;

    private AStar astar;
    private BaseEnemy baseEnemy;

    private List<Vector2Int> route = new List<Vector2Int>();

    private Vector3 originalPosition;

    // Start is called before the first frame update
    void Start()
    {
        astar = FindObjectOfType<AStar>();
        baseEnemy = FindObjectOfType<BaseEnemy>();
        originalPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        //there is still a path to be walked
        if (this.route.Count > 0)
        {
            //Move a bit towards the next route position
            if (this.MoveAlongRoute())
            {
                route.RemoveAt(0);
            }
        }
    }

    //Move a bit along the route, returns whether the first element in route has been reached
    private bool MoveAlongRoute()
    {
        Vector2 moveDirection = (route[0] - (Vector2)transform.position);
        moveDirection.Normalize();
        moveDirection *= SPEED * Time.deltaTime;
        Vector3 moveDirection3D = new Vector3(moveDirection.x, moveDirection.y, 0);
        Vector3 result = transform.position + moveDirection3D;

        Vector2 transformDelta = route[0] - (Vector2)transform.position;
        Vector2 resultDelta = route[0] - (Vector2)result;
        transformDelta.Normalize();
        resultDelta.Normalize();
        float xProduct = transformDelta.x * resultDelta.x;
        float yProduct = transformDelta.y * resultDelta.y;

        if (xProduct < 0 || yProduct < 0)
        {
            //arrived at new position
            transform.position = new Vector3(route[0].x, route[0].y, 0);
            return true;
        }
        else
        {
            transform.position = result;
            return false;
        }
    }

    public void InitiateMove(Vector2Int target)
    {
        if (this.route.Count > 0 || !this.astar.CheckWalkability(target))
        {
            //if there is still a route to be walked or the target is not reachable, dont initiate a move
            return;
        }

        Vector2Int currentPosition = new Vector2Int(Mathf.RoundToInt(transform.position.x), Mathf.RoundToInt(transform.position.y));
        route = this.astar.CalculateRoute(currentPosition, target, 10000);

        if (route.Count > WALKING_RANGE)
        {
            route.RemoveRange(WALKING_RANGE, route.Count - WALKING_RANGE);
        }

        if (route.Count > 0)
        {
            //exclude target from route to avoid the enemy stepping on us
            if (route[route.Count - 1].x == target.x && route[route.Count - 1].y == target.y)
            {
                route.RemoveAt(route.Count - 1);
            }
        }
    }

    public List<Vector2Int> GetRoute()
    {
        return this.route;
    }
}
