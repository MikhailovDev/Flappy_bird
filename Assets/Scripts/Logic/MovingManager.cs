using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingManager : MonoBehaviour, IMovement
{
    [SerializeField] private BirdController bird;
    [SerializeField] private BackgroundMovement backgroundMovement;
    [SerializeField] private GameObject[] pipeMovementItems;

    public void StartMoving()
    {
        backgroundMovement.StartMoving();
        bird.StartMoving();

        SetStartPipeMoving();
    }

    public void StopMoving()
    {
        backgroundMovement.StopMoving();
        bird.StopMoving();

        SetStopPipeMoving();
    }

    public void StartBaseMoving() => backgroundMovement.StartBaseMoving();

    private void SetStartPipeMoving()
    {
        pipeMovementItems = GameObject.FindGameObjectsWithTag("PipePrefab");

        if (pipeMovementItems != null)
        {
            foreach (var pipeMovementItem in pipeMovementItems)
                pipeMovementItem.GetComponent<PipeMovement>().StartMoving();
        }
    }

    private void SetStopPipeMoving()
    {
        pipeMovementItems = GameObject.FindGameObjectsWithTag("PipePrefab");

        if (pipeMovementItems != null)
        {
            foreach (var pipeMovementItem in pipeMovementItems)
                pipeMovementItem.GetComponent<PipeMovement>().StopMoving();
        }
    }
}
