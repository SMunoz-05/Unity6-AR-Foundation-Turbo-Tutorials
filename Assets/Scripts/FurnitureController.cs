using UnityEngine;

public class FurnitureController : MonoBehaviour
{
    private Touch touch;
    private Quaternion rotationY;
    private float rotateSpeedModifier = 0.1f;

    // Para Escala
    private float scaleSpeedModifier = 0.01f;

    void Update()
    {
        // --- ROTACIÓN CON UN DEDO (MOVIMIENTO HORIZONTAL) ---
        if (Input.touchCount == 1)
        {
            touch = Input.GetTouch(0);
            if (touch.phase == TouchPhase.Moved)
            {
                rotationY = Quaternion.Euler(0f, -touch.deltaPosition.x * rotateSpeedModifier, 0f);
                transform.rotation = rotationY * transform.rotation;
            }
        }

        // --- ESCALA CON DOS DEDOS (PINCH / PINZA) ---
        if (Input.touchCount == 2)
        {
            Touch touchZero = Input.GetTouch(0);
            Touch touchOne = Input.GetTouch(1);

            Vector2 touchZeroPrevPos = touchZero.position - touchZero.deltaPosition;
            Vector2 touchOnePrevPos = touchOne.position - touchOne.deltaPosition;

            float prevMagnitude = (touchZeroPrevPos - touchOnePrevPos).magnitude;
            float currentMagnitude = (touchZero.position - touchOne.position).magnitude;

            float difference = currentMagnitude - prevMagnitude;

            float newScale = transform.localScale.x + (difference * scaleSpeedModifier);
            newScale = Mathf.Clamp(newScale, 0.5f, 3.0f); // Límites de tamaño
            transform.localScale = new Vector3(newScale, newScale, newScale);
        }

        // --- PRUEBA EN PC (EDITOR) ---
        if (Input.GetMouseButton(1)) // Clic derecho para rotar en PC
        {
            transform.Rotate(Vector3.up, -Input.GetAxis("Mouse X") * 5f);
        }
    }

    // MÉTODO PARA EL COLOR (Requerimiento de la imagen)
    public void ChangeColor(string hexColor)
    {
        Color newCol;
        if (ColorUtility.TryParseHtmlString(hexColor, out newCol))
        {
            // Busca en el mueble y sus hijos el material
            Renderer[] renderers = GetComponentsInChildren<Renderer>();
            foreach (Renderer r in renderers)
            {
                r.material.color = newCol;
            }
        }
    }
}