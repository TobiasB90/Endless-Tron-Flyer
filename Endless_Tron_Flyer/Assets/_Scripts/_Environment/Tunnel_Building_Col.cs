using UnityEngine;

public class Tunnel_Building_Col : MonoBehaviour {

    public LayerMask BuildingLayer;
    
    public void OnTriggerEnter(Collider other)
    {
        if ((BuildingLayer & 1 << other.gameObject.layer) == 1 << other.gameObject.layer)
        {
            Destroy(other.gameObject);
        }
    }
}
