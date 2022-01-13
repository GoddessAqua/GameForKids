using System;
using UnityEngine;
using UnityEngine.EventSystems;
using DG.Tweening;
using System.Threading;
using System.Collections;

public class MouseTouch: MonoBehaviour,  IPointerDownHandler
{
    private CardGrid _dataFromCardCell;
    private Camera Cam;
    [SerializeField] private string _id;
    public string Id
    {
        get 
        {
            return _id;
        }
        set
        {
            _id = value;
        }
    } 
    private GameObject _choosenElement;

    [SerializeField] private ParticleSystem _stars;
    
    void Awake()
    {
        _stars.Stop();
    }
    
    void Start()
    {
        _stars.Stop();
        _dataFromCardCell = FindObjectOfType<CardGrid>();
        Cam = FindObjectOfType<Camera>();
    }
    
    public void OnPointerDown(PointerEventData eventData)
    {
        _stars.Stop();
        DOTween.CompleteAll();
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            RaycastHit2D aHit = new RaycastHit2D();
            aHit = Physics2D.Raycast(Cam.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
            _choosenElement = aHit.collider.gameObject;
            
            if (_dataFromCardCell.GetTask == _choosenElement.GetComponent<MouseTouch>()._id)
            {
                _choosenElement.transform.DOScale(new Vector3(1.2f,1.2f,1.2f), 2f).SetLoops(2, LoopType.Yoyo);
                StartCoroutine(Wait());
            }
            else
            {
                _stars.Play();
                StartCoroutine(WaitAnimation());    
            }
        }
    }

    void GenerateNewLevel()
    {
        foreach(var item in _dataFromCardCell.GetComponent<CardGrid>().GetMassOfGameObjects)
        {
            Destroy(item);
        }
        _dataFromCardCell.GridRows = _dataFromCardCell.GridRows + 1;
        _dataFromCardCell.StartLevelGeneration(_dataFromCardCell.GridRows, _dataFromCardCell.GridCols);
    }
    
    IEnumerator Wait()
    {
        yield return new WaitForSeconds(3.5f);
        GenerateNewLevel();
    }

    IEnumerator WaitAnimation()
    {
        Tween myTween = _choosenElement.transform.DOShakePosition(1f, strength: new Vector3(0,1,0), vibrato: 4, randomness: 1, snapping: false, fadeOut: true).SetLoops(1,LoopType.Yoyo);
        yield return myTween.WaitForCompletion();
        _stars.Stop();  
    }
}
