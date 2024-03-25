using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

/// <summary>
/// 默认Ready,
/// 点击开始StandBy
/// 松开鼠标InProgress
/// 产生碰撞，回滚StandBy
/// 水果越界GameOver
/// 游戏结束后，延迟0.5s CalculateScore
/// </summary>
public enum GameState {
    Ready = 0,
    StandBy = 1,// 有水果待命
    InProgress = 2,// 有水果掉落（过程中）
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

    // AudioListener一个场景只能有一个，一般放在主相机上
    public AudioSource combineSource;
    public AudioSource hitSource;

    // 静态的实例，可以直接在别的类中使用
    public static GameManager gameManagerInstance;
    // 在游戏对象启用之前，调用一次
    private void Awake() {
        // 实例化
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
        // 游戏开始
        //Debug.Log("Start");
        CreateFruit();
        gameState = GameState.StandBy;
        startButton.SetActive(false);
    }

    public void InvokeCreateFruit(float invokeTime) {
        // 延迟time后调用某方法
        Invoke("CreateFruit", invokeTime);
    }

    public void CreateFruit() {
        int index = Random.Range(0, 5);// 0-4
        if (fruitList.Length > index && fruitList[index] != null) {
            GameObject fruitObj = fruitList[index];
            // 克隆水果
            var currentFruit = Instantiate(fruitObj, fruitBornPosition.transform.position, fruitBornPosition.transform.rotation);
            currentFruit.GetComponent<Fruit>().fruitState = FruitState.StandBy;
        }
    }

    /// <summary>
    /// 合成后的新水果需要有重力
    /// </summary>
    /// <param name="currentFruitType">当前碰撞的水果类型</param>
    /// <param name="currentPosition">当前碰撞的水果位置</param>
    /// <param name="collisionPosition">被碰撞（水果）的位置</param>
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
