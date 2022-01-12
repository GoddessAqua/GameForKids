using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;
using DG.Tweening;
using System.Collections;

public class CardGrid: MonoBehaviour
{
    private int _gridRows = 1;
    public int GridRows 
    {
        set
        {
            _gridRows = value;
        }
        get
        {
            return _gridRows;
        }
    }
    private const int _gridCols = 3; 
    public int GridCols
    {
        get
        {
            return _gridCols;
        }
    }
    private float _offsetX = 3f;
    private float _offsetY = -3f;
    [SerializeField] private Cards cards;
    private List<int> _isAlreadyUsedIndexes = new List<int>();
    private int _index;
    [SerializeField] private GameObject _original;
    private GameObject [] _arrayOfGameObjects;
    public GameObject[] GetMassOfGameObjects => _arrayOfGameObjects;
    private string [] _idMass;
    public string [] GetMassOfIndexes => _idMass;
    private string _task;
    public string GetTask => _task;
    [SerializeField] private Text _taskLabel;
    private string _previousGussed;
    private RestartGame _restartGame;

    void Start()
    {
       _restartGame = FindObjectOfType<RestartGame>();
       StartLevelGeneration(_gridRows,_gridCols);
    }
    
    public void StartLevelGeneration(int gridRows, int gridCols)
    {
        var counter = 0;
        if (_gridRows <= 3)
        {
            FindObjectOfType<RestartGame>().enabled = false;

            _isAlreadyUsedIndexes = new List<int>();
            _arrayOfGameObjects = new GameObject[_gridCols * _gridRows];
            _idMass = new string[_gridCols * _gridRows];
            
            Vector3 startPos = _original.transform.position;
            GameObject card = _original;
            
            for(int i = 0; i < gridCols; i++)
            {
                for(int j = 0; j < gridRows; j++)
                {
                    while(_isAlreadyUsedIndexes.Any(x => x == _index))
                    {
                        _index = Random.Range(0, cards.ArrayOfCards.Length);
                    }
                    
                    card = Instantiate(_original);
                    _arrayOfGameObjects[counter] = card;
                    
                    card.GetComponent<SpriteRenderer>().sprite = cards.ArrayOfCards[_index].Sp;
                    card.GetComponent<MouseTouch>().Id = cards.ArrayOfCards[_index].Id;
                    
                    _idMass[counter] = cards.ArrayOfCards[_index].Id;
                    _isAlreadyUsedIndexes.Add(_index);
                    
                    float posX = (_offsetX * i) + startPos.x;
                    float posY = (_offsetY * j) + startPos.y;
                    
                    card.transform.position = new Vector3(posX, posY, startPos.z);
                    counter++;
                    
                    if (_gridRows == 1)
                        card.transform.DOScale(new Vector3(1.2f,1.2f,1.2f), 2f).SetLoops(2, LoopType.Yoyo);
                }
            }
            _task = _idMass[Random.Range(0, _idMass.Length)];
            while(_task == _previousGussed)
            {
                _task = _idMass[Random.Range(0, _idMass.Length)];
            }
            _previousGussed = _task;
            _taskLabel.text = "Find " + _task;
            if (_gridRows == 1)
            {
                _taskLabel.color = new Color(_taskLabel.color.r,_taskLabel.color.g,_taskLabel.color.b,0);
                StartCoroutine(Wait());
                _taskLabel.DOFade(1f, 1f);
            }
        }
        else
        {
            GameObject.Find("Background").GetComponent<SpriteRenderer>().DOFade(0.5f, 0.5f);
            _taskLabel.enabled = false;
            _restartGame.enabled = true;
            _restartGame.GetComponent<SpriteRenderer>().enabled = true;
            _restartGame.GetComponent<SpriteRenderer>().transform.position = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width / 2, Screen.height / 2, 10));
        }
    }

    IEnumerator Wait()
    {
        yield return new WaitForSeconds(2f);
    }
}
