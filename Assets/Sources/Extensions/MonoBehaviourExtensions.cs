using UnityEngine;

public static class MonoBehaviourExtensions
{
    public static void UseMeshFilterFor(this MonoBehaviour behaviour, Mesh mesh)
    {
        var filter = behaviour.GetComponent<MeshFilter>();
        filter.mesh = mesh;
    }

    public static void UseMeshColliderFor(this MonoBehaviour behaviour, Mesh mesh)
    {
        if (behaviour.TryGetComponent<MeshCollider>(out var collider))
        {
            collider.sharedMesh = mesh;
        }
    }
}