using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    public int gridWidth = 10;
    public int gridHeight = 10;
    public float gridSpacing = 1f;

    public List<GameObject> gridBlockPrefabs;  // List of possible tile prefabs
    public GameObject startPrefab;  // Prefab for start tile
    public GameObject endPrefab;    // Prefab for end tile

    private GameObject[,] gridArray;  // 2D array to store grid tiles
    private Vector2Int startPos;
    private Vector2Int endPos;

    private void Start()
    {
        GenerateGrid();
        GeneratePath();
    }

    // Generate the grid
    private void GenerateGrid()
    {
        gridArray = new GameObject[gridWidth, gridHeight];

        // Iterate over the grid
        for (int x = 0; x < gridWidth; x++)
        {
            for (int y = 0; y < gridHeight; y++)
            {
                // Generate position for this tile
                Vector3 position = new Vector3(x * gridSpacing, 0, y * gridSpacing);

                // Randomly select a prefab from the gridBlockPrefabs list
                int randomIndex = Random.Range(0, gridBlockPrefabs.Count);
                GameObject selectedPrefab = gridBlockPrefabs[randomIndex];

                // Instantiate the selected prefab
                GameObject tile = Instantiate(selectedPrefab, position, Quaternion.identity);
                gridArray[x, y] = tile;
            }
        }

        // Generate start and end positions
        startPos = new Vector2Int(0, 0);  // Set the start at (0,0)
        endPos = new Vector2Int(gridWidth - 1, gridHeight - 1);  // Set the end at the opposite corner

        // Place start and end tiles
        PlaceSpecialTile(startPrefab, startPos);
        PlaceSpecialTile(endPrefab, endPos);
    }

    // Ensure that there is a path from start to end
    private void GeneratePath()
    {
        // Use a simple path algorithm (here, we are moving right and then up)
        Vector2Int currentPos = startPos;

        while (currentPos != endPos)
        {
            // Move towards the end point either horizontally or vertically
            if (currentPos.x < endPos.x)
            {
                currentPos.x++;
            }
            else if (currentPos.y < endPos.y)
            {
                currentPos.y++;
            }

            // Color the path tiles differently or mark them as part of the path
            GameObject tile = gridArray[currentPos.x, currentPos.y];
            tile.GetComponent<Renderer>().material.color = Color.green;  // Visualize the path
        }
    }

    // Helper method to place a special tile (start or end)
    private void PlaceSpecialTile(GameObject prefab, Vector2Int gridPos)
    {
        Vector3 worldPos = new Vector3(gridPos.x * gridSpacing, 0, gridPos.y * gridSpacing);
        Instantiate(prefab, worldPos, Quaternion.identity);
    }
}
