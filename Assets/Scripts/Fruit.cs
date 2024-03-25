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
/// Ĭ��Ready
/// ������Ϸ�������StandBy
/// �ɿ����Dropping
/// ������ײCollision
/// </summary>
public enum FruitState {
    Ready = 0,// δ����
    StandBy = 1,// ����
    Dropping = 2,// ����
    Collision = 3,// ��ײ������ˮ����ǽ�壩
}

public class Fruit : MonoBehaviour {

    // Unity,�ڽű���Public�ı������߲�����������Unity���� / Inspector�������ͼ���޸�
    // ����ȡֵ��ȡ������е�ֵ
    public FruitType fruitType = FruitType.One;
    public FruitState fruitState = FruitState.Ready;
    private bool isMove = false;

    public float limit_x = 2.0f;

    public Vector3 originalScale = Vector3.zero;
    public float scaleSpeed = 0.1f;

    public float fruitScore = 1.0f;

    public Vector3 referenceResolution = new Vector2(480, 800); // �ο��ֱ���

    private void Awake() {
        // �ڴ˴���������unity����������������ͬ
        //originalScale = new Vector3(0.5f, 0.5f, 0.5f);

        Vector3 currentFruitSize = this.gameObject.GetComponent<Transform>().localScale;
        // ������С
        //float scaleFactor = ResizePrefabBasedOnScreenSize();//fruitObj.gameObject.GetComponent<Transform>().position
        //originalScale = currentFruitSize * (1 - scaleFactor + 0.15f);
    }

    // Start is called before the first frame update
    // ����Ϸ��������ʱ������һ��
    void Start() {
    }

    // Update is called once per frame
    // ÿִ֡��һ�Σ�ÿ֡��ʱ������ã�Time.deltaTime��
    // ��unity��ÿ֡ʱ�����õ�·��Ϊ Edit -> Project Settings -> Time - Fixed TimeStep
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
                // �������ֻ������Ļ���ƶ�
                //Cursor.lockState = CursorLockMode.Confined;
                Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);// ��Ļ����ת����unity��������
                Vector2 worldPosLeftBottom = Camera.main.ViewportToWorldPoint(Vector2.zero);// �ӿ�����ת����unity�������� ����0��0
                Vector2 worldPosTopRight = Camera.main.ViewportToWorldPoint(Vector2.one);// �ӿ�����ת����unity������������1��1

                // thisָFruit����ű�
                // this.gameObject����ָ�� ��One����Ϸ����
                // ����GetComponentȡ��Ϸ��������� Transform

                var bounds = this.gameObject.GetComponent<Collider2D>().bounds;
                var fruitRadius = Mathf.Abs(bounds.max.x - bounds.center.x);

                // this.gameObject.GetComponent<Transform>().position = mousePosition; ������д��������z��Ҳȡ���ֵ�����²���ͬһƽ���
                // this.gameObject.GetComponent<Transform>().position = new Vector3(mousePosition.x, this.gameObject.GetComponent<Transform>().position.y);
                // ��һ��д����
                // this.gameObject.GetComponent<Transform>().position = new Vector3(mousePosition.x, ����.y, this.gameObject.GetComponent<Transform>().position.z);
                // ����ˮ�����ķ�Χ����Ļ��
                this.gameObject.GetComponent<Transform>().position = new Vector3(
                    Mathf.Clamp(mousePosition.x, worldPosLeftBottom.x + fruitRadius, worldPosTopRight.x - fruitRadius),
                    this.gameObject.GetComponent<Transform>().position.y);
            }
        }

        // ���������--ˮ��һֱ��ת�����޷���һ����limit_x�����ɽ���ǽ��
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

    // ��ײ��⣺������ײʱ�ͻ�һֱ���
    /// <summary>
    /// ��ǰFruit����Ϸ������ײ��collisionʱ
    /// </summary>
    /// <param name="collision">��ײ��������</param>
    void OnCollisionEnter2D(Collision2D collision) {
        //Debug.Log("collision");

        // ֻ����Dropping�£���Ϸ״̬�Ż�ع�ΪStandBy
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

        // Dropping��Collisionʱ��ˮ�����Խ��кϳ�
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
    /// ������Ļ�ֱ��ʵ���ˮ����С
    /// </summary>
    float ResizePrefabBasedOnScreenSize() {//Vector3 prefabSize
        // ��ȡ��ǰ��Ļ�ֱ���
        Vector2 currentResolution = new Vector2(Screen.width, Screen.height);

        //Vector3 prefabSize = this.gameObject.GetComponent<Transform>().position;

        // �������ű���
        float scaleFactorX = referenceResolution.x / currentResolution.x;
        float scaleFactorY = referenceResolution.y / currentResolution.y;
        return Mathf.Min(scaleFactorX, scaleFactorY); // ȡ��С�����ű���

        // ����Ԥ�Ƽ�ʵ����������С
        //GameObject prefabInstance = Instantiate(prefab);
        //prefabInstance.transform.localScale = new Vector3(scaleFactor, scaleFactor, scaleFactor);
        //prefabInstance.transform.position = Vector3.zero; // ������Ҫ����λ��
    }
}
