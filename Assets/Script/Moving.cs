using UnityEngine;

public class Moving : MonoBehaviour
{
    [SerializeField] private Transform positionA;
    [SerializeField] private Transform positionB;
    [SerializeField] private float speed = 2f;
    private Vector3 target;
    void Start()
    {
        target = positionA.position;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, target, speed * Time.deltaTime);

        if (Vector3.Distance(transform.position, target) < 0.01f)
        {
            if (target == positionA.position)
            {
                target = positionB.position;
            }
            else
            {
                target = positionA.position;
            }
        }

    }
    

}
