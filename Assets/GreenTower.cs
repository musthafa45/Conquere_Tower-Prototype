using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GreenTower : BaseTower
{
    protected override int TowerHealth { get ; set; }
    protected override int TowerUnitCapacity { get; set; } = 30;
}
