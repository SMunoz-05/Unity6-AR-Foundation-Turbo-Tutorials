using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class ARFurnitureManager : MonoBehaviour
{
    [SerializeField] private ARRaycastManager raycastManager;
    [SerializeField] private GameObject[] furniturePrefabs;

    // Ya no es privada una sola, ahora simplemente creamos nuevos
    private GameObject lastSpawnedObject;
    private int currentPrefabIndex = 0;

    void Update()
    {
        // Toque en celular
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            if (touch.phase == TouchPhase.Began)
            {
                if (!UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject(touch.fingerId))
                {
                    PlaceNewObject(touch.position);
                }
            }
        }
        // Clic en PC (Simulador)
        else if (Input.GetMouseButtonDown(0))
        {
            if (!UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject())
            {
                PlaceNewObject(Input.mousePosition);
            }
        }
    }

    void PlaceNewObject(Vector2 touchPosition)
    {
        var rayHits = new List<ARRaycastHit>();
        if (raycastManager.Raycast(touchPosition, rayHits, TrackableType.PlaneWithinPolygon))
        {
            Pose hitPose = rayHits[0].pose;

            // 1. DESELECCIONAR TODOS LOS ANTERIORES
            FurnitureController[] allFurniture = Object.FindObjectsByType<FurnitureController>(FindObjectsSortMode.None);
            foreach (FurnitureController f in allFurniture)
            {
                f.isSelected = false;
            }

            // 2. INSTANCIAR EL NUEVO
            lastSpawnedObject = Instantiate(furniturePrefabs[currentPrefabIndex], hitPose.position, hitPose.rotation);

            // 3. SELECCIONAR SOLO EL NUEVO
            FurnitureController newController = lastSpawnedObject.GetComponent<FurnitureController>();
            if (newController != null)
            {
                newController.isSelected = true;
            }

            Debug.Log("Nuevo objeto instanciado y seleccionado: " + lastSpawnedObject.name);
        }
    }

    public void SwitchFurniture(int index)
    {
        currentPrefabIndex = index;
        Debug.Log("Mueble seleccionado para la PRÓXIMA instancia: " + index);
    }

    // El cambio de color ahora afectará al ÚLTIMO objeto que pusiste
    public void SetColorToSpawned(string hex)
    {
        if (lastSpawnedObject != null)
        {
            var controller = lastSpawnedObject.GetComponent<FurnitureController>();
            if (controller != null) controller.ChangeColor(hex);
        }
    }
    public void ClearAllFurniture()
    {
        // Busca todos los objetos con el script de control y los borra
        FurnitureController[] allFurniture = Object.FindObjectsByType<FurnitureController>(FindObjectsSortMode.None);
        foreach (FurnitureController f in allFurniture)
        {
            Destroy(f.gameObject);
        }
    }
    public void DeleteLast()
    {
        if (lastSpawnedObject != null)
        {
            Destroy(lastSpawnedObject);
        }
    }
    public void TogglePlanes(bool show)
    {
        foreach (var plane in GetComponent<ARPlaneManager>().trackables)
        {
            plane.gameObject.SetActive(show);
        }
    }
}