
using UnityEngine;
using UnityEngine.InputSystem;

public class PauseGame : MonoBehaviour
{
    public bool isPaused { get; set; }
    public InputActionAsset actions;
    public GameObject pauseMenu;
    void Awake()
    {
        isPaused = false;
        PlayerInputActions playerInputActions = new PlayerInputActions();
        playerInputActions.Player.PauseGame.performed += OnPauseGame;
        playerInputActions.Player.Enable();
        PAUSE_EVENT.Resume += BackButtonWasPressed;
    }

    private void BackButtonWasPressed()
    {
        isPaused= false;
    }

    void OnEnable()
    {
        actions.FindActionMap("Player").Enable();
    }
    void OnDisable()
    {
        actions.FindActionMap("Player").Disable();
    }
    public void Pause()
    {
        PAUSE_EVENT.OnPause();
        isPaused= true;
        pauseMenu.SetActive(true);
    }
    public void Resume()
    {
        PAUSE_EVENT.OnResume();
        isPaused= false;
        pauseMenu.SetActive(false);
    }
    private void OnPauseGame(InputAction.CallbackContext context)
    {
        if(isPaused)
            Resume();
        else
            Pause();
    }


}

