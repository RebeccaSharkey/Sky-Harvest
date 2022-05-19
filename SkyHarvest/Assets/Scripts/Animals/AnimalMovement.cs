using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// Base script for all animal movement
/// </summary>
public class AnimalMovement : MonoBehaviour
{
    private enum State { Idle, Walking};

    private NavMeshAgent agent;
    [SerializeField] private Animator anim;
    private State state;
    private bool bIsReadyToMove = true;

    private bool bIsRotating = false;
    private float rotationTime = 0;

    [SerializeField] private float radius;
    [SerializeField] private float stopDistance;
    [SerializeField] private float timeToRotate;
    [SerializeField] private float waitTime;

    [SerializeField] private string bIsMoving;
    [SerializeField] private bool bCanUseAnimations;

    private Vector3 anchorPoint;

    private void Start()
    {
        state = State.Idle;
        agent = GetComponent<NavMeshAgent>();
        anchorPoint = transform.position;
    }
    private void Update()
    {
        //Checks if the object is ready to move and if so move to destination
        if(bIsReadyToMove)
        {
            bIsReadyToMove = false;
            StartCoroutine(MoveToPos(SetPosition(transform.position)));
            state = State.Walking;
        }

        //Changes animations depending on the state of the animal
        if (bCanUseAnimations)
        {
            switch (state)
            {
                case State.Idle:
                    anim.SetBool(bIsMoving, false);
                    break;
                case State.Walking:
                    anim.SetBool(bIsMoving, true);
                    break;
            }
        }
    }

    /// <summary>
    /// Sets a move position by checking within a sphere radius of a player
    /// Checks whether the point is within a valid distance of the initial animal spawn point and sets it accordingly
    /// </summary>
    /// <param name="centre">Vector3 passed in which serves as the centre of the check sphere</param>
    /// <returns>Returns the destination point set to then be used for movement</returns>
    private Vector3 SetPosition(Vector3 centre)
    {
        Vector3 randomPoint = centre + Random.insideUnitSphere * radius;
        if(Vector3.Distance(randomPoint, anchorPoint) < radius)
        {
            return randomPoint;
        }
        else
        {
            randomPoint = transform.position;
            return randomPoint;
        }
    }

    /// <summary>
    /// Cororoutine that moves the animal to the point passed in
    /// Disables auto rotation of the NavMesh and sets the rotation manually
    /// Checks how far the animal is from its destination and changes its state accordingly
    /// </summary>
    /// <param name="destination">Vector3 passed in serving as the destination for the animal to move to</param>
    /// <returns>Yields for a certain amount of time to allow for rotation to finish before moving</returns>
    private IEnumerator MoveToPos(Vector3 destination)
    {
        agent.updateRotation = false;
        yield return ChangeRotation(destination);
        state = State.Walking;
        agent.SetDestination(destination);
        while (agent.remainingDistance >= stopDistance)
        {
            yield return null;
        }
        
        yield return new WaitForSeconds(1f);
        agent.updateRotation = true;
        bIsReadyToMove = true;
        yield return null;
    }

    /// <summary>
    /// Coroutine used to manually lerp the animal rotation to look at its destination
    /// </summary>
    /// <param name="destination">Vector3 passed in serving as the location for the animal to look towards</param>
    private IEnumerator ChangeRotation(Vector3 destination)
    {
        Vector3 relativePos = destination - transform.position;
        relativePos.y = 0f;
        Quaternion targetRot = Quaternion.LookRotation(relativePos);
        bIsRotating = true;

        Quaternion initialRot = transform.rotation;
        state = State.Idle;
        while(bIsRotating)
        {
            rotationTime += (Time.deltaTime) * (1 / timeToRotate);
            transform.rotation = Quaternion.Lerp(initialRot, targetRot, rotationTime);
            yield return null;
            if(rotationTime >= 1)
            {
                bIsRotating = false;
                rotationTime = 0;
                break;
            }
        }

        state = State.Idle;
        yield return new WaitForSeconds(waitTime);
    }
}
