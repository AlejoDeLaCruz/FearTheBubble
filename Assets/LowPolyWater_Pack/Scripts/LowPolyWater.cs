using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LowPolyWater
{
    public class LowPolyWaterClass : MonoBehaviour
    {
        public float waveHeight = 0.5f;
        public float waveFrequency = 0.5f;
        public float waveLength = 0.75f;

        //Position where the waves originate from
        public Vector3 waveOriginPosition = new Vector3(0.0f, 0.0f, 0.0f);

        MeshFilter meshFilter;
        Mesh mesh;
        Vector3[] vertices;

        private void Awake()
        {
            //Get the Mesh Filter of the gameobject
            meshFilter = GetComponent<MeshFilter>();
        }

        void Start()
        {
            CreateMeshLowPoly(meshFilter);
        }

        /// <summary>
        /// Rearranges the mesh vertices to create a 'low poly' effect
        /// </summary>
        /// <param name="mf">Mesh filter of gamobject</param>
        /// <returns></returns>
        MeshFilter CreateMeshLowPoly(MeshFilter mf)
        {
            mesh = mf.sharedMesh;

            //Get the original vertices of the gameobject's mesh
            Vector3[] originalVertices = mesh.vertices;

            //Get the list of triangle indices of the gameobject's mesh
            int[] triangles = mesh.triangles;

            //Create a vector array for new vertices 
            Vector3[] vertices = new Vector3[triangles.Length];
            
            //Assign vertices to create triangles out of the mesh
            for (int i = 0; i < triangles.Length; i++)
            {
                vertices[i] = originalVertices[triangles[i]];
                triangles[i] = i;
            }

            //Update the gameobject's mesh with new vertices
            mesh.vertices = vertices;
            mesh.SetTriangles(triangles, 0);
            mesh.RecalculateBounds();
            mesh.RecalculateNormals();
            this.vertices = mesh.vertices;

            return mf;
        }
        
        void Update()
        {
            GenerateWaves();
        }

        /// <summary>
        /// Based on the specified wave height and frequency, generate 
        /// wave motion originating from waveOriginPosition
        /// </summary>
        void GenerateWaves()
        {
            for (int i = 0; i < vertices.Length; i++)
            {
                Vector3 v = vertices[i];

                //Initially set the wave height to 0
                v.y = 0.0f;

                //Get the distance between wave origin position and the current vertex
                float distance = Vector3.Distance(v, waveOriginPosition);
                distance = (distance % waveLength) / waveLength;

                //Oscilate the wave height via sine to create a wave effect
                v.y = waveHeight * Mathf.Sin(Time.time * Mathf.PI * 2.0f * waveFrequency
                + (Mathf.PI * 2.0f * distance));
                
                //Update the vertex
                vertices[i] = v;
            }

            //Update the mesh properties
            mesh.vertices = vertices;
            mesh.RecalculateNormals();
            mesh.MarkDynamic();
            meshFilter.mesh = mesh;
        }
        // --- NUEVO MÉTODO PARA OBTENER LA ALTURA DE LA OLA EN UNA POSICIÓN DEL MUNDO ---
        public float GetWaveHeightAtPosition(Vector3 worldPosition)
        {
            // Convertir la posición del mundo a posición local respecto al objeto de agua
            Vector3 localPos = transform.InverseTransformPoint(worldPosition);

            // Calcular la distancia desde el origen de las olas (en espacio local)
            float distance = Vector3.Distance(localPos, waveOriginPosition);
            distance = (distance % waveLength) / waveLength;

            // Calcular la altura de la ola (offset) en la posición local
            float waveOffset = waveHeight * Mathf.Sin(Time.time * Mathf.PI * 2.0f * waveFrequency
                                    + (Mathf.PI * 2.0f * distance));

            // Retornar la altura total sumando la posición Y del objeto de agua (que actúa como base)
            return transform.position.y + waveOffset;
        }
    }
}