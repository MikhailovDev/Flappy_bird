using UnityEngine;
using UnityEngine.UI;

public class ScorePanelObjects : MonoBehaviour
{
    private const int BRONZE_MEDAL_INDEX = 1;
    private const int SILVER_MEDAL_INDEX = 2;
    private const int GOLDEN_MEDAL_INDEX = 3;
    private const int PLATINUM_MEDAL_INDEX = 4;

    [Header("Labels")]
    [SerializeField] private Text currentScoreLabel;
    [SerializeField] private Text newScoreLabel;

    [Header("Medals and label")]
    [SerializeField] private GameObject bronzeMedal;
    [SerializeField] private GameObject silverMedal;
    [SerializeField] private GameObject goldenMedal;
    [SerializeField] private GameObject platinumMedal;
    [SerializeField] private GameObject newLabel;

    [Header("Parents")]
    [SerializeField] private GameObject medalsParent;
    [SerializeField] private GameObject newLabelParent;

    private GameObject currentMedal;
    private GameObject currentNewLabel;

    public GameObject AddNewLabel()
    {
        return currentNewLabel = InstantiateObject(newLabel, newLabelParent);
    }

    public GameObject AddMedal(int medalIndex)
    {
        switch (medalIndex) {
            case BRONZE_MEDAL_INDEX:
                currentMedal = InstantiateObject(bronzeMedal, medalsParent);
                break;
            case SILVER_MEDAL_INDEX:
                currentMedal = InstantiateObject(silverMedal, medalsParent);
                break;
            case GOLDEN_MEDAL_INDEX:
                currentMedal = InstantiateObject(goldenMedal, medalsParent);
                break;
            case PLATINUM_MEDAL_INDEX:
                currentMedal = InstantiateObject(platinumMedal, medalsParent);
                break;
            default:
                return null;
        }

        return currentMedal;
    }

    public void RemoveMedal()
    {
        if (currentMedal != null)
        {
            Destroy(currentMedal);
        }
    }

    public void RemoveNewLabel() 
    {
        if (currentNewLabel != null)
        {
            Destroy(currentNewLabel);
        }
    }

    private GameObject InstantiateObject(GameObject gameObject, GameObject parent)
    {
        GameObject instantiatedObject = Instantiate(gameObject, Vector3.zero, Quaternion.identity);
        instantiatedObject.transform.SetParent(parent.transform, false);

        return instantiatedObject;
    }
}
