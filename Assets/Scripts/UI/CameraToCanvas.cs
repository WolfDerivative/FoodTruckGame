using UnityEngine;


/// <summary>
///  Assign main camera of the scene to the Canvas' Render Camera property.
///  
///  Note: This script should will probably carried over between scenes with
///  DontDestroy() call, therefore - use OnEnable() instead of Start() for the 
///  main logic.
/// </summary>
[RequireComponent(typeof(Canvas))]
public class CameraToCanvas : MonoBehaviour {

    protected Canvas _canvas;

    public void OnEnable() {
        if (_canvas == null)
            _canvas = GetComponent<Canvas>();
        if (_canvas.worldCamera != null)
            return;
        _canvas.worldCamera = Camera.main;
    }//OnEnable

}//class
