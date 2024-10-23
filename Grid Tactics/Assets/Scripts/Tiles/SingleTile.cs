using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SingleTile : MonoBehaviour, IInteractable
{
    [SerializeField] private GameObject hoverVisual;
    [SerializeField] private GameObject selectedVisual;

    private bool isHovered = false;
    private bool isSelected = false;

    private void Start()
    {
        hoverVisual.SetActive(false);
        selectedVisual.SetActive(false);
    }

    // Implementing hover effect
    private void OnMouseEnter()
    {
        // When the mouse hovers over this tile, show the hover visual
        if (!isSelected)
        {
            hoverVisual.SetActive(true);
        }
    }

    private void OnMouseExit()
    {
        // When the mouse stops hovering, hide the hover visual
        if (!isSelected)
        {
            hoverVisual.SetActive(false);
        }
    }

    public void SetSelected(bool selected)
    {
        isSelected = selected;
        hoverVisual.SetActive(false);
        selectedVisual.SetActive(isSelected);
    }

    // Implement the Interact method (for future purposes)
    public void Interact()
    {
        SpawnManager.Instance.PlacePlayer(this.transform);
    }
}
