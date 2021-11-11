using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class GrassDisplacementObject : MonoBehaviour
{
    [SerializeField]
    private GameObject _displacementTextureObject;

    [SerializeField]
    private LayerMask _grassLayer;

    [SerializeField]
    private float _maxDistance = 2f;

    private MeshRenderer _displacementRenderer;
    private MaterialPropertyBlock _propertyBlock;

    private int _transparencyGuid;

    private Ray _cachedRay;

    private void Awake()
    {
        if (_displacementTextureObject == null)
            Debug.LogWarning("No Displacement Object has been set.");
        else
        {
            _propertyBlock = new MaterialPropertyBlock();

            _displacementRenderer = _displacementTextureObject.GetComponent<MeshRenderer>();
            _displacementRenderer.GetPropertyBlock(_propertyBlock);

            _cachedRay = new Ray(this.transform.position, Vector3.down);

            _transparencyGuid = Shader.PropertyToID("_Transparency");
        }
    }

    private void Update()
    {
        _cachedRay.origin = this.transform.position;

#if UNITY_EDITOR
        Awake();
#endif

        if (Physics.Raycast(_cachedRay, out RaycastHit hit, _maxDistance, _grassLayer))
        {
            _propertyBlock.SetFloat(_transparencyGuid, hit.distance / _maxDistance);
            _displacementRenderer.SetPropertyBlock(_propertyBlock);
        }
        else
        {
            _propertyBlock.SetFloat(_transparencyGuid, 1);
            _displacementRenderer.SetPropertyBlock(_propertyBlock);
        }

        //Fix X and Z rotations
        _displacementTextureObject.transform.rotation = Quaternion.Euler(0f, _displacementTextureObject.transform.rotation.eulerAngles.y, 0f);
    }
}
