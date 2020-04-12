
using DG.Tweening;
using UnityEngine;

[RequireComponent(typeof(RectTransform))]
public class ScanerUIManager : MonoBehaviour
{
    RectTransform rectTransform;
    

    #region Getter
    static ScanerUIManager instance;
    public static ScanerUIManager Instance
    {
        get
        {
            if (instance == null)
                instance = FindObjectOfType<ScanerUIManager>();
            if (instance == null)
                Debug.LogError("ScanerUIManager not found");
            return instance;
        }
    }
    #endregion Getter

    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        rectTransform.DOAnchorPosX(0, 0f);
    }

    public void Show(float delay = 0f)
    {
        rectTransform.DOAnchorPosX(0, 0.3f).SetDelay(delay);
    }

    public void Hide(float delay = 0f)
    {
        rectTransform.DOAnchorPosX(rectTransform.rect.width * -1, 0.3f).SetDelay(delay);
    }

    public void ShowSettingsMenu()
    {
        //Hide();
        AddUIManager.Instance.Show();
    }

}
