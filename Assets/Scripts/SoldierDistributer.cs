using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class SoldierDistributer : MonoBehaviour
{
    private bool _isMovingArmy = false;
    private BaseTower _baseTower;
    private List<BaseSoldier> baseSoldiers;

    private void Awake()
    {
        baseSoldiers = new List<BaseSoldier>();
    }
    private void Start()
    {
       _baseTower = GetComponent<BaseTower>();
        baseSoldiers = _baseTower.GetSoldierlist();
    }
    private void Update()
    {
        if (_baseTower.GetTargetTowerPos() != Vector3.zero && !_isMovingArmy)
        {
            StartCoroutine(MoveArmy());
            _isMovingArmy = true;
        }
    }

    private IEnumerator MoveArmy()
    {
       
        //foreach (BaseSoldier baseSoldier in baseSoldiers)
        //{
        //    Vector3 targetPos = _baseTower.GetTargetTowerPos();
        //    baseSoldier.gameObject.SetActive(true);
        //    baseSoldier.SetTarget(targetPos);
        //    baseSoldier.Move();
        //    yield return new WaitForSeconds(3f);
        //}
        //Debug.Log("All Army Deployed");

        for (int i = 0; i < baseSoldiers.Count; i++)
        {
            Vector3 targetPos = _baseTower.GetTargetTowerPos();
            baseSoldiers[i].gameObject.SetActive(true);
            baseSoldiers[i].SetTarget(targetPos);
            baseSoldiers[i].Move();
            yield return new WaitForSeconds(1f);
        }
        Debug.Log("All Army Deployed");
    }
}
