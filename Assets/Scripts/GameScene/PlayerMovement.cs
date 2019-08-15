using UnityEngine;
public class PlayerMovement : Movement
{
    private float horizontal;
    private float vertical;
    private Rigidbody2D rb2d;
    private IMovable movable;
    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
        movable = GetComponent<IMovable>();
    }
    private void FixedUpdate()
    {
        if (horizontal != 0 && !_isMoving)
        {
            StartCoroutine(MoveHorizontal(horizontal, rb2d, movable));
            movable.SetMoving();
        }
        else if (vertical != 0 && !_isMoving)
        {
            StartCoroutine(MoveVertical(vertical, rb2d, movable));
            movable.SetMoving();
        }
        else if (horizontal == 0 && vertical == 0)
        {
            movable.SetIdle();
        }

    }
    void Update()
    {
        horizontal = Input.GetAxisRaw("Horizontal");
        vertical = Input.GetAxisRaw("Vertical");
        if (Input.GetKeyDown(KeyCode.Space))
        {
            movable.Fire();
        }
    }
}