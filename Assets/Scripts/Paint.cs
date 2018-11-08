using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRTK;

[RequireComponent(typeof(VRTK_InteractableObject))]
public class Paint : MonoBehaviour {
    
    public Material material;
    public float tolerance = 0.1;
    VRTK_InteractableObject interactableObject;
    bool painting;

    public Transform linePrefab;

    List<GameObject> lines = new List<GameObject>();

    void OnEnable() {
        // Save resources by getting components only once
        if (interactableObject == null) {
            interactableObject = GetComponent<VRTK_InteractableObject>();
        }

        // Register our delegate functions
        interactableObject.InteractableObjectUsed += BrushUsed;
        interactableObject.InteractableObjectUnused += BrushUnused;
    }

    void OnDisable() {
        // Unregister our delegate functions
        interactableObject.InteractableObjectUsed -= BrushUsed;
        interactableObject.InteractableObjectUnused -= BrushUnused;
    }
	
	void Update () {
        if (painting == false) {
            return;
        }

        line.Draw(this.transform.position + new Vector3(0, 0.25f, 0));
	}

    void BrushUsed (object sender, InteractableObjectEventArgs e) {
        painting = true;
        line = Instantiate(linePrefab, this.transform.position + new Vector3(0, 0.25f, 0), Quaternion.identity);
        lines.Add(line);
    }

    void BrushUnused (object sender, InteractableObjectEventArgs e) {
        painting = false;

        if (lineRenderer == null) {
            lineRenderer = currentLine.GetComponent<LineRenderer>();
        }

        line.StopDrawing();
    } 
}
