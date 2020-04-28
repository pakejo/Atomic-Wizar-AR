using DG.Tweening;
using UnityEngine;

/*
 * #########################################################################
 * #            CLASE ENCARGADA DE MANEJAR EL ESCÁNER DE PANTALLA          #
 * #########################################################################
 */

[RequireComponent(typeof(RectTransform))]
public class ScanerUIManager : MonoBehaviour
{
    private RectTransform rectTransform;

    #region Getter

    private static ScanerUIManager instance;

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

    /*
     * Función de Unity usada para inicializar los datos de la clase
     */

    private void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        rectTransform.DOAnchorPosX(0, 0f);
    }

    /*
     * Funcion que muestra el escáner
     */

    public void Show(float delay = 0f)
    {
        rectTransform.DOAnchorPosX(0, 0.3f).SetDelay(delay);
    }

    /*
     * Funcion que oculta el escáner
     */

    public void Hide(float delay = 0f)
    {
        rectTransform.DOAnchorPosX(rectTransform.rect.width * -1, 0.3f).SetDelay(delay);
    }

    /*
     * Funcion que muestra el menú
     */

    public void ShowSettingsMenu()
    {
        //Hide();
        AddUIManager.Instance.Show();
    }
}