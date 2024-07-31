using ThatOneSamuraiGame.Scripts;
using ThatOneSamuraiGame.Scripts.Input;
using ThatOneSamuraiGame.Scripts.Scene.SceneManager;
using ThatOneSamuraiGame.Scripts.UI.Pause.PauseManager;
using ThatOneSamuraiGame.Scripts.UI.UserInterfaceManager;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    /*
     * The Game manager should be reserved for:
     * - holding high level references
     * - handling game initialisation 
     * - handling settings
     * - handling state transitions for whole game
     * - handling configuration
     * - Handling coordination of global events
     * - management of global services and lookup
     */
    
    #region - - - - - - Fields - - - - - -

    //Singleton instance
    public static GameManager instance = null;

    [Header("Game Settings")]
    public GameSettings gameSettings;
    public bool bShowAttackPopups = true;

    [Space]
    public PostProcessingController postProcessingController; // keep this, its likely to be part of some graphics manager

    [Header("Persistent Managers")]
    [SerializeField] private SceneManager m_SceneManager;
    [SerializeField] private UserInterfaceManager m_UserInterfaceManager;
    [SerializeField] private PauseManager m_PauseManager;
    public AudioManager audioManager;
    private IInputManager m_InputManager;
    
    private GameState m_GameState;
    
    #endregion Fields

    #region - - - - - - Properties - - - - - -

    public GameState GameState
        => this.m_GameState;
    
    // ----------------------------------------------
    // Managers
    // ----------------------------------------------

    public IInputManager InputManager
        => this.m_InputManager;

    public IPauseManager PauseManager
        => this.m_PauseManager;

    public ISceneManager SceneManager
        => this.m_SceneManager;

    public IUserInterfaceManager UserInterfaceManager
        => this.m_UserInterfaceManager;
    
    // ----------------------------------------------
    // Property pass-through 
    // ----------------------------------------------

    // ALL PROPERTIES BELOW THIS:
    //  - This is to only maintain existing references to the old fields to reduce propagated changes.
    //  - Will be resolved once the state of the source values are resolved.
    
    public CheckpointManager CheckpointManager
        => ((ISceneManager)this.m_SceneManager).CheckpointManager;

    public RewindManager RewindManager
        => ((ISceneManager)this.m_SceneManager).RewindManager;

    public EnemyTracker EnemyTracker
        => ((ISceneManager)this.m_SceneManager).EnemyTracker;

    public EnemySpawnManager EnemySpawnManager
        => ((ISceneManager)this.m_SceneManager).EnemySpawnManager;
    
    // ----------------------------------------------
    // Camera
    // ----------------------------------------------

    public CameraControl CameraControl
        => ((ISceneManager)this.m_SceneManager).CameraControl;
    
    public LockOnTracker LockOnTracker
        => ((ISceneManager)this.m_SceneManager).LockOnTracker;
    
    public Camera MainCamera
        => ((ISceneManager)this.m_SceneManager).MainCamera;
    
    public GameObject ThirdPersonViewCamera
        => ((ISceneManager)this.m_SceneManager).ThirdPersonViewCamera;

    // ----------------------------------------------
    // Player
    // ----------------------------------------------

    public PlayerController PlayerController
        => ((ISceneManager)this.m_SceneManager).PlayerController;
    
    #endregion Properties

    #region - - - - - - Lifecycle Methods - - - - - -

    void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);

        // Perform setup pipeline
        // Ticket: #43 - Move this into its own pipeline handler to separate initialisation logic from the game manager.
        
        // Setup game scene
        SetupGraphics();
        SetupAudio();

        // Locate services
        this.m_GameState = this.GetComponent<GameState>();
        this.m_InputManager = this.GetComponent<IInputManager>();
    }

    // Start is called before the first frame update
    void Start()
    {
        ((ISceneManager)this.m_SceneManager).SetupScene();
        ((IUserInterfaceManager)this.m_UserInterfaceManager).SetupUserInterface();
        
        this.m_InputManager.ConfigureMenuInputControl();
        this.m_InputManager.SwitchToMenuControls();
    }

    #endregion Lifecycle Methods

    #region - - - - - - Methods - - - - - -

    void SetupGraphics() // Handled in pipeline
    {
        //Add Post Processing
        postProcessingController = Instantiate(gameSettings.dayPostProcessing, transform.position, Quaternion.identity).GetComponent<PostProcessingController>();
    }

    void SetupAudio() // Handled in pipeline
    {
        if (FindObjectOfType<AudioManager>() == null) 
            audioManager = Instantiate(gameSettings.audioManger, transform.position, Quaternion.identity).GetComponent<AudioManager>();
    }
    
    // -----------------------------------------------------------
    // Temporary: Actions pertaining to defined game events
    // -----------------------------------------------------------

    // Note: Should be refactored to its own definable hard-coded event.
    public void OnOpeningSceneStart() // Scene manager - but possible delete if not used
        => Debug.LogWarning("Not implemented");

    #endregion Methods

}
