using UnityEngine;

public class InputManager : MonoBehaviour
{
    private static InputManager instance;
    public static InputManager Instance { get => instance; }

    public float horizontal;
    public float vertical;
    public Vector2 Movement;

    public bool JumpWasPressed;
    public bool JumpIsHeld;
    public bool JumpWasReleased;
    public bool RunIsHeld;
    public bool jumpLastHeld;

    public bool inputAttackReceived;
    public bool canReceiveAttackInput;


    private void Awake()
    {
        instance = this;
        jumpLastHeld = false;
    }

    private void Update()
    {
        this.MoveInput();
        this.JumpInput();
        this.PauseGame();
        this.MapGame();
    }

    void MoveInput()
    {
        this.horizontal = Input.GetAxisRaw("Horizontal");
        this.vertical = Input.GetAxisRaw("Vertical");
        this.Movement = new Vector2(this.horizontal, 0);

        RunIsHeld = Input.GetKey(KeyCode.LeftShift);
    }

    void JumpInput()
    {
        bool jumpNow = Input.GetKey(KeyCode.Space);
        JumpWasPressed = jumpNow && !jumpLastHeld;
        JumpIsHeld = jumpNow;
        JumpWasReleased = !jumpNow && jumpLastHeld;
        jumpLastHeld = jumpNow;
    }

    public virtual void AttackPressing()
    {
        if (canReceiveAttackInput)
        {
            inputAttackReceived = true;
            canReceiveAttackInput = false;
        }
        else
        {
            return;
        }

    }

    public virtual void InputAttack()
    {
        if (!canReceiveAttackInput)
        {
            canReceiveAttackInput = true;
        }
        else
        {
            canReceiveAttackInput = false;
        }
    }

    void PauseGame()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (UIManager.Instance.pauseUI.activeSelf)
            {
                UIManager.Instance.Continue();
            }
            else
            {
                UIManager.Instance.Pause();
            }
        }
    }

    void MapGame()
    {
        if (Input.GetKeyDown(KeyCode.M))
        {
            if (MapManager.instance.mapGame.activeSelf)
            {
                MapManager.instance.CloseMap();
            }
            else
            {
                MapManager.instance.OpenMap();
            }
        }
    }
}
