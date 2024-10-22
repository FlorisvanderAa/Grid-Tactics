using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGridMover : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private LayerMask tileLayer;
    [SerializeField] private LayerMask obstacleLayer;
    [SerializeField] private int maxSteps = 3;        // Max steps per turn
    [SerializeField] private int possibleSteps = 1;
    [SerializeField] private float gridSpacing = 1f;
    [SerializeField] private float checkRadius = 0.5f;
    [SerializeField] private TileManager tileManager; // Reference to the TileManager

    private Vector3 targetPosition;
    private bool isMoving = false;
    private SingleTile currentTile;
    private int remainingSteps;                       // Keep track of steps remaining

    void Start()
    {
        targetPosition = transform.position;
        remainingSteps = maxSteps;
    }

    void Update()
    {
        if (!isMoving) // Only handle mouse input if not currently moving
        {
            HandleMouseClick();
        }

        MoveToTarget();

        if (Input.GetKeyDown(KeyCode.R))
        {
            remainingSteps = maxSteps;
        }
    }

    void HandleMouseClick()
    {
        if (isMoving || remainingSteps == 0) return; // Do not allow more movement if already moving or no steps left

        // Detect mouse click (left mouse button)
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, Mathf.Infinity, tileLayer))
            {
                SingleTile tile = hit.collider.GetComponent<SingleTile>();

                if (tile != null)
                {
                    Vector3 tilePosition = hit.collider.transform.position;
                    int steps = CalculateStepsToTarget(tilePosition);

                    if (steps == possibleSteps && !IsObstacleInPath(transform.position, tilePosition)) // Only allow 1-step moves
                    {
                        if (currentTile != null)
                        {
                            currentTile.SetSelected(false);  // Deselect the previous tile
                        }

                        tile.SetSelected(true);
                        currentTile = tile;

                        // Move to the selected tile
                        targetPosition = tilePosition;
                        remainingSteps--;  // Decrement the step count

                        // Clear highlights after selecting a tile
                        tileManager.ClearHighlightedTiles();
                    }
                    else
                    {
                        Debug.Log("Invalid move! Only 1 step allowed per move or path is blocked.");
                    }
                }
            }
        }

        // Show possible movement range when player is selected (after right-click)
        if (Input.GetMouseButtonDown(1))
        {
            ShowPossibleMoves();
        }
    }

    // Show possible tiles the player can move to
    void ShowPossibleMoves()
    {
        tileManager.HighlightTilesInRange(transform.position, possibleSteps, gridSpacing); // Show tiles in range of 1 step
    }

    // Calculate the number of steps from current position to target position without diagonal movement
    int CalculateStepsToTarget(Vector3 targetPos)
    {
        Vector3 diff = targetPos - transform.position;

        // Calculate steps separately for X and Z axes without allowing diagonal moves
        int stepsX = Mathf.Abs(Mathf.RoundToInt(diff.x / gridSpacing));
        int stepsZ = Mathf.Abs(Mathf.RoundToInt(diff.z / gridSpacing));

        // Only allow orthogonal movement (no diagonal steps)
        int totalSteps = stepsX + stepsZ;

        return totalSteps;
    }

    // Move towards the target tile, 1 step at a time
    void MoveToTarget()
    {
        if (Vector3.Distance(transform.position, targetPosition) > 0.01f)
        {
            isMoving = true;
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);

            // When the player reaches the target tile
            if (Vector3.Distance(transform.position, targetPosition) <= 0.01f)
            {
                isMoving = false;

                // Show the possible moves after each step, as long as steps remain
                if (remainingSteps > 0)
                {
                    ShowPossibleMoves();
                }
            }
        }
    }

    // Check if there is an obstacle between the current position and the target position
    bool IsObstacleInPath(Vector3 startPos, Vector3 endPos)
    {
        // Create a direction vector from the current position to the target position
        Vector3 direction = (endPos - startPos).normalized;

        // Cast a ray from the current position in the direction of the target position
        float distance = Vector3.Distance(startPos, endPos);

        RaycastHit hit;
        if (Physics.Raycast(startPos, direction, out hit, distance, obstacleLayer))
        {
            // If we hit an obstacle, return true (path is blocked)
            return true;
        }

        // No obstacles detected, path is clear
        return false;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawLine(transform.position, targetPosition);
    }
}
