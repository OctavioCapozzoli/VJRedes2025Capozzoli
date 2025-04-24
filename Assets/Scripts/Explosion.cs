using UnityEngine;
using Photon.Pun;
using Photon;

public class Explosion : MonoBehaviourPun
{
    public AnimatedSpriteRenderer start;
    public AnimatedSpriteRenderer middle;
    public AnimatedSpriteRenderer end;

    [PunRPC]
    public void SetActiveRenderer(AnimatedSpriteRenderer renderer)
    {
        start.enabled = renderer == start;
        middle.enabled = renderer == middle;
        end.enabled = renderer == end;
    }

    [PunRPC]
    public void SetDirection(Vector2 direction)
    {
        float angle = Mathf.Atan2(direction.y, direction.x);
        transform.rotation = Quaternion.AngleAxis(angle * Mathf.Rad2Deg, Vector3.forward);
    }

    [PunRPC]
    public void DestroyAfter(float seconds)
    {
        Destroy(gameObject, seconds);
    }

}
