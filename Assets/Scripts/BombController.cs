using System.Collections;
using UnityEngine;
using UnityEngine.Tilemaps;
using Photon.Pun;

public class BombController : MonoBehaviourPun
{
    [Header("Bomb")]
    public KeyCode inputKey = KeyCode.LeftShift;
    public GameObject bombPrefab;
    public float bombFuseTime = 3f;
    public int bombAmount = 1;
    private int bombsRemaining;

    [Header("Explosion")]
    public Explosion explosionPrefab;
    public LayerMask explosionLayerMask;
    public float explosionDuration = 1f;
    public int explosionRadius = 1;

    [Header("Destructible")]
    public Tilemap destructibleTiles;
    public Destructible destructiblePrefab;

    private void OnEnable()
    {
        bombsRemaining = bombAmount;
        destructibleTiles = GameObject.FindWithTag("Destructible").GetComponent<Tilemap>();

    }

    private void Update()
    {
        if (photonView.IsMine)
        {
            if (bombsRemaining > 0 && Input.GetKeyDown(inputKey))
            {
                StartCoroutine(PlaceBomb());
            }
        }
    }
    [PunRPC]
    private IEnumerator PlaceBomb()
    {
        Vector2 position = transform.position;
        position.x = Mathf.Round(position.x);
        position.y = Mathf.Round(position.y);

        GameObject bomb = PhotonNetwork.Instantiate("Bomb", position, Quaternion.identity);
        bombsRemaining--;

        yield return new WaitForSeconds(bombFuseTime);

        position = bomb.transform.position;
        position.x = Mathf.Round(position.x);
        position.y = Mathf.Round(position.y);


        //PROBLEMA FALTA PHOTONETWORK.INSTANTIATE
        Explosion explosion = PhotonNetwork.Instantiate(explosionPrefab.name, position, Quaternion.identity).GetComponent<Explosion>();
        //explosion.SetActiveRenderer(explosion.start);
        explosion.photonView.RPC("DestroyAfter", RpcTarget.All, explosionDuration);
        explosion.SetActiveRenderer(explosion.start);

        Explode(position, Vector2.up, explosionRadius);
        Explode(position, Vector2.down, explosionRadius);
        Explode(position, Vector2.left, explosionRadius);
        Explode(position, Vector2.right, explosionRadius);

        PhotonNetwork.Destroy(bomb);
        bombsRemaining++;
    }

    [PunRPC]
    private void Explode(Vector2 position, Vector2 direction, int length)
    {
        if (length <= 0) {
            return;
        }

        position += direction;

        if (Physics2D.OverlapBox(position, Vector2.one / 2f, 0f, explosionLayerMask))
        {
            //ClearDestructible(position);
            photonView.RPC("ClearDestructible", RpcTarget.All, position);
            return;
        }

        Explosion explosion = PhotonNetwork.Instantiate(explosionPrefab.name, position, Quaternion.identity).GetComponent<Explosion>();
        explosion.SetActiveRenderer(length > 1 ? explosion.middle : explosion.end);
        explosion.photonView.RPC("SetDirection", RpcTarget.All,direction);
        //explosion.DestroyAfter(explosionDuration);
        explosion.photonView.RPC("DestroyAfter",RpcTarget.All, explosionDuration);

        Explode(position, direction, length - 1);
    }

    [PunRPC]
    private void ClearDestructible(Vector2 position)
    {
        Vector3Int cell = destructibleTiles.WorldToCell(position);
        TileBase tile = destructibleTiles.GetTile(cell);

        if (tile != null)
        {
            Instantiate(destructiblePrefab, position, Quaternion.identity);
            destructibleTiles.SetTile(cell, null);
        }
    }

    public void AddBomb()
    {
        bombAmount++;
        bombsRemaining++;
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Bomb")) {
            other.isTrigger = false;
        }
    }

}
