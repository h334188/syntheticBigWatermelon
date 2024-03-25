using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

/// <summary>
/// Ĭ��Ready,
/// �����ʼStandBy
/// �ɿ����InProgress
/// ������ײ���ع�StandBy
/// ˮ��Խ��GameOver
/// ��Ϸ�������ӳ�0.5s CalculateScore
/// </summary>
public enum GameState {
    Ready = 0,
    StandBy = 1,// ��ˮ������
    InProgress = 2,// ��ˮ�����䣨�����У�
    GameOver = 3,
    CalculateScore = 4,
}

public class GameManager : MonoBehaviour
{
    public GameState gameState = GameState.Ready;
    public GameObject[] fruitList;

    public GameObject fruitBornPosition;
    public GameObject startButton;

    public Vector3 combineScale = new Vector3(0, 0, 0);

    public float totalScore = 0.0f;
    public TextMeshProUGUI totalScoreText;

    public TextMeshProUGUI highestScoreText;

    // AudioListenerһ������ֻ����һ����һ������������
    public AudioSource combineSource;
    public AudioSource hitSource;

    // ��̬��ʵ��������ֱ���ڱ������ʹ��
    public static GameManager gameManagerInstance;
    // ����Ϸ��������֮ǰ������һ��
    private void Awake() {
        // ʵ����
        gameManagerInstance = this;
    }

    // Start is called before the first frame update
    void Start() {
        float highestScore = PlayerPrefs.GetFloat("HighestScore");
        //Debug.Log("@@@@" + highestScore);
        highestScoreText.text = "HighestScore " + highestScore.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void StratGame() {
        // ��Ϸ��ʼ
        //Debug.Log("Start");
        CreateFruit();
        gameState = GameState.StandBy;
        startButton.SetActive(false);
    }

    public void InvokeCreateFruit(float invokeTime) {
        // �ӳ�time�����ĳ����
        Invoke("CreateFruit", invokeTime);
    }

    public void CreateFruit() {
        int index = Random.Range(0, 5);// 0-4
        if (fruitList.Length > index && fruitList[index] != null) {
            GameObject fruitObj = fruitList[index];
            // ��¡ˮ��
            var currentFruit = Instantiate(fruitObj, fruitBornPosition.transform.position, fruitBornPosition.transform.rotation);
            currentFruit.GetComponent<Fruit>().fruitState = FruitState.StandBy;
        }
    }

    /// <summary>
    /// �ϳɺ����ˮ����Ҫ������
    /// </summary>
    /// <param name="currentFruitType">��ǰ��ײ��ˮ������</param>
    /// <param name="currentPosition">��ǰ��ײ��ˮ��λ��</param>
    /// <param name="collisionPosition">����ײ��ˮ������λ��</param>
    public void CombineNewFruit(FruitType currentFruitType, Vector3 currentPosition, Vector3 collisionPosition) {
        Vector3 centerPosition = (currentPosition + collisionPosition) / 2;
        int index = (int)currentFruitType + 1;
        GameObject combineFruitObj = fruitList[index];
        var combineFruit = Instantiate(combineFruitObj, centerPosition, combineFruitObj.transform.rotation);

        combineFruit.GetComponent<Fruit>().fruitState = FruitState.Collision;
        combineFruit.GetComponent<Rigidbody2D>().gravityScale = 1.0f;
        combineFruit.transform.localScale = combineScale;

        combineSource.Play();
    }
}
