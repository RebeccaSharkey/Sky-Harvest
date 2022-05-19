using System;
using UnityEngine;

/// <summary>
/// Script to move a model around with the mouse
/// </summary>
public class ModelViewer : MonoBehaviour
{
   [SerializeField] private GameObject modelToView;
   [SerializeField] private Vector3 spawnOffset;

   [SerializeField] private Vector3 zoomAmount;
   [SerializeField] private int numOfZooms;
   private int zoomIndex = 0;
   
   [SerializeField] private Camera mainCam;
   private GameObject model;

   private Vector3 previousPos;
   private Camera cam;

   private bool bCanInspect = false;
   
   private void Start()
   {
      cam = GetComponent<Camera>();
   }
   
   private void LateUpdate()
   {
      MouseInput();
   }

   private void MouseInput()
   {
      if (Input.GetMouseButtonDown(0) && bCanInspect)
      {
         previousPos = cam.ScreenToViewportPoint(Input.mousePosition);
      }
      
      //Rotates the model relative to the direction between drag pos and the mouse pos
      if (Input.GetMouseButton(0) && bCanInspect)
      {
         Vector3 dir = previousPos - cam.ScreenToViewportPoint(Input.mousePosition);
         model.transform.Rotate(Vector3.right, -dir.y * 180f);
         model.transform.Rotate(Vector3.up, dir.x * 180f, Space.World);
         //model.transform.Translate(new Vector3(0f, 0f, -dragDistance.z));

         previousPos = cam.ScreenToViewportPoint(Input.mousePosition);
      }
      
      if (Input.GetAxisRaw("Mouse ScrollWheel") > 0f && zoomIndex < numOfZooms && bCanInspect)
      {
         transform.position += zoomAmount;
         zoomIndex++;
      }
      else if (Input.GetAxisRaw("Mouse ScrollWheel") < 0f && zoomIndex > 0 && bCanInspect)
      {
         transform.position -= zoomAmount;
         zoomIndex--;
      }
   }

   public void ChangeViewer(bool state)
   {
      if (state)
      {
         mainCam.enabled = false;
         cam.enabled = true;
         model = Instantiate(modelToView, transform.position + spawnOffset, transform.rotation);

      }
      else
      {
         Destroy(model);
         model = null;
         cam.enabled = false;
         mainCam.enabled = true;
      }
      
   }
   
   private void ChangeModelToView(GameObject newModel)
   {
      modelToView = newModel;
   }

   public void ChangeAllowInspector(bool state)
   {
      bCanInspect = state;
   }

   private void OnEnable()
   {
      CustomEvents.ModelViewer.OnChangeModel += ChangeModelToView;
      CustomEvents.ModelViewer.OnAllowInspector += ChangeAllowInspector;
   }

   private void OnDisable()
   {
      CustomEvents.ModelViewer.OnChangeModel -= ChangeModelToView;
      CustomEvents.ModelViewer.OnAllowInspector -= ChangeAllowInspector;
   }
}
