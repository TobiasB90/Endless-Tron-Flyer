using DG.Tweening;
using UnityEngine;


public class Anim_RotatingCube : MonoBehaviour {

    public GameObject CubeObj;
    public bool Sequence_01;
    public bool Sequence_02;

    // Use this for initialization
    void Start () {
        RotatingCube();
	}
	
    void RotatingCube()
    {
        if (Sequence_01)
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
        else if(Sequence_02)
        {
            Sequence mySequence2 = DOTween.Sequence();
            mySequence2.Append(transform.DORotate(new Vector3(-90, 0, 0), 1f, RotateMode.LocalAxisAdd));
            mySequence2.Append(transform.DOLocalMoveZ(-60f, 1f));

            mySequence2.Join(CubeObj.transform.DOLocalMoveY(17f, 1f));
            mySequence2.Join(CubeObj.transform.DOLocalMoveZ(-17f, 1f));

            mySequence2.Append(transform.DORotate(new Vector3(-90, 0, 0), 1f, RotateMode.LocalAxisAdd));
            mySequence2.Append(transform.DOLocalMoveZ(60f, 1f));
            mySequence2.Join(CubeObj.transform.DOLocalMoveY(-17f, 1f));

            mySequence2.SetLoops(-1, LoopType.Restart);
        }
    }
}
