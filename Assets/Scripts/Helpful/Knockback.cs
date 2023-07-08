using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Knockback
{
    public static void PushBack(Transform thisTransform, Rigidbody2D otherRb)
    {
        // if (otherRb.GetComponent<Enemy>() != null && otherRb.GetComponent<Enemy>().IsKnockable)
        // {
        //     Vector2 difference = otherRb.transform.position - thisTransform.position;
        //     difference = difference.normalized * 8f;
        //     otherRb.AddForce(difference, ForceMode2D.Impulse);
        //     otherRb.GetComponent<Enemy>().currentState = EnemyState.stagger;
        //     otherRb.GetComponent<Enemy>().Knock(otherRb, 0.2f);
        // }
        if (otherRb.GetComponent<PlayerController>() != null)
        {
            Vector2 difference = otherRb.transform.position - thisTransform.position;
            difference = difference.normalized * 8f;
            otherRb.AddForce(difference, ForceMode2D.Impulse);
            otherRb.GetComponent<PlayerController>().currentState = PlayerState.stagger;
            otherRb.GetComponent<PlayerController>().Knock(0.3f);
        }
    }
}
