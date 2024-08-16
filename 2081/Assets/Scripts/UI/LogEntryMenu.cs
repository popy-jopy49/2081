using Michsky.UI.Heat;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LogEntryMenu : MonoBehaviour
{

    [SerializeField] private TMP_Text nameText;
    [SerializeField] private TMP_Text mainText;
    [SerializeField] private Image coverImage;
    [SerializeField] private RectTransform layoutGroupParent;

    private void Awake()
    {
        // On Awake, find all lore tablets
        LoreTabletO[] loreTablets = FindObjectsByType<LoreTabletO>(FindObjectsSortMode.None);
        foreach (LoreTabletO loreTablet in loreTablets)
        {
            loreTablet.OnLorePickUp += OnLorePickUp;
        }
    }

    private void OnLorePickUp(object sender, int logNumber)
    {
        // Unsubscribe from this one to avoid errors
        LoreTabletO loreTablet = sender as LoreTabletO;
        loreTablet.OnLorePickUp -= OnLorePickUp;

		ButtonManager button = Instantiate(GameAssets.I.LogButton, layoutGroupParent).GetComponent<ButtonManager>();
		Instantiate(GameAssets.I.Spacer, layoutGroupParent);
        button.buttonText = $"Log {logNumber + 1}";
        button.onClick.AddListener(() => 
        {
		    GameAssets.LogEntry logEntry = GameAssets.I.LogEntries[logNumber];
		    nameText.text = logEntry.Name;
		    mainText.text = logEntry.Contents;
		    if (logEntry.CoverImage)
			    coverImage.sprite = logEntry.CoverImage;
	    });
    }

}
