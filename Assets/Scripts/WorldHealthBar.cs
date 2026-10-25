using UnityEngine;
using UnityEngine.UI;

public class WorldHealthBar : MonoBehaviour
{
    private Transform target;
    private Transform barTransform;
    private Camera viewCamera;
    private Image fillImage;
    private Vector3 offset;
    private float maximumHealth;

    public void Initialize(
        Transform followTarget,
        float currentHealth,
        float maxHealth,
        float heightOffset,
        float width = 1f
    )
    {
        target = followTarget;
        maximumHealth = Mathf.Max(1f, maxHealth);
        offset = Vector3.up * heightOffset;
        viewCamera = Camera.main;

        GameObject canvasObject = new GameObject(
            followTarget.name + " Health Bar",
            typeof(RectTransform),
            typeof(Canvas)
        );

        Canvas canvas = canvasObject.GetComponent<Canvas>();
        canvas.renderMode = RenderMode.WorldSpace;
        canvas.worldCamera = viewCamera;
        canvas.sortingOrder = 20;

        barTransform = canvasObject.transform;
        RectTransform canvasRect = canvasObject.GetComponent<RectTransform>();
        canvasRect.sizeDelta = new Vector2(width * 100f, 12f);
        canvasRect.localScale = Vector3.one * 0.01f;

        Image background = CreateImage("Background", canvasRect);
        background.color = new Color(0.08f, 0.08f, 0.08f, 0.85f);

        fillImage = CreateImage("Fill", canvasRect);
        fillImage.type = Image.Type.Filled;
        fillImage.fillMethod = Image.FillMethod.Horizontal;
        fillImage.fillOrigin = (int)Image.OriginHorizontal.Left;
        RectTransform fillRect = fillImage.rectTransform;
        fillRect.anchorMin = Vector2.zero;
        fillRect.anchorMax = Vector2.one;
        fillRect.offsetMin = new Vector2(2f, 2f);
        fillRect.offsetMax = new Vector2(-2f, -2f);

        SetHealth(currentHealth);
        UpdatePosition();
    }

    public void SetHealth(float currentHealth)
    {
        if (fillImage == null)
        {
            return;
        }

        float healthRatio = Mathf.Clamp01(currentHealth / maximumHealth);
        fillImage.fillAmount = healthRatio;
        fillImage.color = Color.Lerp(Color.red, Color.green, healthRatio);
    }

    void LateUpdate()
    {
        UpdatePosition();
    }

    void UpdatePosition()
    {
        if (target == null || barTransform == null)
        {
            return;
        }

        barTransform.position = target.position + offset;

        if (viewCamera == null)
        {
            viewCamera = Camera.main;
        }

        if (viewCamera != null)
        {
            Vector3 directionToCamera = viewCamera.transform.position - barTransform.position;
            if (directionToCamera.sqrMagnitude > 0f)
            {
                barTransform.rotation = Quaternion.LookRotation(directionToCamera, Vector3.up);
            }
        }
    }

    Image CreateImage(string objectName, RectTransform parent)
    {
        GameObject imageObject = new GameObject(
            objectName,
            typeof(RectTransform),
            typeof(CanvasRenderer),
            typeof(Image)
        );
        imageObject.transform.SetParent(parent, false);

        RectTransform imageRect = imageObject.GetComponent<RectTransform>();
        imageRect.anchorMin = Vector2.zero;
        imageRect.anchorMax = Vector2.one;
        imageRect.offsetMin = Vector2.zero;
        imageRect.offsetMax = Vector2.zero;

        return imageObject.GetComponent<Image>();
    }

    void OnDestroy()
    {
        if (barTransform != null)
        {
            Destroy(barTransform.gameObject);
        }
    }
}
