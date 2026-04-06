using UnityEngine;

public class FurnitureController : MonoBehaviour
{
    public bool isSelected = false; // Solo si es true, responderá a los gestos

    private Touch touch;
    private Quaternion rotationY;
    private float rotateSpeedModifier = 0.1f;
    private float scaleSpeedModifier = 0.01f;

    void Update()
    {
        // SI NO ESTÁ SELECCIONADO, NO HACE NADA
        if (!isSelected) return;

        // --- ROTACIÓN CON UN DEDO ---
        if (Input.touchCount == 1)
        {
            touch = Input.GetTouch(0);
            if (touch.phase == TouchPhase.Moved)
            {
                rotationY = Quaternion.Euler(0f, -touch.deltaPosition.x * rotateSpeedModifier, 0f);
                transform.rotation = rotationY * transform.rotation;
            }
        }

        // --- ESCALA CON DOS DEDOS ---
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
            newScale = Mathf.Clamp(newScale, 0.5f, 3.0f);
            transform.localScale = new Vector3(newScale, newScale, newScale);
        }

        // --- PRUEBA EN PC ---
        float horizontalInput = Input.GetAxis("Horizontal");
        if (horizontalInput != 0)
        {
            transform.Rotate(Vector3.up, -horizontalInput * 100f * Time.deltaTime);
        }

        float scroll = Input.GetAxis("Mouse ScrollWheel");
        if (scroll != 0f)
        {
            float newScale = transform.localScale.x + (scroll * 0.5f);
            newScale = Mathf.Clamp(newScale, 0.5f, 3.0f);
            transform.localScale = new Vector3(newScale, newScale, newScale);
        }
    }

    public void ChangeColor(string hexColor)
    {
        Color newCol;
        if (ColorUtility.TryParseHtmlString(hexColor, out newCol))
        {
            Renderer[] renderers = GetComponentsInChildren<Renderer>();
            foreach (Renderer r in renderers)
            {
                r.material.color = newCol;
            }
        }
    }
}