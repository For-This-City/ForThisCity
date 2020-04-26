using UnityEngine;

public class AIController : TopDownCharacterController
{
    [SerializeField] float stopDistance = 5f;

    protected override void Awake()
    {
        base.Awake();
    }

    private void FixedUpdate()
    {
        Vector2 targetDirection = PlayerController.playerPosition - transform.position;
        Vector2 movementDelta = new Vector2(Mathf.Clamp(targetDirection.x, -1.0f, 1.0f), Mathf.Clamp(targetDirection.y, -1.0f, 1.0f));
        float targetDistance = Vector2.Distance(PlayerController.playerPosition, transform.position);

        if (targetDistance > stopDistance)
            character.Move(movementDelta);
        else
            character.Move(Vector2.zero);
        
    }
}
