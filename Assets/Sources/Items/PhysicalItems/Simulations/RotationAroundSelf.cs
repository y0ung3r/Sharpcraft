using UnityEngine;

[RequireComponent(typeof(PhysicalItem))]
public class RotationAroundSelf : MonoBehaviour
{
    [SerializeField]
    [Range(0.0f, 35.0f)]
    private float _rotationAngle = 35.0f;

    private MeshRenderer _meshRenderer;

    public void Awake()
    {
        _meshRenderer = GetComponent<MeshRenderer>();
    }

    public void FixedUpdate()
        => transform.RotateAround(_meshRenderer.bounds.center, Vector3.up, _rotationAngle * Time.fixedDeltaTime);
}
