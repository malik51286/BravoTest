using UnityEngine;
using System.Collections;

public abstract class Movement : MonoBehaviour
{
    protected bool _isMoving;
    protected IEnumerator MoveHorizontal(float movementHorizontal, Rigidbody2D rb2d, IMovable movable)
    {
        _isMoving = true;
        Quaternion rotation = Quaternion.Euler(0, 0, -movementHorizontal * 90f);
        transform.rotation = rotation;

        float movementProgress = 0f;
        Vector2 movement, endPos;

        while (movementProgress < Mathf.Abs(movementHorizontal))
        {
            movementProgress += movable.speed * Time.deltaTime;
            movementProgress = Mathf.Clamp(movementProgress, 0f, 1f);
            movement = new Vector2(movable.speed * Time.deltaTime * movementHorizontal, 0f);
            endPos = rb2d.position + movement;

            if (movementProgress == 1)
            {
                endPos = new Vector2(Mathf.Round(endPos.x), endPos.y);
            }

            rb2d.MovePosition(endPos);

            yield return new WaitForFixedUpdate();
        }

        _isMoving = false;
    }

    protected IEnumerator MoveVertical(float movementVertical, Rigidbody2D rb2d, IMovable movable)
    {
        _isMoving = true;
        Quaternion rotation;

        if (movementVertical < 0)
        {
            rotation = Quaternion.Euler(0, 0, movementVertical * 180f);
        }
        else
        {
            rotation = Quaternion.Euler(0, 0, 0);
        }
        transform.rotation = rotation;

        float movementProgress = 0f;
        Vector2 endPos, movement;

        while (movementProgress < Mathf.Abs(movementVertical))
        {

            movementProgress += movable.speed * Time.deltaTime;
            movementProgress = Mathf.Clamp(movementProgress, 0f, 1f);

            movement = new Vector2(0f, movable.speed * Time.deltaTime * movementVertical);
            endPos = rb2d.position + movement;

            if (movementProgress == 1)
            {
                endPos = new Vector2(endPos.x, Mathf.Round(endPos.y));
            }
            rb2d.MovePosition(endPos);
            yield return new WaitForFixedUpdate();

        }

        _isMoving = false;

    }
}