using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class PauseMenu : MonoBehaviour
{
    public static bool GameIsPaused = false;

    public GameObject pauseMenuUi;
    [SerializeField] private GameObject pauseFirstButton;
    [SerializeField] private GameObject howToPlayUi;
    [SerializeField] private GameObject playerPrefab;

    private PlayerInput playerInput;
    private InputAction pauseAction;

    void Awake()
    {
        playerInput = GetComponent<PlayerInput>();
        if (playerInput == null)
            playerInput = FindObjectOfType<PlayerInput>();

        if (playerInput != null)
        {
            pauseAction = playerInput.actions.FindAction("PauseMenu/Pause");

            if (pauseAction != null)
            {
                Debug.Log("Pause action found!");
            }
            else
            {
                Debug.LogError("Pause action not found! Check your Input Actions setup.");
            }
        }
    }

    void Start()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    void OnEnable()
    {
        if (pauseAction != null)
        {
            pauseAction.Enable();
            pauseAction.performed += OnPausePressed;
            Debug.Log("Pause action enabled");
        }
    }

    void OnDisable()
    {
        if (pauseAction != null)
        {
            pauseAction.performed -= OnPausePressed;
            pauseAction.Disable();
        }
    }

    private void OnPausePressed(InputAction.CallbackContext context)
    {
        Debug.Log("Pause button pressed!");
        TogglePause();
    }

    void Update()
    {
        if (Gamepad.current != null && Gamepad.current.startButton.wasPressedThisFrame)
        {
            Debug.Log("Start button pressed directly!");
            TogglePause();
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Debug.Log("Escape pressed!");
            TogglePause();
        }
    }

    private void TogglePause()
    {
        if (GameIsPaused)
        {
            Resume();
        }
        else
        {
            Pause();

            Debug.Log("game is paused");
        }
    }

    public void Resume()
    {
        pauseMenuUi.SetActive(false);
        howToPlayUi.SetActive(false);
        Time.timeScale = 1f;
        GameIsPaused = false;

        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        if (playerPrefab != null)
            playerPrefab.SetActive(true);

        EventSystem.current.SetSelectedGameObject(null);
    }

    public void Pause()
    {
        pauseMenuUi.SetActive(true);
        Time.timeScale = 0f;
        GameIsPaused = true;

        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;

        if (playerPrefab != null)
            playerPrefab.SetActive(false);

        EventSystem.current.SetSelectedGameObject(null);
        StartCoroutine(SetSelectedNextFrame());
    }

    public void RestartGame()
    {
        Time.timeScale = 1f;
        GameIsPaused = false;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        Scene currentScene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(currentScene.name);
    }

    public void MainMenu()
    {
        Time.timeScale = 1f;
        GameIsPaused = false;
        SceneManager.LoadScene("Main Menu");
    }

    private IEnumerator SetSelectedNextFrame()
    {
        yield return null;
        EventSystem.current.SetSelectedGameObject(pauseFirstButton);
    }

    public void QuitGame()
    {
        Application.Quit();
        Debug.Log("App is Quitting");
    }

    public void OpenHowToPlay()
    {
        pauseMenuUi.SetActive(false);
        howToPlayUi.SetActive(true);

        EventSystem.current.SetSelectedGameObject(null);

        var howToPlayFirst = howToPlayUi.GetComponentInChildren<UnityEngine.UI.Selectable>();
        if (howToPlayFirst != null)
            EventSystem.current.SetSelectedGameObject(howToPlayFirst.gameObject);
    }

    public void CloseHowToPlay()
    {
        howToPlayUi.SetActive(false);
        pauseMenuUi.SetActive(true);

        EventSystem.current.SetSelectedGameObject(null);
        StartCoroutine(SetSelectedNextFrame());
    }

}