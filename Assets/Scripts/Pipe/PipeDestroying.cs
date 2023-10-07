using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class PipeDestroying : MonoBehaviour
{
    public static void DestroyPipes()
    {
        GameObject[] pipes = GameObject.FindGameObjectsWithTag("PipePrefab");

        if (pipes != null)
        {
            foreach (var pipe in pipes)
            {
                Destroy(pipe);
            }
        }
    }
}
