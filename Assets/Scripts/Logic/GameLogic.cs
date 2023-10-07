using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class GameLogic : MonoBehaviour
{
    #region BUTTON_NAMES

    const string _START_BUTTON = "UI_Btn_Start";
    const string _SCORES_BUTTON = "UI_Btn_Scores";
    const string _EXIT_GAME_BUTTON = "UI_Btn_ExitGame";
    const string _PLAY_AGAIN_BUTTON = "UI_Btn_PlayAgain";
    const string _MENU_BUTTON = "UI_Btn_Menu";
    const string _CLOSE_SCORES_TABEL_BUTTON = "UI_Btn_CloseScoresTabel";
    const string _RESET_DATA_BUTTON = "UI_Btn_ResetData";
    const string _YES_BUTTON = "UI_Btn_Yes";
    const string _NO_BUTTON = "UI_Btn_No";

    #endregion

    [Header("Buttons")]
    [SerializeField] private GameObject[] buttons;
    [SerializeField] private Button[] buttonsButtonComponents;

    [Header("Screens and window")]
    [SerializeField] private GameObject menuScreen;
    [SerializeField] private GameObject gamePreviewScreen;
    [SerializeField] private GameObject scoresTabelScreen;
    [SerializeField] private GameObject gameOverScreen;
    [SerializeField] private GameObject resetDialogWindow;

    [Header("Managers")]
    [SerializeField] private SoundManager soundManager;
    [SerializeField] private AnimationManager animationManager;
    [SerializeField] private MovingManager movingManager;
    [SerializeField] private GameObject pipeManager;

    [Header("Other")]
    [SerializeField] private GameObject bird;
    [SerializeField] private Animator birdAnimator;
    [SerializeField] private CanvasGroup blackPanel;
    [SerializeField] private TopScoreNumbers scoreLogic;
    [SerializeField] private GameObject touchController;
    [SerializeField] private ScorePanelObjects scorePanel;

    private int score;

    private Rigidbody2D birdRB;

    private ScoreData _scoreData;

    private bool _screenIsTouched;

    public int Score
    {
        get => score;
        set => score = value;
    }

    #region EVENT

    public static event Action OnGameStarted;

    #endregion

    #region MONO

    private void OnEnable()
    {
        TouchListener.OnScreenTouched += TouchController_OnScreenTouched;
        CollisionListener.OnBirdCollided += CollisionChecking_OnBirdCollided;
    }

    private void OnDisable()
    {
        TouchListener.OnScreenTouched -= TouchController_OnScreenTouched;
        CollisionListener.OnBirdCollided -= CollisionChecking_OnBirdCollided;
    }

    private void Start()
    {
        movingManager.StopMoving();

        animationManager.AppearMenuAnimation();

        birdRB = bird.GetComponent<Rigidbody2D>();

        _scoreData = new ScoreData();
    }

    #endregion

    public void OnButtonClicked(string buttonName)
    {
        soundManager.PlayBubble();

        var button = FindButtonByName(buttonName);

        if (buttonName != _YES_BUTTON && buttonName != _NO_BUTTON)
        {
            animationManager.SetButtonAnimation(button);
        }

        switch (buttonName)
        {
            case _START_BUTTON:
                StartCoroutine(PlayStartBtnLogic());

                SetButtonInteractable(false, _SCORES_BUTTON, _EXIT_GAME_BUTTON, _START_BUTTON);
                break;
            case _SCORES_BUTTON:
                StartCoroutine(PlayScoreBtnLogic());

                SetButtonInteractable(false, _START_BUTTON, _EXIT_GAME_BUTTON);
                break;
            case _EXIT_GAME_BUTTON:
                StartCoroutine(PlayExitGameBtnLogic());

                SetButtonInteractable(false, _SCORES_BUTTON, _START_BUTTON);
                break;
            case _PLAY_AGAIN_BUTTON:
                StartCoroutine(PlayAgainBtnLogic());

                SetButtonInteractable(false, _MENU_BUTTON);
                break;
            case _MENU_BUTTON:
                StartCoroutine(PlayMenuBtnLogic());

                SetButtonInteractable(false, _PLAY_AGAIN_BUTTON);
                break;
            case _CLOSE_SCORES_TABEL_BUTTON:
                StartCoroutine(PlayCloseScoresTabelBtnLogic());
                break;
            case _RESET_DATA_BUTTON:
                PlayResetDataBtnLogic();

                SetButtonInteractable(false, _RESET_DATA_BUTTON, _CLOSE_SCORES_TABEL_BUTTON);
                break;
            case _YES_BUTTON:
                PlayResetYesBtnLogic();

                SetButtonInteractable(isInteractable: true, _RESET_DATA_BUTTON, _CLOSE_SCORES_TABEL_BUTTON);
                break;
            case _NO_BUTTON:
                PlayResetNoBtnLogic();

                SetButtonInteractable(isInteractable: true, _RESET_DATA_BUTTON, _CLOSE_SCORES_TABEL_BUTTON);
                break;
        }
    }

    private void SetButtonInteractable(bool isInteractable, params string[] buttonNames)
    {
        foreach (string buttonName in buttonNames)
        {
            var button = FindButtonByName(buttonName, buttonsButtonComponents);

            button.enabled = isInteractable;
        }
    }

    private void SetButtonInteractable(bool isInteractable, Button[] buttonsButtonComponents)
    {
        foreach (Button buttonButtonComponent in buttonsButtonComponents)
        {
            buttonButtonComponent.enabled = isInteractable;
        }
    }

    public void AddScore() => Score++;

    public void ResetScoreData()
    {
        _scoreData.ResetData();

        ScoreTabelUpdate();
    }

    private void TouchController_OnScreenTouched()
    {
        if (!_screenIsTouched)
        {
            StartCoroutine(PlayGameStartedLogic());

            _screenIsTouched = true;
        }
    }

    private void CollisionChecking_OnBirdCollided(string obj) => _screenIsTouched = false;

    private GameObject FindButtonByName(string buttonName)
    {
        foreach (var button in buttons)
        {
            if (button.name == buttonName)
            {
                return button;
            }
        }

        return null;
    }

    private Button FindButtonByName(string buttonName, Button[] arrayOfButtons)
    {
        foreach (var button in arrayOfButtons)
        {
            if (button.name == buttonName)
            {
                return button;
            }
        }

        return null;
    }

    private IEnumerator PlayGameStartedLogic()
    {
        GameStarted();

        animationManager.FadeGamePreviewScreenAnimation();

        yield return new WaitForSeconds(0.3f);

        gamePreviewScreen.SetActive(false);

        animationManager.AppearScoreNumbersAnimation(scoreLogic.CurrentScoreNumbersList);
    }

    private IEnumerator PlayStartBtnLogic()
    {
        SetGameStartingBehavior();
        animationManager.FadeMenuAnimation();

        soundManager.StartCoroutine(soundManager.SoundFadeOut(audio: soundManager.BgMenu, time: 1));
        soundManager.StartCoroutine(soundManager.SoundFadeIn(audio: soundManager.BgGame, time: 3f));

        yield return new WaitForSeconds(0.5f);

        menuScreen.SetActive(false);
        gamePreviewScreen.SetActive(true);

        animationManager.AppearGamePreviewScreenAnimation();

        yield return new WaitForSeconds(0.7f);

        touchController.SetActive(true);

        SetButtonInteractable(true, buttonsButtonComponents);
    }

    private IEnumerator PlayAgainBtnLogic()
    {
        animationManager.FadeGameOverScreenAnimation();

        yield return new WaitForSeconds(0.7f);

        SetGameStartingBehavior();

        PipeDestroying.DestroyPipes();

        gameOverScreen.SetActive(false);
        gamePreviewScreen.SetActive(true);

        animationManager.AppearGamePreviewScreenAnimation();

        yield return new WaitForSeconds(0.7f);

        touchController.SetActive(true);

        SetButtonInteractable(true, buttonsButtonComponents);
    }

    private IEnumerator PlayMenuBtnLogic()
    {
        animationManager.FadeGameOverScreenAnimation();

        soundManager.StartCoroutine(soundManager.SoundFadeOut(audio: soundManager.BgGame, time: 1));
        soundManager.StartCoroutine(soundManager.SoundFadeIn(audio: soundManager.BgMenu, time: 3f));

        yield return new WaitForSeconds(0.7f);

        bird.SetActive(false);
        gameOverScreen.SetActive(false);
        menuScreen.SetActive(true);

        animationManager.AppearMenuAnimation();

        bird.transform.position = new Vector3(-2, 0, -2);
        PipeDestroying.DestroyPipes();

        SetButtonInteractable(true, buttonsButtonComponents);
    }

    private IEnumerator PlayScoreBtnLogic()
    {
        animationManager.FadeMenuAnimation();

        yield return new WaitForSeconds(0.5f);

        menuScreen.SetActive(false);
        scoresTabelScreen.SetActive(true);

        ScoreTabelUpdate();

        animationManager.AppearScoreTabelAnimation();
    }

    private IEnumerator PlayExitGameBtnLogic()
    {
        animationManager.FadeMenuAnimation();

        soundManager.StartCoroutine(soundManager.SoundFadeOut(audio: soundManager.BgMenu, time: 0.5f));

        yield return new WaitForSeconds(0.5f);

        Application.Quit();
    }

    private IEnumerator PlayCloseScoresTabelBtnLogic()
    {
        animationManager.FadeScoreTabelAnimation();

        yield return new WaitForSeconds(1f);

        resetDialogWindow.SetActive(false);
        scoresTabelScreen.SetActive(false);
        menuScreen.SetActive(true);

        animationManager.AppearMenuAnimation();

        SetButtonInteractable(isInteractable: true, buttonsButtonComponents);
    }

    private void PlayResetDataBtnLogic()
    {
        resetDialogWindow.SetActive(true);
        animationManager.AppearResetScorePanelAnimation();
    }

    private void PlayResetYesBtnLogic()
    {
        ResetScoreData();

        animationManager.FadeResetScorePanelAnimation();
    }

    private void PlayResetNoBtnLogic()
    {
        animationManager.FadeResetScorePanelAnimation();
    }

    private void SetGameStartingBehavior()
    {
        movingManager.StartBaseMoving();

        birdRB.gravityScale = 0;
        birdRB.velocity = Vector2.zero;

        birdAnimator.enabled = true;
        bird.SetActive(true);

        bird.transform.position = new Vector3(-2, 0, -2);

        scorePanel.RemoveMedal();
        scorePanel.RemoveNewLabel();
    }

    private void GameStarted()
    {
        OnGameStarted?.Invoke();

        birdRB.gravityScale = 1;

        pipeManager.SetActive(true);

        movingManager.StartMoving();
    }

    private void ScoreTabelUpdate()
    {
        _scoreData.LoadData();

        GameObject[] scoreTabelItems = GameObject.FindGameObjectsWithTag("RatingTabelItem");
        Text scoreValue;

        if (scoreTabelItems.Length > 0)
        {
            if (_scoreData.ScoreNumbersInTabelList.Count > 0)
            {
                for (int i = 0; i < _scoreData.ScoreNumbersInTabelList.Count; i++)
                {
                    scoreValue = scoreTabelItems[i].GetComponent<Text>();
                    scoreValue.text = _scoreData.ScoreNumbersInTabelList[i].ToString();
                }
            }
            else
            {
                for (int i = 0; i < scoreTabelItems.Length; i++)
                {
                    scoreValue = scoreTabelItems[i].GetComponent<Text>();
                    scoreValue.text = "Empty";
                }
            }
        }
    }
}
