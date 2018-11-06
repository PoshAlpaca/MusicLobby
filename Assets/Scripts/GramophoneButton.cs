using UnityEngine;
using VRTK;

[RequireComponent(typeof(VRTK_InteractableObject))]
public class GramophoneButton : MonoBehaviour {
    VRTK_InteractableObject interactableObject;
    public Gramophone gramophone;

    void OnEnable() {
        if (interactableObject == null) {
            interactableObject = GetComponent<VRTK_InteractableObject>();
        }

        // Register gramophone Used and Unused as delegate functions, so that they get activated when the user clicks.
        interactableObject.InteractableObjectUsed += gramophone.Used;
        interactableObject.InteractableObjectUnused += gramophone.Unused;
    }

    void OnDisable() {
        // Unregister our delegate functions.
        interactableObject.InteractableObjectUsed -= gramophone.Used;
        interactableObject.InteractableObjectUnused -= gramophone.Unused;
	}
}
