using DG.Tweening;
using UnityEngine;

[RequireComponent(typeof(RectTransform))]
public class AddUIManager : MonoBehaviour
{
    private RectTransform rectTransform;

    #region Getter

    private static AddUIManager instance;

    public static AddUIManager Instance
    {
        get
        {
            if (instance == null)
                instance = FindObjectOfType<AddUIManager>();
            if (instance == null)
                Debug.LogError("HomeUIManager not found");
            return instance;
        }
    }

    #endregion Getter

    private void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        rectTransform.DOAnchorPosX(rectTransform.rect.width, 0f);
    }

    //Deslizamiento hacia la derecha (o abrir)
    public void Show(float delay = 0f)
    {
        rectTransform.DOAnchorPosX(0, 0.3f).SetDelay(delay);
    }

    //Deslizamiento hacia la izquierda (o esconder)
    public void Hide(float delay = 0f)
    {
        rectTransform.DOAnchorPosX(rectTransform.rect.width, 0.3f).SetDelay(delay);
    }

    //Funcion que se ejecutara al pulsar el boton
    //Primero escondera el menu de añadir (linea 46) y luego mostrara el escaner de nuevo (linea 47)
    public void ShowHomeScreen()
    {
        Hide();
        ScanerUIManager.Instance.Show();
    }
}