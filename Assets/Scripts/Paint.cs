using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRTK;

[RequireComponent(typeof(VRTK_InteractableObject))]
public class Paint : MonoBehaviour {
    public Material material;
    public float tolerance = 1;

    VRTK_InteractableObject interactableObject;
    LineRenderer lineRenderer;
    bool painting;

    GameObject currentLine;
    List<GameObject> lines = new List<GameObject>();

    void OnEnable() {
        // Save resources by getting components only once
        if (interactableObject == null) {
            interactableObject = GetComponent<VRTK_InteractableObject>();
        }

        // Register our delegate functions
        interactableObject.InteractableObjectUsed += InteractableObjectUsed;
        interactableObject.InteractableObjectUnused += InteractableObjectUnused;
    }

    void OnDisable() {
        // Unregister our delegate functions
        interactableObject.InteractableObjectUsed -= InteractableObjectUsed;
        interactableObject.InteractableObjectUnused -= InteractableObjectUnused;
    }

	void Start () {
	}
	
	void Update () {
        if (painting == false) {
            return;
        }

        // Make sure we have a line renderer to work with
        if (lineRenderer == null) {
            lineRenderer = currentLine.GetComponent<LineRenderer>();
        }

        // Add current position of brush's tip to the lineRenderer's points
        lineRenderer.positionCount += 1;
        lineRenderer.SetPosition(lineRenderer.positionCount - 1, this.transform.position + new Vector3(0, 0.25f, 0));
	}

    void InteractableObjectUsed (object sender, InteractableObjectEventArgs e) {
        painting = true;
        CreateLine();
    }

    void InteractableObjectUnused (object sender, InteractableObjectEventArgs e) {
        painting = false;

        if (lineRenderer == null) {
            lineRenderer = currentLine.GetComponent<LineRenderer>();
        }

        // Reduce the amount of points in the line to improve performance
        lineRenderer.Simplify(tolerance);
    }

    void CreateLine () {
        // https://docs.unity3d.com/ScriptReference/LineRenderer.SetPositions.html
        
        // Create new empty gameObject and give it a line renderer component
        // We need a new gameObject for each individual line (otherwise we'd get one continuous line)
        currentLine = new GameObject("3D line");
        currentLine.transform.position = this.transform.position + new Vector3(0, 0.25f, 0);

        lineRenderer = currentLine.AddComponent<LineRenderer>();
        lineRenderer.material = material;
        lineRenderer.widthMultiplier = 0.2f;
        lineRenderer.positionCount = 0;
        lineRenderer.numCapVertices = 5;

        lines.Add(currentLine);
    }
}
