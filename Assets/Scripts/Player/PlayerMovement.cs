using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;

public class PlayerMovement : MonoBehaviour
{
    public float SPEED = 3f;
    public int WALKING_RANGE = 4;

    public Tilemap tilemap;
    public SpriteRenderer walkingRangeIndicator;

    private GameLoop gameLoop;
    private AStar astar;

    //insert names of ending Tiles
    private List<string> endingTiles = new List<string>() { "red" };

    private List<Vector2Int> route = new List<Vector2Int>();

    // Start is called before the first frame update
    void Start()
    {
        gameLoop = FindObjectOfType<GameLoop>();
        astar = FindObjectOfType<AStar>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!this.gameLoop.CanWalk() || this.route.Count > 0)
        {
            this.walkingRangeIndicator.gameObject.SetActive(false);
        }
        else
        {
            this.walkingRangeIndicator.gameObject.SetActive(true);
        }

        //there is still a path to be walked
        if (this.route.Count > 0)
        {
            //Move a bit towards the next route position
            if(this.MoveAlongRoute())
            {
                route.RemoveAt(0);
                
                if(route.Count == 0)
                {
                    CheckLevelComplete();
                    this.gameLoop.IncrementTurnState();
                }
            }
        }
    }

    //Move a bit along the route, returns whether the first element in route has been reached
    private bool MoveAlongRoute()
    {
        Vector2 moveDirection = (route[0] - (Vector2)transform.position);
        moveDirection = NormalizeVector(moveDirection);
        moveDirection *= SPEED * Time.deltaTime;
        Vector3 moveDirection3D = new Vector3(moveDirection.x, moveDirection.y, 0);
        Vector3 result = transform.position + moveDirection3D;

        Vector2 transformDelta = route[0] - (Vector2)transform.position;
        Vector2 resultDelta = route[0] - (Vector2)result;
        transformDelta = NormalizeVector(transformDelta);
        resultDelta = NormalizeVector(resultDelta);
        float xProduct = transformDelta.x * resultDelta.x;
        float yProduct = transformDelta.y * resultDelta.y;

        if (xProduct < 0 || yProduct < 0)
        {
            //arrived at new position
            this.MoveTo(new Vector3(route[0].x, route[0].y, 0));
            return true;
        }
        else
        {
            this.MoveTo(result);
            return false;
        }
    }

    private void MoveTo(Vector3 target)
    {
        transform.position = target;
        Transform cameraTransform = Camera.main.transform;
        cameraTransform.position = new Vector3(target.x, target.y, cameraTransform.position.z);
    }

    public void InitiateMove(Vector2Int target)
    {
        if(this.route.Count > 0 || !this.astar.CheckWalkability(target))
        {
            //if there is still a route to be walked or the target is not reachable, dont initiate a move
            return;
        }

        Vector2Int currentPosition = new Vector2Int(Mathf.RoundToInt(transform.position.x), Mathf.RoundToInt(transform.position.y));
        route = this.astar.CalculateRoute(currentPosition, target, WALKING_RANGE);

        //user clicked on his own position -> does not want to move this round
        if(route == null)
        {
            route = new List<Vector2Int>();
            this.gameLoop.IncrementTurnState();
        }
    }

    public Vector2 NormalizeVector(Vector2 value)
    {
        float x, y;

        if (value.x < 0)
        {
            x = -1;
        }
        else if (value.x > 0)
        {
            x = 1;
        }
        else
        {
            x = 0;
        }

        if (value.y < 0)
        {
            y = -1;
        }
        else if (value.y > 0)
        {
            y = 1;
        }
        else
        {
            y = 0;
        }

        return new Vector2(x, y);
    }

    private bool CheckLevelComplete()
    {
        TileBase tileBase = tilemap.GetTile(new Vector3Int((int)transform.position.x, (int)transform.position.y, 0));

        if (tileBase == null)//this should never trigger if wall detection and level layout are done correctly
        {
            return false;
        }

        foreach (string tile in endingTiles)
        {
            if (tileBase.name == tile)
            {
                this.gameLoop.EndLevel();
            }
        }

        return false;
    }

    public List<Vector2Int> GetRoute()
    {
        return this.route;
    }

    public void SetRoute(List<Vector2Int> value)
    {
        this.route = value;
    }
}
