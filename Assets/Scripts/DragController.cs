using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEngine;
using UnityEngine.EventSystems;

public class DragController : MonoBehaviour
{
    public static DragController Instance;

    private LineRenderer lineRenderer;
    private Vector3 startPoint;
    private Vector3 endPoint;
    private bool isDrawing;
    [SerializeField] private LayerMask _castableMask;

    private BaseTower _currentStartDragPosBuilding;
    private BaseTower _targetOfStartdragBuilding;

    private Camera _camera;
    private GameObject _interactedBuilding;
    private void Awake()
    {
        Instance = this;
        _camera = Camera.main;
    }

    void Update()
    {

        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = _camera.ScreenPointToRay(Input.mousePosition);
            RaycastHit raycastHit;
            if (Physics.Raycast(ray, out raycastHit, 100, _castableMask))
            {
                if (raycastHit.collider != null)
                {
                    _interactedBuilding = raycastHit.collider.gameObject;
                    _currentStartDragPosBuilding = raycastHit.collider.gameObject.GetComponent<BaseTower>();
                    lineRenderer = raycastHit.collider.GetComponentInChildren<LineRenderer>();
                    lineRenderer.positionCount = 2;
                    StartDrawing();
                }
            }
        }
        else if (Input.GetMouseButton(0))
        {
            ContinueDrawing();
        }
        else if (Input.GetMouseButtonUp(0))
        {
            FinishDrawing();
        }
    }

    void StartDrawing()
    {
        if (lineRenderer != null)
        {
            Vector3 newstartPoint = _interactedBuilding.transform.position;
            newstartPoint.y = .1f;
            startPoint = /*GetMouseWorldPosition();*/ newstartPoint;
            isDrawing = true;
            lineRenderer.SetPosition(0, startPoint);
        }


    }

    void ContinueDrawing()
    {
        if (isDrawing && lineRenderer != null)
        {
            endPoint = /*GetMouseWorldPosition()*/GetMouseWorldPosition3D();
            //endPoint.z = 0f;
            endPoint.y = 0.1f;
            lineRenderer.gameObject.transform.rotation = Quaternion.Euler(90f, 0f, 0f);
            lineRenderer.SetPosition(1, endPoint);
        }
    }

    void FinishDrawing()
    {
        if (lineRenderer != null)
        {
            isDrawing = false;

            Ray ray = _camera.ScreenPointToRay(Input.mousePosition);
            RaycastHit raycastHit;
            if (Physics.Raycast(ray, out raycastHit, 100, _castableMask))
            {
                if (raycastHit.collider != null)
                {
                    Debug.Log("Collider name" + raycastHit.collider.name);

                    lineRenderer.SetPosition(1, raycastHit.collider.transform.position);
                    lineRenderer.gameObject.transform.rotation = Quaternion.Euler(90f, 0f, 0f);
                    _targetOfStartdragBuilding = raycastHit.collider.gameObject.GetComponent<BaseTower>();
                    _currentStartDragPosBuilding.SetTargetTower(raycastHit.collider.gameObject.GetComponent<BaseTower>());
                }

            }
            else
            {
                lineRenderer.positionCount = 0;
                _targetOfStartdragBuilding = null;
            }
        }

    }

    public bool HasTargetPathPlaced()
    {
        return _currentStartDragPosBuilding != null && _targetOfStartdragBuilding != null;
    }
    public Vector3 GetTargetBuildingPos()
    {
        return _targetOfStartdragBuilding.transform.position;
    }

    Vector3 GetMouseWorldPosition3D()
    {
        Ray ray0 = _camera.ScreenPointToRay(Input.mousePosition);
        RaycastHit raycastHit0;
        if (Physics.Raycast(ray0, out raycastHit0))
        {
            if (raycastHit0.collider != null)
            {
                return raycastHit0.point;
            }
        }
        return Vector3.zero;
    }


}
