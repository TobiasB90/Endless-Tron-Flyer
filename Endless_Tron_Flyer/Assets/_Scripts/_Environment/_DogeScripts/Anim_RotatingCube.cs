using DG.Tweening;
using UnityEngine;


public class Anim_RotatingCube : MonoBehaviour {

    public GameObject CubeObj;
	// Use this for initialization
	void Start () {
        RotatingCube();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void RotatingCube()
    {
        Sequence mySequence1 = DOTween.Sequence();
        mySequence1.Append(transform.DORotate(new Vector3(-90, 0, 0), 1f, RotateMode.LocalAxisAdd));
        mySequence1.Append(transform.DOLocalMoveZ(60f, 1f));
        mySequence1.Join(CubeObj.transform.DOLocalMoveY(17f, 1f));
        mySequence1.Append(transform.DORotate(new Vector3(-90, 0, 0), 1f, RotateMode.LocalAxisAdd));
        mySequence1.Append(transform.DOLocalMoveZ(-60f, 1f));
        mySequence1.Join(CubeObj.transform.DOLocalMoveY(17f, 1f));
        mySequence1.Join(CubeObj.transform.DOLocalMoveZ(-17f, 1f));

        mySequence1.SetLoops(-1, LoopType.Restart);
    }
}
