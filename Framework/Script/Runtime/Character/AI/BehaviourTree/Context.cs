using UnityEngine;
using UnityEngine.AI;

/// <summary> Unity Component 들을 노드들이 공유하기 위해 사용 </summary>
public class Context
{
    public GameObject gameObject;
    public Transform transform;
    public Animator animator;
    public NavMeshAgent agent;

    public static Context CreateContext(GameObject gameObject)
    {
        Context context = new Context
        {
            gameObject = gameObject,
            transform = gameObject.transform,
            animator = gameObject.GetComponent<Animator>(),
            agent = gameObject.GetComponent<NavMeshAgent>()
        };

        return context;
    }
}
