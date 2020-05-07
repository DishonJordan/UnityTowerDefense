using UnityEngine;

public class Mechanic : MonoBehaviour
{
    public enum State
    {
        Home,
        MoveToTask,
        Working,
        MoveToHome,
        Finished
    }

    [Header("Properties")]
    public float movementSpeed;
    public float taskSpeed;

    [HideInInspector]
    public Task task;

    private Vector3 homePosition;
    private State state;
    private float timer;
    private MechanicManager mManager;
    private float turnSpeed;
    private Animator anim;

    private void Start()
    {
        state = State.Home;
        timer = 0.0f;
        turnSpeed = 4f;
        homePosition = transform.position;
        mManager = MechanicManager.instance;
        anim = GetComponent<Animator>();
    }

    private void Update()
    {
        /* State Transitions */
        switch (state)
        {
            case State.Home:
                if (task != null)
                {
                    state = State.MoveToTask;
                }
                break;
            case State.MoveToTask:
                if (Vector3.Distance(transform.position, task.taskLocation) < 0.6f)
                {
                    state = State.Working;
                }
                break;
            case State.Working:
                if (timer > taskSpeed)
                {
                    state = State.Finished;
                    timer = 0.0f;
                }
                break;
            case State.Finished:
                task = null;
                RequestTask();

                if (task != null)
                {
                    state = State.MoveToTask;
                }
                else
                {
                    state = State.MoveToHome;
                }
                break;
            case State.MoveToHome:
                if (Vector3.Distance(transform.position, homePosition) < 0.3f)
                {
                    state = State.Home;
                }
                RequestTask();
                if (task != null)
                {
                    state = State.MoveToTask;
                }
                break;
            default:
                Debug.Log("Should not reach here");
                break;
        }

        /* State Actions */
        switch (state)
        {
            case State.Home:
                anim.SetBool("Move", false);
                RequestTask();
                break;
            case State.MoveToTask:
                TurnTowardsTarget(task.taskLocation);
                break;
            case State.Working:
                anim.SetBool("Move", false);
                WorkOnTurret();
                break;
            case State.Finished:
                anim.SetBool("Move", false);
                break;
            case State.MoveToHome:
                TurnTowardsTarget(homePosition);
                break;
            default:
                Debug.Log("Should not reach here");
                break;
        }
    }

    /* Moves the mechanic towards the target */
    private void MoveTowardsTarget(Vector3 target)
    {
        anim.SetBool("Move", true);
        transform.position = Vector3.MoveTowards(transform.position, new Vector3(target.x, transform.position.y, target.z), movementSpeed * Time.deltaTime);
    }

    private void TurnTowardsTarget(Vector3 target)
    {
        Vector3 goalDirection = (target - transform.position).normalized;

        if (Vector3.Dot(goalDirection, transform.forward) > 0.90f)
        {
            MoveTowardsTarget(target);
        }
        else
        {
            Vector3 newDirection = Vector3.RotateTowards(transform.forward, new Vector3(target.x, transform.position.y, target.z) - transform.position, turnSpeed * Time.deltaTime, 0.0f);
            transform.rotation = Quaternion.LookRotation(newDirection);
        }
    }

    /* Delay before task is performed */
    private void WorkOnTurret()
    {
        timer += Time.deltaTime;
        if (timer > taskSpeed)
        {
            task.PerformTask();
        }
    }

    public void RequestTask()
    {
        task = mManager.GetTask();
    }
}
