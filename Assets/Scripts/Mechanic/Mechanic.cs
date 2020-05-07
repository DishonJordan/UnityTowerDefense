using UnityEngine;

public class Mechanic : MonoBehaviour
{
    public enum State
    {
        Home,
        OrientTask,
        MoveToTask,
        Work,
        Finish,
        OrientHome,
        MoveToHome,
        DefaultOrientation
    }

    [Header("Properties")]
    public float movementSpeed;
    public float taskSpeed;

    [HideInInspector]
    private Task task;

    private Vector3 homePosition;
    private Quaternion homeRotation;
    private Animator anim;
    public State state;
    private MechanicManager mManager;

    private float timer;
    private float turnSpeed;
    private float heightOfMap = 0.254f;

    private void Start()
    {
        state = State.Home;
        timer = 0.0f;
        turnSpeed = 7f;
        homePosition = transform.position;
        homeRotation = transform.rotation;
        mManager = MechanicManager.instance;
        anim = GetComponent<Animator>();
    }

    private void Update()
    {
        /* State Transisions */
        switch (state)
        {
            case State.Home:
                if (task != null)
                {
                    state = State.OrientTask;
                }
                break;
            case State.OrientTask:
                Vector3 goalDirection = (task.taskLocation - transform.position).normalized;

                if (Vector3.Dot(goalDirection, transform.forward) > 0.95f)
                {
                    state = State.MoveToTask;
                }
                break;
            case State.MoveToTask:
                if (Vector3.Distance(transform.position, task.taskLocation) < 0.5f)
                {
                    state = State.Work;
                }
                break;
            case State.Work:
                if (timer > taskSpeed)
                {
                    state = State.Finish;
                    timer = 0.0f;
                }
                break;
            case State.Finish:
                task = null;
                RequestTask();

                if (task != null)
                {
                    state = State.OrientTask;
                }
                else
                {
                    state = State.OrientHome;
                }
                break;
            case State.OrientHome:
                RequestTask();

                if (task != null)
                {
                    state = State.OrientTask;
                    break;
                }

                Vector3 newDirection = (homePosition - transform.position).normalized;

                if (Vector3.Dot(newDirection, transform.forward) > 0.98f)
                {
                    state = State.MoveToHome;
                }
                break;
            case State.MoveToHome:
                RequestTask();

                if (task != null)
                {
                    state = State.OrientTask;
                    break;
                }

                if (Vector3.Distance(transform.position, homePosition) < 0.1f)
                {
                    state = State.DefaultOrientation;
                }
                break;
            case State.DefaultOrientation:
                state = State.Home;
                break;
            default:
                Debug.Log("Should not reach here");
                break;
        }

        /* State Actions */
        switch (state)
        {
            case State.Home:
                RequestTask();
                anim.SetBool("Move", false);
                anim.SetBool("Idle", true);
                anim.SetBool("Work", false);
                break;
            case State.OrientTask:
                anim.SetBool("Move", false);
                anim.SetBool("Idle", false);
                anim.SetBool("Work", false);

                TurnTowardsTarget(task.taskLocation);
                break;
            case State.MoveToTask:
                anim.SetBool("Move", true);
                anim.SetBool("Idle", false);
                anim.SetBool("Work", false);

                MoveTowardsTarget(task.taskLocation);
                break;
            case State.Work:
                anim.SetBool("Move", false);
                anim.SetBool("Idle", false);
                anim.SetBool("Work", true);

                WorkOnTurret();
                break;
            case State.Finish:
                anim.SetBool("Move", false);
                anim.SetBool("Idle", true);
                anim.SetBool("Work", false);

                break;
            case State.OrientHome:
                anim.SetBool("Move", false);
                anim.SetBool("Idle", false);
                anim.SetBool("Work", false);

                TurnTowardsTarget(homePosition);
                break;
            case State.MoveToHome:
                anim.SetBool("Move", true);
                anim.SetBool("Idle", false);
                anim.SetBool("Work", false);

                MoveTowardsTarget(homePosition);
                break;
            case State.DefaultOrientation:
                anim.SetBool("Move", false);
                anim.SetBool("Idle", false);
                anim.SetBool("Work", false);
                RestoreDefaultRotation();
                break;
            default:
                break;
        }
    }

    /* Moves the mechanic towards the target */
    private void MoveTowardsTarget(Vector3 target)
    {
        transform.position = Vector3.MoveTowards(transform.position, new Vector3(target.x, heightOfMap, target.z), movementSpeed * Time.deltaTime);
    }

    /* Turns the mechanic towards the desired target */
    private void TurnTowardsTarget(Vector3 target)
    {
        Vector3 newDirection = Vector3.RotateTowards(transform.forward, new Vector3(target.x, transform.position.y, target.z) - transform.position,
            turnSpeed * Time.deltaTime, 0.0f);

        transform.rotation = Quaternion.LookRotation(newDirection);
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

    /* Make the Mechanic face the default rotation */
    private void RestoreDefaultRotation()
    {
        transform.rotation = homeRotation;
    }

    /* Requests a task from the mechanic manager */
    public void RequestTask()
    {
        task = mManager.GetTask();
    }
}
