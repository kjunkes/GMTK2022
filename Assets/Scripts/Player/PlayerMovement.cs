using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    const int CUTOFF = 1000;

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

    public void Move(Vector2Int target)
    {
        Vector2Int currentPosition = new Vector2Int(Mathf.RoundToInt(transform.position.x), Mathf.RoundToInt(transform.position.y));

        List<Vector2Int> route = this.AStar(target, currentPosition);

        foreach(Vector2Int vector in route)
        {
            Debug.Log(vector);
        }
        Debug.Log("");
    }

    private List<Vector2Int> AStar(Vector2Int start, Vector2Int target)
    {
        List<Tile> openList = new List<Tile>() { new Tile(start, Vector2Int.Distance(start, target), 0, null) };
        List<Vector2Int> closedList = new List<Vector2Int>();
        int i = 0;

        while(openList.Count > 0 && i < CUTOFF)
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
            openList.Remove(currentPosition);
            closedList.Add(currentPosition.position);

            if(currentPosition.position.x == target.x && currentPosition.position.y == target.y)
            {
                //final tile has been reached, compute route and return
                List<Vector2Int> route = new List<Vector2Int>() { currentPosition.position };

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
                        //neighbor is equal to tile, check distance is shorter
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

        //filter elements from closedList
        neighbors = neighbors.Where(x =>
        {
            foreach(Vector2Int position in closedList)
            {
                if(x.x == position.x && x.y == position.y)
                {
                    return false;
                }
            }

            return true;
        }).ToList();

        //TODO filter for walkability

        return neighbors;
    }
}
