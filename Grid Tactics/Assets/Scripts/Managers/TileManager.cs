using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileManager : MonoBehaviour
{
    [SerializeField] private List<GameObject> tiles = new List<GameObject>();   // List of all tiles

    // Highlight tiles in range based on player position and max steps
    public void HighlightTilesInRange(Vector3 playerPosition, int maxSteps, float gridSpacing)
    {
        // Iterate through all tiles
        foreach (GameObject tile in tiles)
        {
            Vector3 tilePosition = tile.transform.position;

            // Calculate the number of steps required to reach the tile
            int stepsX = Mathf.Abs(Mathf.RoundToInt((tilePosition.x - playerPosition.x) / gridSpacing));
            int stepsZ = Mathf.Abs(Mathf.RoundToInt((tilePosition.z - playerPosition.z) / gridSpacing));
            int totalSteps = stepsX + stepsZ;

            // Highlight the tile if it's within the max steps
            if (totalSteps <= maxSteps)
            {
                SingleTile singleTile = tile.GetComponent<SingleTile>();
                if (singleTile != null)
                {
                    singleTile.SetSelected(true);
                }
            }
        }
    }

    // Clear the highlights on all tiles
    public void ClearHighlightedTiles()
    {
        foreach (GameObject tile in tiles)
        {
            SingleTile singleTile = tile.GetComponent<SingleTile>();
            if (singleTile != null)
            {
                singleTile.SetSelected(false);
            }
        }
    }
}
