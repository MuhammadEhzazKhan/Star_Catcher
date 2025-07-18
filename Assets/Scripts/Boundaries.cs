using UnityEngine;

public class Boundaries : MonoBehaviour
{
    private Camera mainCamera;
    private SpriteRenderer spriteRenderer;
    private float halfWidth;

    void Start()
    {
        mainCamera = Camera.main;

        // Get sprite renderer for width
        spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer == null)
        {
            Debug.LogError("No SpriteRenderer found on this object.");
            return;
        }

        halfWidth = spriteRenderer.bounds.extents.x;
    }

    void LateUpdate()
    {
        if (mainCamera == null || spriteRenderer == null)
            return;

        Vector3 pos = transform.position;

        float screenLeft = mainCamera.ViewportToWorldPoint(new Vector3(0, 0, 0)).x + halfWidth;
        float screenRight = mainCamera.ViewportToWorldPoint(new Vector3(1, 0, 0)).x - halfWidth;

        // Clamp only X for side-to-side movement
        pos.x = Mathf.Clamp(pos.x, screenLeft, screenRight);

        transform.position = pos;
    }
}
