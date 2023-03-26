using UnityEngine;

public class RotateOrbitCenterWithMouse : MonoBehaviour
{
    [SerializeField] private float rotationSpeed = 200f;
    [SerializeField] private float smoothness = 5f;
    private Vector3 previousMousePosition;
    private Vector3 targetRotationSpeed;
    private Vector3 currentRotationSpeed;
    private bool isDragging = false;

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            previousMousePosition = Input.mousePosition;
            isDragging = true;
        }

        if (Input.GetMouseButtonUp(0))
        {
            isDragging = false;
        }

        if (isDragging)
        {
            Vector3 mouseDelta = Input.mousePosition - previousMousePosition;
            //targetRotationSpeed = new Vector3(mouseDelta.y, -mouseDelta.x, 0) * rotationSpeed;
            targetRotationSpeed = new Vector3(-mouseDelta.y, mouseDelta.x, 0) * rotationSpeed;
            previousMousePosition = Input.mousePosition;
        }
        else
        {
            targetRotationSpeed = Vector3.zero;
        }

        currentRotationSpeed = Vector3.Lerp(currentRotationSpeed, targetRotationSpeed, Time.deltaTime * smoothness);
        transform.Rotate(currentRotationSpeed * Time.deltaTime, Space.World);
    }
}
