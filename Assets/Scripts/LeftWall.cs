using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeftWall : MonoBehaviour {

    // Start is called before the first frame update
    void Start() {
        Vector2 worldPosLeftBottom = Camera.main.ViewportToWorldPoint(Vector2.zero);// 视口坐标转换成unity世界坐标 左下0，0

        var bounds = this.gameObject.GetComponent<Collider2D>().bounds;
        var bounder = Mathf.Abs(bounds.max.x - bounds.center.x);

        this.gameObject.GetComponent<Transform>().position = new Vector3(
            worldPosLeftBottom.x - bounder, this.gameObject.GetComponent<Transform>().position.y);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
