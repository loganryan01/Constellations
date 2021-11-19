using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OutroController : MonoBehaviour
{
    [SerializeField] private PlayerController player;
    [SerializeField] private FadingUI fadingElements;

    private void OnTriggerStay(Collider collider)
    {
        if (player.IsPlayerFinished() && collider.tag == "Player")
        {
            fadingElements.FadeElement(fadingElements.GetFadeUI());
        }
    }
}