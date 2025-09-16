        using UnityEngine;
        using System.Collections;

        public class InvertObjectNormals : MonoBehaviour
        {
            void Start()
            {
                FlipNormalsAndTriangles();
            }

            void FlipNormalsAndTriangles()
            {
                MeshFilter mf = GetComponent<MeshFilter>();
                if (mf == null || mf.sharedMesh == null)
                {
                    Debug.LogError("MeshFilter or Mesh is missing!");
                    return;
                }

                Mesh mesh = mf.sharedMesh;
                Vector3[] normals = mesh.normals;
                int[] triangles = mesh.triangles;

                // Invert Normals
                for (int i = 0; i < normals.Length; i++)
                {
                    normals[i] = -normals[i];
                }
                mesh.normals = normals;

                // Reverse Triangles (to face the correct way after normal inversion)
                for (int i = 0; i < triangles.Length; i += 3)
                {
                    int temp = triangles[i];
                    triangles[i] = triangles[i + 2];
                    triangles[i + 2] = temp;
                }
                mesh.triangles = triangles;

                mesh.RecalculateBounds(); // Important to ensure correct bounds
            }
        }