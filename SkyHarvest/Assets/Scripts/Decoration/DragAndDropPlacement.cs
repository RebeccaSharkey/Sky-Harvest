using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragAndDropPlacement : MonoBehaviour
{
    [SerializeField] private float yPos = 30f;
    [SerializeField] private GameObject draggedObject;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private Transform placedFurnitureParent;
    private bool isPlacing = false;
    private Camera mainCam;

    private void Start()
    {
        mainCam = Camera.main;
    }

    public void Test()
    {
        PickUpObject(draggedObject);
    }

    public void SpawnObject(string _resourcePath)
    {
        draggedObject = Instantiate(Resources.Load($"Prefabs/Furniture/{_resourcePath}", typeof(GameObject)) as GameObject, transform.position, Quaternion.Euler(0f, 0f, transform.rotation.z), transform);
        isPlacing = true;
        StartCoroutine(Placing());
    }

    public void PickUpObject(GameObject _object)
    {
        draggedObject = _object;
        isPlacing = true;
        StartCoroutine(Placing());
    }

    private IEnumerator Placing()
    {
        CustomEvents.PlayerControls.OnRemovePlayerControl?.Invoke(false);
        while(isPlacing)
        {
            Ray ray = mainCam.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, groundLayer))
            {
                draggedObject.transform.position = new Vector3(hit.point.x, yPos, hit.point.z);
                if (Input.GetMouseButtonDown(0)) PlaceObject();
            }
            
            yield return null;
        }
        CustomEvents.PlayerControls.OnRemovePlayerControl?.Invoke(true);
    }

    private void PlaceObject()
    {
        if(draggedObject.GetComponent<PlaceableObject>().IsPlaceable())
        {
            isPlacing = false;
            draggedObject.transform.parent = placedFurnitureParent;
            draggedObject = null;
        }
        else
        {
            Debug.Log("Blocked from placing");
            //can't place
            return;
        }
    }
}
