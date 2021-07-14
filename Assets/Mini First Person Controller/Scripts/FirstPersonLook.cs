using UnityEngine;

public class FirstPersonLook : MonoBehaviour
{
    [SerializeField] Transform character;
    [SerializeField] bool isCharacterManuallySet; // For having the character being manually defined.
    [Header("Manual Character must have a parent if you only wanna rotate the camera!")]
    [SerializeField] Transform manualCharacter; // MUST HAVE A PARENT OTHERWISE WHOLE THING WILL ROTATE!
    [SerializeField] bool rotateCameraInsteadOfCharacter = false; // Whether the camera should rotated instead of the character.

    public float sensitivity = 2;
    public float smoothing = 1.5f;

    Vector2 velocity;
    Vector2 frameVelocity;

    Quaternion startRotation;

    void Reset()
    {
        if (!isCharacterManuallySet)
        {
            // Get the character from the FirstPersonMovement in parents.
            character = GetComponentInParent<FirstPersonMovement>().transform;
        }
    }

    void Start()
    {
        // Lock the mouse cursor to the game screen.
        Cursor.lockState = CursorLockMode.Locked;

        if (isCharacterManuallySet)
            character = manualCharacter;

        startRotation = transform.rotation;
    }

    void Update()
    {
        // Get smooth velocity.
        Vector2 mouseDelta = new Vector2(Input.GetAxisRaw("Mouse X"), Input.GetAxisRaw("Mouse Y"));
        Vector2 rawFrameVelocity = Vector2.Scale(mouseDelta, Vector2.one * sensitivity);
        frameVelocity = Vector2.Lerp(frameVelocity, rawFrameVelocity, 1 / smoothing);
        velocity += frameVelocity;
        velocity.y = Mathf.Clamp(velocity.y, -90, 90);

        // Rotate camera up-down and controller left-right from velocity.
        transform.localRotation = Quaternion.AngleAxis(-velocity.y, Vector3.right);
        character.localRotation = Quaternion.AngleAxis(velocity.x, Vector3.up);
    }

    void OnEnable()
    {
        if (rotateCameraInsteadOfCharacter)
        {
            transform.rotation = startRotation;
        }
    }
}
