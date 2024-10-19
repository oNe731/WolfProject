using UnityEngine;

public class Quest_BlueSlime : MonoBehaviour
{
    public GameObject[] blueSlimes;

    public GameObject questCompletionObject;

    void Update()
    {
        bool allSlimesGone = true;

        for (int i = 0; i < blueSlimes.Length; i++)
        {
            if (blueSlimes[i] != null)
            {
                allSlimesGone = false;
                break;
            }
        }

        if (allSlimesGone)
        {
            questCompletionObject.SetActive(true);

            this.enabled = false;
        }
    }
}
