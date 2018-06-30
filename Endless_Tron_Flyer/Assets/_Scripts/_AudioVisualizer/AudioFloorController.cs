using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class AudioFloorController : MonoBehaviour
{
    public GameObject[] FloorTiles;
    public float deactivateCD;

    FloorFadeOnAudio[] _components;
    private List<ActivatedTile> activatedTiles;

    void Start()
    {
        FloorTiles = GameObject.FindGameObjectsWithTag("AudioFloor");
        activatedTiles = new List<ActivatedTile>();
        _components = new FloorFadeOnAudio[FloorTiles.Length];

        for (int i = 0; i < FloorTiles.Length; i++)
        {
            _components[i] = FloorTiles[i].GetComponent<FloorFadeOnAudio>();
            FloorTiles[i].SetActive(false);
        }

        InvokeRepeating("ActivateFloor", 0, 1f);
    }

    void Update()
    {
        for (int i = 0; i < activatedTiles.Count; i++)
        {
            var tile = activatedTiles[i];

            if (_components[tile.Id].fadingIn == false &&
               _components[tile.Id].fadingOut == false &&
               tile.ReadyToDestroy && _components[tile.Id].isReady == true)
            {
                FloorTiles[tile.Id].SetActive(false);
                activatedTiles.RemoveAt(i);
            }
        }
    }

    void ActivateFloor()
    {
        if (activatedTiles.Count >= _components.Length)
            return;
        int newIndex = -1;

        while (newIndex < 0 || activatedTiles.Any(x => x.Id == newIndex))
        {
            newIndex = Random.Range(0, FloorTiles.Length);
        }

        var tile = new ActivatedTile() { Id = newIndex };

        activatedTiles.Add(tile);

        FloorTiles[tile.Id].SetActive(true);
        StartCoroutine(WaitToDestroy(deactivateCD, tile));
    }

    IEnumerator WaitToDestroy(float deactivateCD, ActivatedTile tile)
    {
        tile.ReadyToDestroy = false;
        yield return new WaitForSeconds(deactivateCD);
        tile.ReadyToDestroy = true;
    }

    private class ActivatedTile
    {
        public int Id { get; set; }
        public bool ReadyToDestroy { get; set; }
    }
}
