using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Player", menuName = "ScriptableObjects/Player")]
public class PlayerSO : ScriptableObject
{
    public string name;
    public GameObject playerPrefab;
    public int steps;
}
