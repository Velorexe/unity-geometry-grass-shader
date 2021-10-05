﻿using System.Collections;
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
        }
    }

    private void FixedUpdate()
    {
        _cachedRay.origin = this.transform.position;

        if(Physics.Raycast(_cachedRay, out RaycastHit hit, _maxDistance, _grassLayer))
        {
            _propertyBlock.SetFloat("_Transparency", hit.distance / _maxDistance);
            _displacementRenderer.SetPropertyBlock(_propertyBlock);
        }
    }
}