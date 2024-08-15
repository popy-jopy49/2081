using UnityEngine;

public class LogEntryMenu : MonoBehaviour
{

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


    }

}
