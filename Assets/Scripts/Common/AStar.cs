using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;

public class AStar : MonoBehaviour
{
    private const int ASTAR_CUTOFF = 100;

    private List<string> passableTiles = new List<string>() { "green", "red", "64test" };

    public Tilemap tilemap;

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
        
    }

    public List<Vector2Int> CalculateRoute(Vector2Int start, Vector2Int target, int range)
    {
        //the user clicked on his own position
        if (target.x == start.x && target.y == start.y)
        {
            return null;
        }

        List<Tile> openList = new List<Tile>() { new Tile(target, Vector2Int.Distance(target, start), 0, null) };
        List<Vector2Int> closedList = new List<Vector2Int>();
        int i = 0;

        while (openList.Count > 0 && i < ASTAR_CUTOFF)
        {
            //sort list by f value
            openList.Sort((x, y) =>
            {
                float difference = x.f - y.f;

                if (difference < 0)
                {
                    return -1;
                }
                else if (difference > 0)
                {
                    return 1;
                }

                return 0;
            });

            Tile currentPosition = openList[0];
            openList.RemoveAt(0);
            closedList.Add(currentPosition.position);

            if (currentPosition.position.x == start.x && currentPosition.position.y == start.y)
            {
                //final tile has been reached, compute route and return
                List<Vector2Int> route = new List<Vector2Int>();

                while (currentPosition.previousTile != null)
                {
                    currentPosition = currentPosition.previousTile;
                    route.Add(currentPosition.position);
                }

                if (route.Count > range)
                {
                    return new List<Vector2Int>();
                }

                return route;
            }

            foreach (Vector2Int neighbor in FindNeighbors(currentPosition.position, closedList))
            {
                bool isInOpenList = false;

                //iterate over neighbors of current tile
                foreach (Tile tile in openList)
                {
                    //check if neighbor is in openList
                    if (tile.HasPosition(neighbor))
                    {
                        //neighbor is equal to tile, check if distance is shorter
                        if (currentPosition.distance + 1 < tile.distance)
                        {
                            //update f and distance
                            tile.distance = currentPosition.distance + 1;
                            tile.f = tile.distance + Vector2.Distance(tile.position, start);
                            isInOpenList = true;
                            break;
                        }
                    }
                }

                if (!isInOpenList)
                {
                    //add newly found tile to openList
                    openList.Add(new Tile(neighbor, currentPosition.distance + 1 + Vector2.Distance(neighbor, start), currentPosition.distance + 1, currentPosition));
                }
            }

            i++;
        }

        //no route has been found (in time), return null
        return new List<Vector2Int>();
    }

    private List<Vector2Int> FindNeighbors(Vector2Int position, List<Vector2Int> closedList)
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
                if (neighbor.x == position.x && neighbor.y == position.y)
                {
                    return false;
                }
            }

            //filter neighbors that you cant step on
            return this.CheckWalkability(neighbor);
        }).ToList();

        return neighbors;
    }

    public bool CheckWalkability(Vector2Int position)
    {
        TileBase tileBase = tilemap.GetTile(new Vector3Int(position.x, position.y, 0));

        if (tileBase == null)//this should never trigger if wall detection and level layout are done correctly
        {
            return false;
        }

        foreach (string tile in passableTiles)
        {
            if (tileBase.name == tile)
            {
                return true;
            }
        }

        return false;
    }
}
