using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tooltip : MonoBehaviour
{
    private float timer = 0;
    private float lifespan = 5;
    private float opacity = 0;

    private CanvasGroup canvasGroup;

    // Update is called once per frame
    void FixedUpdate()
    {
        if (opacity < 1 && timer < 1)
        {
            // Fade in
            opacity += Time.deltaTime;
            canvasGroup.alpha = opacity;
        }
        else if (opacity > 0 && timer == lifespan - 1)
        {
            //Fade out
            opacity -= Time.deltaTime;
            canvasGroup.alpha = opacity;
        }
        else if (timer > lifespan)
        {
            // Destroy once faded out
            Destroy(this);
        }

        timer += Time.deltaTime;
    }

    private void OnEnable()
    {
        canvasGroup = GetComponentInChildren<CanvasGroup>();
    }
}
