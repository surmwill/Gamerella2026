using UnityEngine;

public static class CircleCollider2DExtensions
{
    public static int GetOverlappingColliders(
        this CircleCollider2D circle, 
        Collider2D[] resultsBuffer, 
        LayerMask layerMask = default,
        bool includeTriggers = true)
    {
        // 1. Calculate world-space center from local offset
        Vector2 worldCenter = circle.transform.TransformPoint(circle.offset);

        // 2. Account for transform lossy scale in 2D (taking the max axis for safety)
        Vector3 scale = circle.transform.lossyScale;
        float maxScale = Mathf.Max(Mathf.Abs(scale.x), Mathf.Abs(scale.y));
        float worldRadius = circle.radius * maxScale;

        // 3. Configure the ContactFilter2D struct
        ContactFilter2D contactFilter = new ContactFilter2D();
        contactFilter.useLayerMask = true;
        contactFilter.layerMask = layerMask == 0 ? Physics2D.AllLayers : layerMask;
        contactFilter.useTriggers = includeTriggers;

        // 4. Query the 2D physics world using the ContactFilter overload
        int rawHitCount = Physics2D.OverlapCircle(
            worldCenter, 
            worldRadius, 
            contactFilter, 
            resultsBuffer
        );

        // 4. Filter out the collider's own GameObject from the results buffer
        int validCount = 0;
        for (int i = 0; i < rawHitCount; i++)
        {
            if (resultsBuffer[i] != circle)
            {
                resultsBuffer[validCount++] = resultsBuffer[i];
            }
        }

        return validCount;
    }
}