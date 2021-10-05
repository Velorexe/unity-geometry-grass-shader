using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class GrassDisplacementCamera : MonoBehaviour
{  
    [SerializeField]
    private Camera _camera;

    void Update()
    {
        Vector3 position = transform.position;

        position.x /= _camera.orthographicSize * 2;
        position.z /= _camera.orthographicSize * 2;

        Shader.SetGlobalVector("_DisplacementLocation", position);
    }
}
