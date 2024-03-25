using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TopLineRule : MonoBehaviour {

    public bool isMove = false;
    public float moveSpeed = 0.1f;
    public float limit_y = -5f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update() {
        if (isMove) {
            if (this.transform.position.y > limit_y) {
                this.transform.Translate(Vector3.down * moveSpeed);
            } 
            else {
                isMove = false;

                // 重新加载游戏
                Invoke("ReloadScene", 1.0f);
            }
        }
    }

    // 碰撞触发
    void OnTriggerEnter2D(Collider2D collider) {
        if (collider.gameObject.tag.Contains("Fruit")) {
            if ((int)GameManager.gameManagerInstance.gameState < (int)GameState.GameOver
                && collider.gameObject.GetComponent<Fruit>().fruitState == FruitState.Collision) {
                GameManager.gameManagerInstance.gameState = GameState.GameOver;
                Invoke("MoveLineAndCalculateScore", 0.5f);
            }

            if (GameManager.gameManagerInstance.gameState == GameState.CalculateScore) {
                float currentScore = collider.GetComponent<Fruit>().fruitScore;
                GameManager.gameManagerInstance.totalScore += currentScore;
                GameManager.gameManagerInstance.totalScoreText.text = GameManager.gameManagerInstance.totalScore.ToString();
                Destroy(collider.gameObject);
            }
        }
    }

    void MoveLineAndCalculateScore() {
        isMove = true;
        GameManager.gameManagerInstance.gameState = GameState.CalculateScore;
    }

    void ReloadScene() {
        float highestScore = PlayerPrefs.GetFloat("HighestScore");
        if (highestScore < GameManager.gameManagerInstance.totalScore) { 
            PlayerPrefs.SetFloat("HighestScore", GameManager.gameManagerInstance.totalScore);
        }

        SceneManager.LoadScene("SBW2021");
    }
}
