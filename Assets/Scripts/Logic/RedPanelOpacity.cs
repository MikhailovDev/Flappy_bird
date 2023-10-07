using UnityEngine;

public class RedPanelOpacity : MonoBehaviour
{
    private float MAX_DANGEROUS_HEIGHT = 2.73f;

    [SerializeField] private CanvasGroup panel;
    [SerializeField] private BirdController birdController;

    private float onePersentOfRedDegree;

    private void Awake() => onePersentOfRedDegree = (MAX_DANGEROUS_HEIGHT + birdController.GROUND) / 100;

    private void OnEnable() => CollisionListener.OnBirdCollided += CollisionChecking_OnBirdCollided;

    private void OnDisable() => CollisionListener.OnBirdCollided -= CollisionChecking_OnBirdCollided;

    private void CollisionChecking_OnBirdCollided(string obj)
    {
        float degreeOfRed = (birdController.GroundDistance + birdController.GROUND) / onePersentOfRedDegree / 100;

        if (obj == "Pipe")
        {
            SetDegreeOfRed(0.5f, 0.2f);
        }

        if (obj == "Base")
        {
            SetDegreeOfRed(degreeOfRed, degreeOfRed / 3);
        }
    }

    private void SetDegreeOfRed(float degreeOfRed, float delay)
    {
        panel.LeanAlpha(degreeOfRed, delay).setEaseOutCirc();
        panel.LeanAlpha(0, delay).setEaseInCirc().delay = delay;
    }
}
