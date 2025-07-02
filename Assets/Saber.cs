using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EzySlice;
public class Saber : MonoBehaviour
{
    public LayerMask layer;
    private Vector3 previousPos;

    public Scoring score;

    public Material crossSectionMaterial;

    public float cutForce = 1000;

    public Transform startSlicePoint;
    public Transform endSlicePoint;

    public VelocityEstimator velocityEstimator;

    // Start is called before the first frame update
    void Start()
    {
        // previousPos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        // float maxDistance = lineVisual.GetPosition(1).magnitude;
        float maxDistance = 1;
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.forward, out hit, maxDistance, layer))
        {
            Vector3 hitPoint = transform.position + transform.forward * hit.distance;
            if (Vector3.Angle(hitPoint - previousPos, hit.transform.up) > 130)
            {
                Destroy(hit.transform.gameObject);
                score.AddScore(100);
                Debug.Log("Hit");
                Slice(hit.transform.gameObject);
            }
            // Destroy(hit.transform.gameObject);
            // Debug.Log("Hit");

        }
        previousPos = transform.position + transform.forward * hit.distance;
    }

    public void Slice(GameObject target)
    {
        Vector3 velocity = velocityEstimator.GetVelocityEstimate();
        Vector3 planeNormal = Vector3.Cross(endSlicePoint.position - startSlicePoint.position, velocity);
        planeNormal.Normalize();
        SlicedHull hull = target.Slice(endSlicePoint.position, planeNormal);
        if (hull != null)
        {
            GameObject upperHull = hull.CreateUpperHull(target, crossSectionMaterial);
            GameObject lowerHull = hull.CreateLowerHull(target, crossSectionMaterial);

            // Preserve the position and rotation of the original object
            upperHull.transform.position = target.transform.position;
            upperHull.transform.rotation = target.transform.rotation;
            lowerHull.transform.position = target.transform.position;
            lowerHull.transform.rotation = target.transform.rotation;

            SetupSlicedComponent(upperHull);
            SetupSlicedComponent(lowerHull);
            Destroy(target);
        }
    }

    public void SetupSlicedComponent(GameObject slicedObject)
    {
        Rigidbody rb = slicedObject.AddComponent<Rigidbody>();
        MeshCollider collider = slicedObject.AddComponent<MeshCollider>();
        collider.convex = true;
        rb.AddExplosionForce(cutForce, slicedObject.transform.position, 1);
    }
}