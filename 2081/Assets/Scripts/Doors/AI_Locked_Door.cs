using UnityEngine;

public class AI_Locked_Door : Door
{

    [SerializeField] private AITV AITV;

    protected override void Awake()
    {
        base.Awake();
        if (!AITV)
        {
            Debug.LogError($"{GetType()}.Awake: AITV was not assigned");
            return;
        }

        AITV.OnCompleteVoiceOver += OnCompleteVoiceOver;
    }

    protected void OnCompleteVoiceOver(object sender, System.EventArgs e)
    {
        OpenDoor();
    }

    public new bool OnInteract()
    {
        return false;
    }

}
