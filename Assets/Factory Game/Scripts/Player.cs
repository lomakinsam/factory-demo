using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Unit
{
    [SerializeField] private Camera gameCamera;

    private Vector3? DestinationPoint
    {
        get 
        {
            var ray = gameCamera.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit))
                return hit.point;
            else
                return null;
        }
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
            HandleAction();
    }

    private void HandleAction()
    {
        Vector3? destinationPoint = DestinationPoint;

        if (destinationPoint != null)
            MoveTo((Vector3)destinationPoint);
    }
}
