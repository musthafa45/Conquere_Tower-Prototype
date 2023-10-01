using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseSoldier : MonoBehaviour
{
    [SerializeField] protected float MoveSpeed;
    [SerializeField] protected float RotationSpeed = 50f;
    private Vector3 _target =  Vector3.zero;
    private bool _canMove = false;
    [SerializeField] private SoldierDataSO soldierDataSO;
    private MeshRenderer _soldiermeshRend;

    private void Awake()
    {
        _soldiermeshRend = GetComponent<MeshRenderer>();
    }
    private void Update()
    {
        if (!_canMove && _target == Vector3.zero) return;

        Vector3 targetPos = _target;
        // Move Position Forward to Target
        transform.position = Vector3.MoveTowards(transform.position, targetPos, MoveSpeed * Time.deltaTime); 

        // Rotation
        Vector3 dir = (transform.position - targetPos);   
        Quaternion lookRot = Quaternion.LookRotation(-dir);
        transform.rotation = Quaternion.Slerp(transform.rotation,lookRot, RotationSpeed * Time.deltaTime);

    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.TryGetComponent(out BaseSoldier component))
        {
            Destroy(this.gameObject);
        }
        if(other.gameObject.TryGetComponent(out BaseTower baseTower))
        {
            baseTower.TryToAddOrKillAndDie(this);
            Debug.Log("tower Detected");
        }
    }

    public void SetTarget(Vector3 target)
    {
        _target = target;
    }
    public void Move()
    {
        _canMove = true;
    }
    public void Stop()
    {
        _canMove = false;
    }

    public SoldierDataSO GetSoldierDataSO()
    {
        return soldierDataSO;
    }

    public MeshRenderer GetSoldierTeamColor()
    {
        return _soldiermeshRend;
    }

}
