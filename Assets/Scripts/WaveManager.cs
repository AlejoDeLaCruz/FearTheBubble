using UnityEngine;
using UnityEngine.UI;
using System.Collections;
public class WaveManager : MonoBehaviour
{

    public static WaveManager instance;
    public float amplitude = 1f;
    public float legth = 2f;
    public float speed = 1f;
    public float offset = 0f;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }   
        else if (instance != this)
        {
            Debug.Log("Instance already exists, destroying object");
            Destroy(this);
        }
    }

    private void Update()
    {
        offset += Time.deltaTime * speed;
    }

    public float GetWaveHeight(float x)
    {
        return amplitude * Mathf.Sin(x / legth + offset);
    }
}
