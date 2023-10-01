using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu()]
public class SoldierDataSO : ScriptableObject
{
    public string SoldierName;
    public SoldierType soldierType;
    public GameObject soldierPrefab;
    public enum SoldierType
    {
        Red,Blue,Green
    }
}

