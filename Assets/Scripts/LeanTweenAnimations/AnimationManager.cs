using System.Collections.Generic;
using UnityEngine;

public class AnimationManager : MonoBehaviour
{
    private const float _ASPECT_RATIO = 1.78f;
    private const float _BIRD_BASIC_POSITION_X = -0.8f;

    [Header("Looped image")]
    [SerializeField] private GameObject flappyBirdImage;

    [Header("Copyright")]
    [SerializeField] private CanvasGroup copyrightImage;

    [Header("Buttons")]
    [SerializeField] private GameObject startBtn;
    [SerializeField] private GameObject scoresBtn;
    [SerializeField] private GameObject exitGameBtn;
    [SerializeField] private GameObject playAgainBtn;
    [SerializeField] private GameObject menuBtn;
    [SerializeField] private GameObject closeScoresTabelBtn;
    [SerializeField] private GameObject resetDataBtn;
    [SerializeField] private GameObject yesBtn;
    [SerializeField] private GameObject noBtn;

    [Header("Screens and window")]
    [SerializeField] private GameObject menuScreen;
    [SerializeField] private GameObject gamePreviewScreen;
    [SerializeField] private GameObject scoresTabelScreen;
    [SerializeField] private GameObject gameOverScreen;
    [SerializeField] private GameObject resetDialogWindow;

    [Header("Game preview objects")]
    [SerializeField] private GameObject getReadyImage;
    [SerializeField] private GameObject gameMechanicsImage;
    [SerializeField] private GameObject bird;

    [Header("Scores tabel objects")]
    [SerializeField] private GameObject scoreImage;
    [SerializeField] private GameObject scoresTabelItem1;
    [SerializeField] private GameObject scoresTabelItem2;
    [SerializeField] private GameObject scoresTabelItem3;
    [SerializeField] private GameObject scoresTabelItem4;
    [SerializeField] private GameObject scoresTabelItem5;

    [Header("Reset score panel objects")]
    [SerializeField] private CanvasGroup blackPanel;
    [SerializeField] private GameObject dialogWindow;

    [Header("Game over screen objects")]
    [SerializeField] private GameObject gameOverImage;
    [SerializeField] private GameObject gameOverScorePanel;
    [SerializeField] private CanvasGroup currentScore;
    [SerializeField] private CanvasGroup bestScore;

    [Space]
    [SerializeField] private SoundManager soundManager;

    private GameObject pressedButton;

    private float _resultingAspectRatio;


    #region AnimationsOnYAxis

    private void SetAppearingPopupAnimation(GameObject gameObject, Vector3 to, float time)
    {
        LeanTween.moveLocal(gameObject, to, time).setEaseOutQuint();
    }
    private void SetAppearingPopupAnimation(GameObject gameObject, Vector3 to, float time, float delay)
    {
        LeanTween.moveLocal(gameObject, to, time).setEaseOutQuint().setDelay(delay);
    }

    private void SetFadingPopupAnimation(GameObject gameObject, Vector3 to, float time)
    {
        LeanTween.moveLocal(gameObject, to, time).setEaseOutQuad();
    }
    private void SetFadingPopupAnimation(GameObject gameObject, Vector3 to, float time, float delay)
    {
        LeanTween.moveLocal(gameObject, to, time).setEaseOutQuad().setDelay(delay);
    }

    #endregion

    private void Start() => _resultingAspectRatio = _ASPECT_RATIO / ((float)Screen.height / Screen.width);

    public void AppearMenuAnimation()
    {
        SetAppearingPopupAnimation(flappyBirdImage, new Vector3(0, 780, 0), 0.5f);
        LeanTween.moveLocalY(flappyBirdImage, 740f, 0.35f).setDelay(0.31f).setEaseInOutCubic().setLoopPingPong();

        LeanTween.alphaCanvas(copyrightImage, 1f, 0.5f);

        SetAppearingPopupAnimation(startBtn, new Vector3(0, 207, 0), 0.5f);
        SetAppearingPopupAnimation(scoresBtn, new Vector3(0, -143, 0), 0.5f, 0.1f);
        SetAppearingPopupAnimation(exitGameBtn, new Vector3(0, -493, 0), 0.5f, 0.2f);
    }

    public void FadeMenuAnimation()
    {
        LeanTween.cancel(flappyBirdImage);

        SetFadingPopupAnimation(exitGameBtn, new Vector3(0, -2505, 0), 0.5f);
        SetFadingPopupAnimation(scoresBtn, new Vector3(0, -2155, 0), 0.5f, 0.1f);
        SetFadingPopupAnimation(startBtn, new Vector3(0, -1755, 0), 0.5f, 0.2f);

        LeanTween.alphaCanvas(copyrightImage, 0f, 0.5f);

        SetFadingPopupAnimation(flappyBirdImage, new Vector3(0, 1696, 0), 0.5f);
    }


    public void AppearGamePreviewScreenAnimation()
    {
        LeanTween.alpha(bird, 1f, 0f);

        SetAppearingPopupAnimation(getReadyImage, new Vector3(0, 780, 0), 0.5f);
        SetAppearingPopupAnimation(gameMechanicsImage, new Vector3(0, -215, 0), 0.5f, 0.1f);

        LeanTween.moveLocal(bird, new Vector3(_BIRD_BASIC_POSITION_X * _resultingAspectRatio, 0, -2), 0.5f).setEaseOutBack().setDelay(0.2f);
    }

    public void FadeGamePreviewScreenAnimation()
    {
        SetFadingPopupAnimation(gameMechanicsImage, new Vector3(0, -2071, 0), 0.5f);
        SetFadingPopupAnimation(getReadyImage, new Vector3(0, 1637, 0), 0.5f);
    }


    public void AppearScoreNumbersAnimation(List<GameObject> scoreSymbols)
    {
        foreach (var scoreSymbol in scoreSymbols)
        {
            LeanTween.moveLocal(scoreSymbol, new Vector3(0, 600, 0), 0);
            SetAppearingPopupAnimation(scoreSymbol, new Vector3(0, 0, 0), 0.5f);
        }
    }

    public void FadeScoreNumbersAnimation(List<GameObject> scoreSymbols, float numberPositionOffset = default)
    {
        if (scoreSymbols.Count == 1)
        {
            SetFadingPopupAnimation(scoreSymbols[0], new Vector3(0, 600, 0), 0.5f);
        }
        else
        {
            for (int i = 0; i < scoreSymbols.Count; i++)
            {
                SetFadingPopupAnimation(scoreSymbols[i], new Vector3(numberPositionOffset * i, 600, 0), 0.5f);
            }
        }
    }


    public void AppearScoreTabelAnimation()
    {
        SetAppearingPopupAnimation(scoreImage, new Vector3(0, 780, 0), 0.5f);

        SetAppearingPopupAnimation(scoresTabelItem1, new Vector3(0, 404, 0), 0.5f, 0.1f);
        SetAppearingPopupAnimation(scoresTabelItem2, new Vector3(0, 254, 0), 0.5f, 0.2f);
        SetAppearingPopupAnimation(scoresTabelItem3, new Vector3(0, 104, 0), 0.5f, 0.3f);
        SetAppearingPopupAnimation(scoresTabelItem4, new Vector3(0, -46, 0), 0.5f, 0.4f);
        SetAppearingPopupAnimation(scoresTabelItem5, new Vector3(0, -196, 0), 0.5f, 0.5f);

        LeanTween.moveLocalX(closeScoresTabelBtn, 520, 0.5f).setEaseOutQuad().setDelay(0.6f);
        LeanTween.moveLocalX(resetDataBtn, 520, 0.5f).setEaseOutQuad().setDelay(0.7f);
    }

    public void FadeScoreTabelAnimation()
    {
        LeanTween.moveLocalX(resetDataBtn, 1320, 0.5f).setEaseOutQuad();
        LeanTween.moveLocalX(closeScoresTabelBtn, 1320, 0.5f).setEaseOutQuad().setDelay(0.1f);

        SetFadingPopupAnimation(scoresTabelItem5, new Vector3(1450, -196, 0), 0.5f, 0.2f);
        SetFadingPopupAnimation(scoresTabelItem4, new Vector3(1450, -46, 0), 0.5f, 0.3f);
        SetFadingPopupAnimation(scoresTabelItem3, new Vector3(1450, 104, 0), 0.5f, 0.4f);
        SetFadingPopupAnimation(scoresTabelItem2, new Vector3(1450, 254, 0), 0.5f, 0.5f);
        SetFadingPopupAnimation(scoresTabelItem1, new Vector3(1450, 404, 0), 0.5f, 0.6f);

        SetFadingPopupAnimation(scoreImage, new Vector3(0, 1624, 0), 0.5f, 0.7f);
    }


    public void AppearResetScorePanelAnimation()
    {
        SetAppearingPopupAnimation(dialogWindow, new Vector3(0, 0, 0), 0.5f);
        LeanTween.scale(dialogWindow, Vector3.one, 0.7f).setEaseOutBack();

        LeanTween.alphaCanvas(blackPanel, 1f, 0.5f);

        LeanTween.scale(noBtn, Vector3.one, 0.2f).setDelay(0.4f);
        LeanTween.scale(yesBtn, Vector3.one, 0.2f).setDelay(0.5f);
    }

    public void FadeResetScorePanelAnimation()
    {
        LeanTween.alphaCanvas(blackPanel, 0f, 0.7f);

        LeanTween.scale(dialogWindow, Vector3.zero, 0.1f).setDelay(0.4f);
        SetFadingPopupAnimation(dialogWindow, new Vector3(0, -1956, 0), 0.5f);

        LeanTween.scale(yesBtn, Vector3.zero, 0.2f);
        LeanTween.scale(noBtn, Vector3.zero, 0.2f);
    }


    public void AppearGameOverScreenAnimation(GameObject scorePanelMedal = null, GameObject scorePanelNewLabel = null)
    {
        float delay = 0;

        HidePipesAndBird();

        SetAppearingPopupAnimation(gameOverImage, new Vector3(0, 780, 0), 0.5f);
        SetAppearingPopupAnimation(gameOverScorePanel, new Vector3(0, 150, 0), 0.5f, delay += 0.1f);

        if (scorePanelMedal != null)
        {
            LeanTween.scale(scorePanelMedal, new Vector3(1, 1, 1), 0.2f).setDelay(delay += 0.3f);
            LeanTween.alphaCanvas(scorePanelMedal.GetComponent<CanvasGroup>(), 1, 0.2f).setDelay(delay).setOnComplete(PlayCoin);
        }

        if (scorePanelNewLabel != null)
        {
            LeanTween.scale(scorePanelNewLabel, new Vector3(1, 1, 1), 0.2f).setDelay(delay += 0.3f);
            LeanTween.alphaCanvas(scorePanelNewLabel.GetComponent<CanvasGroup>(), 1, 0.2f).setDelay(delay).setOnComplete(PlayLabel);
        }

        LeanTween.alphaCanvas(currentScore, 1f, 0.3f).setDelay(delay += 0.4f);

        LeanTween.alphaCanvas(bestScore, 1f, 0.3f).setDelay(delay += 0.3f);

        SetAppearingPopupAnimation(playAgainBtn, new Vector3(0, -490, 0), 0.5f, delay += 0.1f);
        SetAppearingPopupAnimation(menuBtn, new Vector3(0, -800, 0), 0.5f, delay += 0.1f);
    }

    public void FadeGameOverScreenAnimation()
    {
        SetFadingPopupAnimation(menuBtn, new Vector3(0, -2770, 0), 0.5f);
        SetFadingPopupAnimation(playAgainBtn, new Vector3(0, -2460, 0), 0.5f, 0.1f);

        SetFadingPopupAnimation(gameOverScorePanel, new Vector3(0, -1820, 0), 0.5f, 0.2f);

        LeanTween.alphaCanvas(currentScore, 0f, 0.1f).setDelay(1f);
        LeanTween.alphaCanvas(bestScore, 0f, 0.1f).setDelay(1f);

        SetFadingPopupAnimation(gameOverImage, new Vector3(0, 1655, 0), 0.5f, 0.3f);
    }


    public void SetButtonAnimation(GameObject button)
    {
        pressedButton = button;

        LeanTween.moveLocalY(button, button.GetComponent<RectTransform>().localPosition.y - 10, 0.1f);
        LeanTween.scale(button, new Vector3(0.9f, 0.9f, 0.9f), 0.1f).setOnComplete(ResetButtonAnimation);
    }

    private void ResetButtonAnimation()
    {
        LeanTween.moveLocalY(pressedButton, pressedButton.GetComponent<RectTransform>().localPosition.y + 10, 0.1f);
        LeanTween.scale(pressedButton, Vector3.one, 0.1f);
    }


    private void HidePipesAndBird()
    {
        LeanTween.alpha(bird, 0f, 1f).setDelay(0.5f);

        GameObject[] pipes = GameObject.FindGameObjectsWithTag("PipePrefab");

        foreach (var pipe in pipes)
        {
            LeanTween.alpha(pipe, 0f, 1f).setDelay(0.5f);
        }
    }

    private void PlayCoin() => soundManager.PlayCoin();

    private void PlayLabel() => soundManager.PlayLabel();
}