using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum FruitType {
    One = 0,
    Two = 1,
    Three = 2,
    Four = 3,
    Five = 4,
    Six = 5,
    Seven = 6,
    Eight = 7,
    Nine = 8,
    Ten = 9,
    Eleven = 10,
}

/// <summary>
/// 默认Ready
/// 启动游戏或点击鼠标StandBy
/// 松开鼠标Dropping
/// 产生碰撞Collision
/// </summary>
public enum FruitState {
    Ready = 0,// 未创建
    StandBy = 1,// 待命
    Dropping = 2,// 掉落
    Collision = 3,// 碰撞（其他水果或墙体）
}

public class Fruit : MonoBehaviour {

    // Unity,在脚本中Public的变量或者参数，可以在Unity引擎 / Inspector检查器视图中修改
    // 最终取值会取检查器中的值
    public FruitType fruitType = FruitType.One;
    public FruitState fruitState = FruitState.Ready;
    private bool isMove = false;

    public float limit_x = 2.0f;

    public Vector3 originalScale = Vector3.zero;
    public float scaleSpeed = 0.1f;

    public float fruitScore = 1.0f;

    public Vector3 referenceResolution = new Vector2(480, 800); // 参考分辨率

    private void Awake() {
        // 在此处设置与在unity界面上输入作用相同
        //originalScale = new Vector3(0.5f, 0.5f, 0.5f);

        Vector3 currentFruitSize = this.gameObject.GetComponent<Transform>().localScale;
        // 调整大小
        //float scaleFactor = ResizePrefabBasedOnScreenSize();//fruitObj.gameObject.GetComponent<Transform>().position
        //originalScale = currentFruitSize * (1 - scaleFactor + 0.15f);
    }

    // Start is called before the first frame update
    // 在游戏对象启用时，调用一次
    void Start() {
    }

    // Update is called once per frame
    // 每帧执行一次，每帧的时间可设置（Time.deltaTime）
    // 在unity中每帧时间设置的路径为 Edit -> Project Settings -> Time - Fixed TimeStep
    void Update() {
        if (GameManager.gameManagerInstance.gameState == GameState.StandBy && fruitState == FruitState.StandBy) {
            if (Input.GetMouseButtonDown(0)) {
                isMove = true;
            }

            if (Input.GetMouseButtonUp(0) && isMove) {
                isMove = false;

                this.gameObject.GetComponent<Rigidbody2D>().gravityScale = 1.0f;
                GameManager.gameManagerInstance.gameState = GameState.InProgress;
                fruitState = FruitState.Dropping;

                GameManager.gameManagerInstance.InvokeCreateFruit(0.5f);
            }

            if (isMove) {
                // 限制鼠标只能在屏幕内移动
                //Cursor.lockState = CursorLockMode.Confined;
                Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);// 屏幕坐标转换成unity世界坐标
                Vector2 worldPosLeftBottom = Camera.main.ViewportToWorldPoint(Vector2.zero);// 视口坐标转换成unity世界坐标 左下0，0
                Vector2 worldPosTopRight = Camera.main.ViewportToWorldPoint(Vector2.one);// 视口坐标转换成unity世界坐标右上1，1

                // this指Fruit这个脚本
                // this.gameObject可以指向 如One的游戏对象
                // 再用GetComponent取游戏对象上面的 Transform

                var bounds = this.gameObject.GetComponent<Collider2D>().bounds;
                var fruitRadius = Mathf.Abs(bounds.max.x - bounds.center.x);

                // this.gameObject.GetComponent<Transform>().position = mousePosition; 【错误写法】会让z轴也取鼠标值，导致不在同一平面吧
                // this.gameObject.GetComponent<Transform>().position = new Vector3(mousePosition.x, this.gameObject.GetComponent<Transform>().position.y);
                // 另一种写法：
                // this.gameObject.GetComponent<Transform>().position = new Vector3(mousePosition.x, …….y, this.gameObject.GetComponent<Transform>().position.z);
                // 限制水果中心范围在屏幕内
                this.gameObject.GetComponent<Transform>().position = new Vector3(
                    Mathf.Clamp(mousePosition.x, worldPosLeftBottom.x + fruitRadius, worldPosTopRight.x - fruitRadius),
                    this.gameObject.GetComponent<Transform>().position.y);
            }
        }

        // 如果出问题--水果一直旋转导致无法下一个，limit_x最好设成紧贴墙壁
        //if (this.transform.position.x > limit_x) {
        //    this.transform.position = new Vector3(limit_x, this.transform.position.y);
        //}
        //if (this.transform.position.x < -limit_x) {
        //    this.transform.position = new Vector3(-limit_x, this.transform.position.y);
        //}

        if (this.transform.localScale.x < originalScale.x) {
            this.transform.localScale += new Vector3(1, 1, 1) * scaleSpeed;
        }

        if (this.transform.localScale.x >= originalScale.x) {
            this.transform.localScale = originalScale;
        }
    }

    // 碰撞监测：产生碰撞时就会一直检测
    /// <summary>
    /// 当前Fruit的游戏对象碰撞到collision时
    /// </summary>
    /// <param name="collision">被撞到的物体</param>
    void OnCollisionEnter2D(Collision2D collision) {
        //Debug.Log("collision");

        // 只有在Dropping下，游戏状态才会回滚为StandBy
        if (fruitState == FruitState.Dropping) {
            if (collision.gameObject.tag.Contains("Floor")) {
                GameManager.gameManagerInstance.gameState = GameState.StandBy;
                fruitState = FruitState.Collision;

                GameManager.gameManagerInstance.hitSource.Play();
            }
            if (collision.gameObject.tag.Contains("Fruit")) {
                GameManager.gameManagerInstance.gameState = GameState.StandBy;
                fruitState = FruitState.Collision;
            }
        }

        // Dropping，Collision时，水果可以进行合成
        if ((int)fruitState >= (int)FruitState.Dropping 
            && collision.gameObject.tag.Contains("Fruit") 
            && fruitType == collision.gameObject.GetComponent<Fruit>().fruitType 
            && fruitType != FruitType.Eleven) {
            float thisPosXY = this.transform.position.x + this.transform.position.y;
            float collisionPosXY = collision.transform.position.x + collision.transform.position.y;
            if (thisPosXY > collisionPosXY) {
                GameManager.gameManagerInstance.CombineNewFruit(fruitType, this.transform.position, collision.transform.position);

                GameManager.gameManagerInstance.totalScore += fruitScore;
                GameManager.gameManagerInstance.totalScoreText.text = GameManager.gameManagerInstance.totalScore.ToString();

                Destroy(this.gameObject);
                Destroy(collision.gameObject);

                Debug.Log(this.gameObject.GetComponent<Fruit>().fruitType + "@@@@" + collision.gameObject.GetComponent<Fruit>().fruitType);
            }
        }
    }

    /// <summary>
    /// 根据屏幕分辨率调整水果大小
    /// </summary>
    float ResizePrefabBasedOnScreenSize() {//Vector3 prefabSize
        // 获取当前屏幕分辨率
        Vector2 currentResolution = new Vector2(Screen.width, Screen.height);

        //Vector3 prefabSize = this.gameObject.GetComponent<Transform>().position;

        // 计算缩放比例
        float scaleFactorX = referenceResolution.x / currentResolution.x;
        float scaleFactorY = referenceResolution.y / currentResolution.y;
        return Mathf.Min(scaleFactorX, scaleFactorY); // 取较小的缩放比例

        // 创建预制件实例并调整大小
        //GameObject prefabInstance = Instantiate(prefab);
        //prefabInstance.transform.localScale = new Vector3(scaleFactor, scaleFactor, scaleFactor);
        //prefabInstance.transform.position = Vector3.zero; // 根据需要设置位置
    }
}
