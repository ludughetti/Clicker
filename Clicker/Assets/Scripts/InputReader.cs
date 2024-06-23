using UnityEngine;
using UnityEngine.InputSystem;

public class InputReader : MonoBehaviour
{
    [SerializeField] private GameController gameController;

    public void HandleCloseInput(InputAction.CallbackContext context)
    {
        if(context.started && gameController.IsCreditScreenActive())
            gameController.ToggleCreditsScreen();
    }
}
