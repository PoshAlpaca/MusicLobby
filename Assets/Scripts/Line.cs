using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRTK;

[RequireComponent(typeof(VRTK_InteractableObject))]
public class Line : MonoBehaviour {
    float tolerance;
    LineRenderer lineRenderer;

    Line(Vector3 position, Material material, float tolerance) {
        tolerance = tolerance;

        // https://docs.unity3d.com/ScriptReference/LineRenderer.SetPositions.html
        
        // Create new empty gameObject and give it a line renderer component
        // We need a new gameObject for each individual line (otherwise we'd get one continuous line)
        currentLine = new GameObject("3D line");
        currentLine.transform.position = position;

        lineRenderer = this.AddComponent<LineRenderer>();
        lineRenderer.material = material;
        lineRenderer.widthMultiplier = 0.2f;
        lineRenderer.positionCount = 0;
        lineRenderer.numCapVertices = 5;
    }

    void Draw (Vector3 position) {
        // Add current position of brush's tip to the lineRenderer's points
        lineRenderer.positionCount += 1;
        lineRenderer.SetPosition(lineRenderer.positionCount - 1, position);
    }

    void StopDrawing () {
        // Reduce the amount of points in the line to improve performance
        lineRenderer.Simplify(tolerance);
    }
}