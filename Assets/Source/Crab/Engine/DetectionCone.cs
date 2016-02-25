
namespace Crab.Debug
{
    using UnityEngine;

    public class DetectionCone
    {
        int quality = 15;
        Mesh mesh;
        public Material material;

        public DetectionCone()
        {
            mesh = new Mesh();
            mesh.vertices = new Vector3[4 * quality];   // Could be of size [2 * quality + 2] if circle segment is continuous
            mesh.triangles = new int[3 * 2 * quality];

            Vector3[] normals = new Vector3[4 * quality];
            Vector2[] uv = new Vector2[4 * quality];

            for (int i = 0; i < uv.Length; i++)
                uv[i] = new Vector2(0, 0);
            for (int i = 0; i < normals.Length; i++)
                normals[i] = new Vector3(0, 1, 0);

            mesh.uv = uv;
            mesh.normals = normals;
        }

        // Draw called once per frame
        public void Draw(Vector3 position, float angle_lookat, float angle_fov = 110, float dist_max = 5.0f, float dist_min = 1.0f)
        {
            float angle_start = angle_lookat - angle_fov/2;
            float angle_end = angle_lookat + angle_fov/2;
            float angle_delta = (angle_end - angle_start) / quality;

            float angle_curr = angle_start;
            float angle_next = angle_start + angle_delta;

            Vector3 pos_curr_min = Vector3.zero;
            Vector3 pos_curr_max = Vector3.zero;

            Vector3 pos_next_min = Vector3.zero;
            Vector3 pos_next_max = Vector3.zero;

            Vector3[] vertices = new Vector3[4 * quality];   // Could be of size [2 * quality + 2] if circle segment is continuous
            int[] triangles = new int[3 * 2 * quality];

            for (int i = 0; i < quality; i++)
            {
                Vector3 sphere_curr = new Vector3(
                Mathf.Sin(Mathf.Deg2Rad * (angle_curr)), 0,   // Left handed CW
                Mathf.Cos(Mathf.Deg2Rad * (angle_curr)));

                Vector3 sphere_next = new Vector3(
                Mathf.Sin(Mathf.Deg2Rad * (angle_next)), 0,
                Mathf.Cos(Mathf.Deg2Rad * (angle_next)));

                pos_curr_min = sphere_curr * dist_min;
                pos_curr_max = sphere_curr * dist_max;

                pos_next_min = sphere_next * dist_min;
                pos_next_max = sphere_next * dist_max;

                int a = 4 * i;
                int b = 4 * i + 1;
                int c = 4 * i + 2;
                int d = 4 * i + 3;

                vertices[a] = pos_curr_min;
                vertices[b] = pos_curr_max;
                vertices[c] = pos_next_max;
                vertices[d] = pos_next_min;

                triangles[6 * i] = a;       // Triangle1: abc
                triangles[6 * i + 1] = b;
                triangles[6 * i + 2] = c;
                triangles[6 * i + 3] = c;   // Triangle2: cda
                triangles[6 * i + 4] = d;
                triangles[6 * i + 5] = a;

                angle_curr += angle_delta;
                angle_next += angle_delta;

            }

            mesh.vertices = vertices;
            mesh.triangles = triangles;


            //Render
            Gizmos.DrawMesh(mesh, position, Quaternion.identity);
        }
    }
}