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

    private void Start()
    {
        state = State.Home;
        timer = 0.0f;
        homePosition = transform.position;
        mManager = MechanicManager.instance;
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
                    task.status = Task.Status.InProgress;
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
                task.UpdateStatus(Task.Status.Finished);
                //mManager.RemoveTask();
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
                RequestTask();
                break;
            case State.MoveToTask:
                MoveTowardsTarget(task.taskLocation);
                break;
            case State.Working:
                WorkOnTurret();
                break;
            case State.Finished:
                break;
            case State.MoveToHome:
                MoveTowardsTarget(homePosition);
                break;
            default:
                Debug.Log("Should not reach here");
                break;
        }
    }

    /* Moves the mechanic towards the target */
    private void MoveTowardsTarget(Vector3 target)
    {
        transform.position = Vector3.MoveTowards(transform.position, new Vector3(target.x, transform.position.y, target.z), movementSpeed * Time.deltaTime);
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
