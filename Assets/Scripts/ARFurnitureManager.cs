using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class ARFurnitureManager : MonoBehaviour
{
    [SerializeField] private ARRaycastManager raycastManager;
    [SerializeField] private GameObject[] furniturePrefabs; // Lista de tus muebles (Silla, Mesa, etc.)

    private GameObject spawnedObject;
    private int currentPrefabIndex = 0;

    void Update()
    {
        // Detectar toque en la pantalla
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            if (touch.phase == TouchPhase.Began)
            {
                PlaceOrMoveObject(touch.position);
            }
        }
        // Detectar clic en el editor (PC)
        else if (Input.GetMouseButtonDown(0))
        {
            // Evitar que el clic atraviese los botones de la UI
            if (!UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject())
            {
                PlaceOrMoveObject(Input.mousePosition);
            }
        }
    }

    void PlaceOrMoveObject(Vector2 touchPosition)
    {
        var rayHits = new List<ARRaycastHit>();
        // Cambiamos a PlaneWithinPolygon para mayor precisión en superficies
        if (raycastManager.Raycast(touchPosition, rayHits, TrackableType.PlaneWithinPolygon))
        {
            Pose hitPose = rayHits[0].pose;

            if (spawnedObject == null)
            {
                // Si no hay objeto, lo crea
                spawnedObject = Instantiate(furniturePrefabs[currentPrefabIndex], hitPose.position, hitPose.rotation);
            }
            else
            {
                // Si ya existe, lo mueve a la nueva posición (Requerimiento: "Ajustar objetos")
                spawnedObject.transform.position = hitPose.position;
            }
        }
    }

    // Método para botones de la UI (Para cambiar entre Silla, Comedor, etc.)
    public void SwitchFurniture(int index)
    {
        currentPrefabIndex = index;
    }

    // Método para cambiar color (Requerimiento de la imagen)
    public void ChangeSelectedColor(Color newColor)
    {
        if (spawnedObject != null)
        {
            // Busca el MeshRenderer en el mueble para cambiarle el color
            var renderer = spawnedObject.GetComponentInChildren<MeshRenderer>();
            if (renderer != null) renderer.material.color = newColor;
        }
    }
}