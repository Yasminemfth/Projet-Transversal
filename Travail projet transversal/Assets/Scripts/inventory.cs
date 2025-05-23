using UnityEngine;
using UnityEngine.UI;

public class inventory : MonoBehaviour
{
    public static inventory instance;

    [SerializeField] private Image tokenSlotImage;
    [SerializeField] private Sprite emptySlotSprite, tokenSprite;
    [SerializeField] private GameObject promptText;

    private SafeBracelet _heldBracelet;

    private void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(gameObject);
    }

    private void Start()
    {
        promptText.SetActive(false);
        UpdateSlotUI();
    }

    private void Update()
    {
        

        // Detecter Q sur Azerty (ou A sur Qwerty)
        if (Input.GetKeyDown(KeyCode.Q))
            Debug.Log("[inventory] J'ai détecté la touche Q");

        if (_heldBracelet != null && Input.GetKeyDown(KeyCode.Q))
        {
            Debug.Log("[inventory] Condition remplie, j'utilise le bracelet");
            promptText.SetActive(false);
            _heldBracelet.Use();
            _heldBracelet = null;
            UpdateSlotUI();
        }
    }

    public void PickupToken(SafeBracelet bracelet)
    {
        if (_heldBracelet != null) return;
        Debug.Log("[inventory] Ramassage du bracelet");
        _heldBracelet = bracelet;
        UpdateSlotUI();
        promptText.SetActive(true);
    }

    private void UpdateSlotUI()
    {
        tokenSlotImage.sprite = (_heldBracelet == null) ? emptySlotSprite : tokenSprite;
    }
}
