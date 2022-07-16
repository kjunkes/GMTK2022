using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;

public class PlayerMovement : MonoBehaviour
{
    public float SPEED = 2f;

    public Tilemap tilemap;
    public GameLoop gameLoop;

    private const int ASTAR_CUTOFF = 1000;

    //insert names of passable Tiles
    private List<String> passableTiles = new List<string>() {"green", "red"};

    private List<Vector2Int> route = new List<Vector2Int>();

    class Tile
    {
        public Vector2Int position;
        public float f;
        public float distance;
        public Tile previousTile;

        public Tile(Vector2Int position, float f, float distance, Tile previousTile)
        {
            this.position = position;
            this.f = f;
            this.distance = distance;
            this.previousTile = previousTile;
        }

        public bool HasPosition(Vector2Int position)
        {
            return this.position.x == position.x && this.position.y == position.y;
        }
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        //there is still a path to be walked
        if (this.route.Count > 0)
        {
            //Move a bit towards the next route position
            if(this.Move())
            {
                route.RemoveAt(0);
                
                if(route.Count == 0)
                {
                    gameLoop.IncrementTurnState();
                }
            }
        }
    }

    //Move a bit along the route, returns whether the first element in route has been reached
    private bool Move()
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
        if(this.route.Count > 0 || !this.CheckWalkability(target))
        {
            //if there is still a route to be walked or the target is not reachable, dont initiate a move
            return;
        }

        Vector2Int currentPosition = new Vector2Int(Mathf.RoundToInt(transform.position.x), Mathf.RoundToInt(transform.position.y));
        route = this.calculateRoute(target, currentPosition);
    }

    //A* algorithm
    private List<Vector2Int> calculateRoute(Vector2Int start, Vector2Int target)
    {
        List<Tile> openList = new List<Tile>() { new Tile(start, Vector2Int.Distance(start, target), 0, null) };
        List<Vector2Int> closedList = new List<Vector2Int>();
        int i = 0;

        while(openList.Count > 0 && i < ASTAR_CUTOFF)
        {
            //sort list by f value
            openList.Sort((x, y) =>
            {
                float difference = x.f - y.f;
                
                if(difference < 0)
                {
                    return -1;
                } else if(difference > 0)
                {
                    return 1;
                }

                return 0;
            });

            Tile currentPosition = openList[0];
            openList.RemoveAt(0);
            closedList.Add(currentPosition.position);

            if(currentPosition.position.x == target.x && currentPosition.position.y == target.y)
            {
                //final tile has been reached, compute route and return
                List<Vector2Int> route = new List<Vector2Int>();

                while(currentPosition.previousTile != null)
                {
                    currentPosition = currentPosition.previousTile;
                    route.Add(currentPosition.position);
                }

                return route;
            }

            foreach(Vector2Int neighbor in findNeighbors(currentPosition.position, closedList))
            {
                bool isInOpenList = false;

                //iterate over neighbors of current tile
                foreach(Tile tile in openList)
                {
                    //check if neighbor is in openList
                    if (tile.HasPosition(neighbor))
                    {
                        //neighbor is equal to tile, check if distance is shorter
                        if(currentPosition.distance + 1 < tile.distance)
                        {
                            //update f and distance
                            tile.distance = currentPosition.distance + 1;
                            tile.f = tile.distance + Vector2.Distance(tile.position, target);
                            isInOpenList = true;
                            break;
                        }
                    }
                }

                if (!isInOpenList)
                {
                    //add newly found tile to openList
                    openList.Add(new Tile(neighbor, currentPosition.distance + 1 + Vector2.Distance(neighbor, target), currentPosition.distance + 1, currentPosition));
                }
            }
        }

        //no route has been found (in time), return null
        return null;
    }

    private List<Vector2Int> findNeighbors(Vector2Int position, List<Vector2Int> closedList)
    {
        List<Vector2Int> neighbors = new List<Vector2Int>();

        neighbors.Add(new Vector2Int(position.x - 1, position.y));
        neighbors.Add(new Vector2Int(position.x + 1, position.y));
        neighbors.Add(new Vector2Int(position.x, position.y + 1));
        neighbors.Add(new Vector2Int(position.x, position.y - 1));

        neighbors = neighbors.Where(neighbor =>
        {
            //filter neighbors that already made the closedList
            foreach (Vector2Int position in closedList)
            {
                if(neighbor.x == position.x && neighbor.y == position.y)
                {
                    return false;
                }
            }

            //filter neighbors that you cant step on
            return this.CheckWalkability(neighbor);
        }).ToList();

        return neighbors;
    }

    private bool CheckWalkability(Vector2Int position)
    {
        TileBase tileBase = tilemap.GetTile(new Vector3Int(position.x, position.y, 0));

        if (tileBase == null)//this should never trigger if wall detection and level layout are done correctly
        {
            return false;
        }

        foreach (String tile in passableTiles)
        {
            if (tileBase.name == tile)
            {
                return true;
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