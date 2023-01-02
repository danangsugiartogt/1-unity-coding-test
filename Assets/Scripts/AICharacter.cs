using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AICharacter : MonoBehaviour
{
    private Queue<Vector3> path = new Queue<Vector3>();
    private Vector3 targetPosition;

    [SerializeField] private GameObject particlePrefab;
    [SerializeField] private float speed = 5;
    [SerializeField] private bool isMoving;

    private ParticleSystem particle;
    private Action onPathCompleted;

    void Awake()
    {
        targetPosition = transform.position;
        particle = Instantiate(particlePrefab).GetComponent<ParticleSystem>(); ;
    }

    public void SetOnPathCompleted(Action action)
    {
        onPathCompleted = action;
    }

    public void SetPath(Queue<Vector3> path)
    {
        this.path = path;        
    }    

    void Update()
    {
        if(Vector3.Distance(transform.position,targetPosition) < 0.05f)
        {
            if(path.Count <= 0)
            {
                isMoving = false;
                onPathCompleted?.Invoke();
                return;
            }
            targetPosition = path.Dequeue();
        }
        isMoving = true;
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);
    }

    void OnTriggerEnter(Collider otherCollider)
    {
        if (otherCollider.tag == "Reward")
        {
            ShowParticle(transform.position);
        }
    }

    void ShowParticle(Vector3 position)
    {
        particle.Stop();
        particle.transform.position = position;
        particle.Play();
    }
}
