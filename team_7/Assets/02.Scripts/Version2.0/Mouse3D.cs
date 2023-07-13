using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mouse3D : MonoBehaviour
{
    public static Mouse3D Instance { get; private set; }

    [SerializeField] private LayerMask mouseColliderLayerMask = new LayerMask();

    private void Awake()
    {
        Instance = this;
    }

    private void Update()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if(Physics.Raycast(ray, out RaycastHit raycastHit, 1000.0f, mouseColliderLayerMask))
        {
            transform.position = raycastHit.point;
        }
    }
    public static Vector3 GetMouseWorldPosition()
    {
        if(Instance == null)
        {
            Debug.LogError("Mouse3D Object dose not exist ");
        }

        return Instance.GetMouseworldPosition_Instance();
    }

    private Vector3 GetMouseworldPosition_Instance()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if(Physics.Raycast(ray, out RaycastHit raycastHit, 1000.0f, mouseColliderLayerMask))
        {
            return raycastHit.point;
        }
        else
        {
            return Vector3.zero;
        }
    }


}
