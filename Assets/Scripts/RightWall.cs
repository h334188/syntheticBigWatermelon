using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RightWall : MonoBehaviour {

    // Start is called before the first frame update
    void Start() {
        Vector2 worldPosTopRight = Camera.main.ViewportToWorldPoint(Vector2.one);// 视口坐标转换成unity世界坐标右上1，1

        var bounds = this.gameObject.GetComponent<Collider2D>().bounds;
        var bounder = Mathf.Abs(bounds.max.x - bounds.center.x);

        this.gameObject.GetComponent<Transform>().position = new Vector3(
            worldPosTopRight.x + bounder, this.gameObject.GetComponent<Transform>().position.y);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
