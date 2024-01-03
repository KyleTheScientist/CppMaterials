using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using UnityEngine;
using BepInEx;

namespace CPPMaterials
{

    public class MeshToFile
    {
        public static void Write(SkinnedMeshRenderer skinnedMeshRenderer)
        {
            if (skinnedMeshRenderer == null)
            {
                Debug.LogError("SkinnedMeshRenderer is not assigned.");
                return;
            }

            Mesh mesh = new Mesh();
            skinnedMeshRenderer.BakeMesh(mesh);

            using (StreamWriter sw = new StreamWriter(Paths.PluginPath + "/mesh.obj"))
            {
                sw.Write(MeshToString(mesh));
            }
        }

        public static string MeshToString(Mesh mesh)
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder();

            foreach (Vector3 v in mesh.vertices)
            {
                sb.Append(string.Format("v {0} {1} {2}\n", v.x, v.y, v.z));
            }

            sb.Append("\n");

            foreach (Vector3 v in mesh.normals)
            {
                sb.Append(string.Format("vn {0} {1} {2}\n", v.x, v.y, v.z));
            }

            sb.Append("\n");

            foreach (Vector3 v in mesh.uv)
            {
                sb.Append(string.Format("vt {0} {1}\n", v.x, v.y));
            }

            for (int i = 0; i < mesh.subMeshCount; i++)
            {
                sb.Append("\n");

                int[] triangles = mesh.GetTriangles(i);
                for (int j = 0; j < triangles.Length; j += 3)
                {
                    sb.Append(string.Format("f {0}/{0}/{0} {1}/{1}/{1} {2}/{2}/{2}\n",
                        triangles[j] + 1, triangles[j + 1] + 1, triangles[j + 2] + 1));
                }
            }

            return sb.ToString();
        }
    }

}
