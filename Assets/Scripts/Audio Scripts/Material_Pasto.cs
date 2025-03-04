using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AK.Wwise;

// MaterialIdentifier.cs
using UnityEngine;

[DisallowMultipleComponent]
public class MaterialIdentifier : MonoBehaviour
{
    [Tooltip("Selecciona el material de la superficie")]
    public SurfaceMaterial surfaceMaterial;
}

public enum SurfaceMaterial
{
    Tierra,
    Madera,
    Pasto,
    Metal,
    Piedra
}