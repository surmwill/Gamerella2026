using UnityEngine;

public static class BoxCollider2DExtensions
{
    public static int GetOverlappingColliders(
        this BoxCollider2D box, 
        Collider2D[] resultsBuffer, 
        LayerMask layerMask = default,
        bool includeTriggers = true)
    {
        // 1. Calculate world-space center from local offset
        Vector2 worldCenter = box.transform.TransformPoint(box.offset);

        // 2. Account for transform lossy scale in 2D
        Vector3 scale = box.transform.lossyScale;
        Vector2 worldSize = new Vector2(
            box.size.x * Mathf.Abs(scale.x), 
            box.size.y * Mathf.Abs(scale.y)
        );

        // 3. Get the world rotation angle around the Z axis
        float worldAngle = box.transform.eulerAngles.z;

        // 4. Configure the ContactFilter2D struct
        ContactFilter2D contactFilter = new ContactFilter2D();
        contactFilter.useLayerMask = true;
        contactFilter.layerMask = layerMask == 0 ? Physics2D.AllLayers : layerMask;
        contactFilter.useTriggers = includeTriggers;

        // 5. Query the 2D physics world using the ContactFilter OverlapBox overload
        int rawHitCount = Physics2D.OverlapBox(
            worldCenter, 
            worldSize, 
            worldAngle, 
            contactFilter, 
            resultsBuffer
        );

        // 6. Filter out the collider's own GameObject from the results buffer
        int validCount = 0;
        for (int i = 0; i < rawHitCount; i++)
        {
            if (resultsBuffer[i] != box)
            {
                resultsBuffer[validCount++] = resultsBuffer[i];
            }
        }

        return validCount;
    }
}