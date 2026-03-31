#if UNITY_EDITOR
using UnityEngine;
using UnityEngine.AI;
using System.Collections.Generic;

public class NavMeshProbePlacer : MonoBehaviour
{
    [ContextMenu("Generate Probes from NavMesh")]
    public void Generate()
    {
        NavMeshTriangulation navData = NavMesh.CalculateTriangulation();
        LightProbeGroup group = GetComponent<LightProbeGroup>();
        if (group == null) group = gameObject.AddComponent<LightProbeGroup>();

        List<Vector3> positions = new List<Vector3>();
        foreach (Vector3 vert in navData.vertices)
        {
            // Add a probe at the floor level
            positions.Add(vert + Vector3.up * 0.5f);
            // Add a probe at head level (to create the 3D volume WebGL needs)
            positions.Add(vert + Vector3.up * 2.5f);
        }

        group.probePositions = positions.ToArray();
        Debug.Log($"Generated {positions.Count} probes on NavMesh!");
    }
}
#endif
