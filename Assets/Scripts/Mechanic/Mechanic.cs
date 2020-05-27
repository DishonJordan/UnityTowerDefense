using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class Mechanic : MonoBehaviour
{
    enum State
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

    public GameObject taskUI;

    [HideInInspector]
    private Task task;

    private Vector3 homePosition;
    private Quaternion homeRotation;
    private Animator anim;
    private State state;
    private MechanicManager mManager;
    private GameObject taskButton;

    private float timer;
    private readonly float turnSpeed = 7f;
    private readonly float heightOfMap = 0.254f;
    private float previousDirection = 0f;

    private void Start()
    {
        state = State.Home;
        timer = 0.0f;

        homePosition = transform.position;
        homeRotation = transform.rotation;

        mManager = MechanicManager.instance;
        anim = GetComponent<Animator>();

        taskButton = taskUI.transform.GetChild(0).GetChild(1).gameObject;
        UpdateTaskUI(null, "No Task");
    }

    private void Update()
    {
        Tick();
    }

    /* Toggles the Task UI */
    private void OnMouseDown()
    {
        taskUI.SetActive(!taskUI.activeSelf);
    }

    /* Runs the state machine */
    private void Tick()
    {
        /* State Transitions */
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

                if (Vector3.Dot(goalDirection, transform.forward) > 0.95f || previousDirection == Vector3.Dot(goalDirection, transform.forward))
                {
                    state = State.MoveToTask;
                }

                previousDirection = Vector3.Dot(goalDirection, transform.forward);
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
                SetAnimationBools(false, true, false);
                break;
            case State.OrientTask:
                TurnTowardsTarget(task.taskLocation);
                SetAnimationBools(false, false, false);
                break;
            case State.MoveToTask:
                MoveTowardsTarget(task.taskLocation);
                SetAnimationBools(true, false, false);
                break;
            case State.Work:
                WorkOnTurret();
                SetAnimationBools(false, false, true);
                break;
            case State.Finish:
                timer = 0.0f;
                SetAnimationBools(false, true, false);
                break;
            case State.OrientHome:
                TurnTowardsTarget(homePosition);
                SetAnimationBools(false, false, false);
                break;
            case State.MoveToHome:
                MoveTowardsTarget(homePosition);
                SetAnimationBools(true, false, false);
                break;
            case State.DefaultOrientation:
                RestoreDefaultRotation();
                SetAnimationBools(false, false, false);
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
            if(task.GetTaskType() != Task.Type.Repair)
            {
                MechanicManager.instance.IncreaseXp();
            }
            UpdateTaskUI(null, "No Task");
        }
    }

    /* Make the Mechanic face the default rotation */
    private void RestoreDefaultRotation()
    {
        transform.rotation = homeRotation;
    }

    /* Requests a task from the mechanic manager */
    private void RequestTask()
    {
        task = mManager.GetTask();
        if (task != null)
        {
            UpdateTaskUI(task.GetIcon(), task.GetTaskName());
        }
    }

    /* Update the Task UI to a specific sprite and string */
    private void UpdateTaskUI(Sprite s, string t)
    {
        taskButton.GetComponentInChildren<TextMeshProUGUI>().text = t;

        if (s != null)
        {
            Image img = taskButton.transform.GetChild(1).GetComponentInChildren<Image>();
            img.sprite = s;
            img.color = new Color(img.color.r, img.color.g, img.color.b, 255);
        }
        else
        {
            taskButton.transform.GetChild(1).GetComponentInChildren<Image>().sprite = null;

            Color c = taskButton.transform.GetChild(1).GetComponentInChildren<Image>().color;
            taskButton.transform.GetChild(1).GetComponentInChildren<Image>().color = new Color(c.r, c.b, c.g, 0);
        }
    }

    public void IncreaseMovementSpeed(float speed) {
        movementSpeed += speed;
    }

    public void DecreaseTaskSpeed(float speed)
    {
        taskSpeed = (taskSpeed - speed < 0f) ? 0f : (taskSpeed - speed);
    }

    /* Sets the animation bools */
    private void SetAnimationBools(bool move, bool idle, bool work)
    {
        anim.SetBool("Move", move);
        anim.SetBool("Idle", idle);
        anim.SetBool("Work", work);
    }
}
