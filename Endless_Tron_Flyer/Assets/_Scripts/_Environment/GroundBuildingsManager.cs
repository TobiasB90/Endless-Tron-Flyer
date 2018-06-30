using UnityEngine;

public class GroundBuildingsManager : MonoBehaviour {

    public float ChanceToActivateBuilding;

	// Use this for initialization
	void Start () {
        for (int i = 0; i < this.gameObject.transform.childCount; i++)
        {
            Transform Go = this.gameObject.transform.GetChild(i);
            int chance = Random.Range(1, 100);
            if(chance >= ChanceToActivateBuilding)
            {
                Go.gameObject.SetActive(false);
            }
        }
    }
	

}
