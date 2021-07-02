using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MarblesMananger : MonoBehaviour
{
    //공 가속도 계산
    public float marblesVelocity = 500f;
    public float strong = 0.3f;
    Rigidbody marblesRigd;
    Vector3 shot;
   
	void OnEnable()
    {
        //초기화
        if(marblesRigd == null) TryGetComponent(out marblesRigd);
        marblesRigd.velocity = Vector3.zero;
        marblesRigd.angularVelocity = Vector3.zero;

        //샷 (현재 위치 - 카메라위치 , z 속도)
        shot = new Vector3(transform.position.x - Camera.main.transform.position.x, transform.position.y - Camera.main.transform.position.y, marblesVelocity);
        shot.Normalize();
        //( 샷 포지션 + 포물선 (y) ) * 힘 , ForceMode.Impulse (당구공처럼 쎄게 나간다)
        marblesRigd.AddForce((shot + new Vector3(0, 1f, 0)) * strong, ForceMode.Impulse);
        Invoke(nameof(OnDisplayNoneActive), 5f);
    }

    void OnDisplayNoneActive()
    {
        gameObject.SetActive(false);
    }

}
