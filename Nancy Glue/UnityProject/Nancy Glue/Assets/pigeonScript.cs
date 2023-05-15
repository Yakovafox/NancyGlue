using UnityEngine;

public class pigeonScript : MonoBehaviour
{
    [SerializeField] private SpriteRenderer _spriteRenderer;
    [SerializeField] private Sprite[] _sprites = new Sprite[3];
    [SerializeField] private float _timer;
    [SerializeField] private float _targetTimer;
    [SerializeField] private int _index;
    private bool _timerSet;
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        //switch through the different pigeon sprites based on a random timer.

        if (!_timerSet)
        {
            switch(_index)
            {
                case 0:
                    _targetTimer = Random.Range(20f, 30f);
                    break;
                case 1:
                    _targetTimer = Random.Range(1f, 10f);
                    break;
                case 2:
                    _targetTimer = Random.Range(2f, 3f);
                    break;
            }
            _timerSet = true;
        }
        if(_timerSet)
        {
            _timer += 1f * Time.deltaTime;
        }
        if(_timer >= _targetTimer)
        {
            _timer = 0;
            _timerSet = false;
            _index++;
            if (_index > 2)
                _index = 0;
            _spriteRenderer.sprite = _sprites[_index];
        }
    }
}
