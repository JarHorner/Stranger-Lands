using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class DamageFlashing
{
    #region Methods

    //if flash is active, and player not reloading (stops flashing when dying), sprite will flash
    public static void SpriteFlashing(float flashLength, float flashCounter, SpriteRenderer sprite)
    {
        if (flashCounter > flashLength * .99f)
        {
            sprite.color = new Color(sprite.color.r, sprite.color.g, sprite.color.b, 1f);
        }
        else if (flashCounter > flashLength * .82f)
        {
            sprite.color = new Color(sprite.color.r, sprite.color.g, sprite.color.b, 0.2f);
        }
        else if (flashCounter > flashLength * .66f)
        {
            sprite.color = new Color(sprite.color.r, sprite.color.g, sprite.color.b, 1f);
        }
        else if (flashCounter > flashLength * .49f)
        {
            sprite.color = new Color(sprite.color.r, sprite.color.g, sprite.color.b, 0.2f);
        }
        else if (flashCounter > flashLength * .33f)
        {
            sprite.color = new Color(sprite.color.r, sprite.color.g, sprite.color.b, 1f);
        }
        else if (flashCounter > flashLength * .16f)
        {
            sprite.color = new Color(sprite.color.r, sprite.color.g, sprite.color.b, 0.2f);
        }
        else if (flashCounter > 0)
        {
            sprite.color = new Color(sprite.color.r, sprite.color.g, sprite.color.b, 1f);
        }
    }



    #endregion
}
