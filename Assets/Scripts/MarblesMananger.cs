using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MarblesMananger : MonoBehaviour
{
    //? ??? ??
    public float marblesVelocity = 500f;
    public float strong = 0.3f;
    Rigidbody marblesRigd;
    Vector3 shot;
    MeshRenderer mesh;
    void OnEnable()
    {
        //???
        if (marblesRigd == null) TryGetComponent(out marblesRigd);
        marblesRigd.velocity = Vector3.zero;
        marblesRigd.angularVelocity = Vector3.zero;

        //? (?? ?? - ????? , z ??)
        shot = new Vector3((transform.position.x) - Camera.main.transform.position.x, (transform.position.y) - Camera.main.transform.position.y, marblesVelocity); ;
        //shot.Normalize();
        //( ? ??? + ??? (y) ) * ? , ForceMode.Impulse (????? ?? ???)
        marblesRigd.AddForce((shot + new Vector3(0, 1f, 0)) * strong, ForceMode.Impulse);


        gameObject.TryGetComponent(out mesh);
        mesh.material.mainTextureOffset = Vector2.zero;
        Invoke(nameof(OnDisplayNoneActive), 5f);
    }

    void OnDisplayNoneActive()
    {
        gameObject.SetActive(false);
    }

    void FixedUpdate()
    {
        mesh.material.mainTextureOffset -= Vector2.up * Time.deltaTime;
    }

}
