using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public abstract class BaseTower : MonoBehaviour
{
    protected abstract int TowerHealth { get; set; }
    protected abstract int TowerUnitCapacity { get; set; }
    [SerializeField] private TextMeshPro _currentSoldiersCount;

    private List<BaseSoldier> _ocupierSoldiers;

    [SerializeField] protected SoldierDataSO _soldierDataSo;
    [SerializeField] protected MeshRenderer[] towerMeshes;
    private Vector3 _targetbuildingPos;

    private void Awake()
    {
        _ocupierSoldiers = new List<BaseSoldier>(TowerUnitCapacity);

    }
    private void Start()
    {
        InitializeSoldiers();
        _currentSoldiersCount.text = _ocupierSoldiers.Count.ToString();
    }

    private void InitializeSoldiers()
    {
        for (int i = 0; i < TowerUnitCapacity; i++)
        {
            var instanceprefab = Instantiate(_soldierDataSo.soldierPrefab, transform.position, Quaternion.identity, transform);
            instanceprefab.transform.position = transform.position;
            instanceprefab.transform.localPosition = Vector3.zero;
            _ocupierSoldiers.Add(instanceprefab.GetComponent<BaseSoldier>());
            instanceprefab.SetActive(false);
        }
    }

    public List<BaseSoldier> GetSoldierlist()
    {
        return _ocupierSoldiers;
    }

    public void SetTargetTower(BaseTower baseTower)
    {
        this._targetbuildingPos = baseTower.transform.position;
    }

    public Vector3 GetTargetTowerPos()
    {
        return this._targetbuildingPos;
    }

    internal void TryToAddOrKillAndDie(BaseSoldier enterSoldier)
    {

        if (_ocupierSoldiers.Count == 0)
        {
            _ocupierSoldiers.Add(enterSoldier);
        }
        else
        {
            bool issametypeSoldierFound = false;

            foreach (BaseSoldier occupiedSoldier in _ocupierSoldiers)
            {
                if (occupiedSoldier.GetType() != enterSoldier.GetType())
                {
                    //Fight
                    BaseSoldier killingBaseSoldier = _ocupierSoldiers[0];
                    _ocupierSoldiers.Remove(killingBaseSoldier);

                    Destroy(enterSoldier.gameObject);
                    issametypeSoldierFound = false;
                    break;
                }
                else
                {
                    issametypeSoldierFound = true;
                    break;
                }

            }
            if (issametypeSoldierFound)
            {
                _ocupierSoldiers.Add(enterSoldier);

                var baseSoldier = _ocupierSoldiers.GroupBy(s => s);
                var leadSoldier = baseSoldier.OrderByDescending(grp => grp.Count()).First();

                BaseSoldier occupiedTeam = leadSoldier.Key;
                foreach(var mesh in towerMeshes)
                {
                    mesh.material = occupiedTeam.GetSoldierTeamColor().material;
                }
                enterSoldier.gameObject.SetActive(false);
            }
        }

        UpdateOccupiedSoldiersCount();

    }

    private void UpdateOccupiedSoldiersCount()
    {
        int redSoldiercount = _ocupierSoldiers.Count(s => s is RedSoldier);
        int blueSoldiercount = _ocupierSoldiers.Count(s => s is BlueSoldier);
        int greenSoldiercount = _ocupierSoldiers.Count(s => s is SoldierGreen);

           

        Debug.Log("Red Soldier Count :" + "" + redSoldiercount);
        Debug.Log("blue Soldier Count :" + "" + blueSoldiercount);
        Debug.Log("Green Soldier Count :" + "" + greenSoldiercount);
        _currentSoldiersCount.text = _ocupierSoldiers.Count.ToString();
    }
}

