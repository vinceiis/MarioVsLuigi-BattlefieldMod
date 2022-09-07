using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Photon.Pun;

public class LoadingPower : MonoBehaviour {
    
    public int marioX = -296, peachX = 296, minX = -410;
    public Vector3 movementSpeed;
    public MarioLoader mario;
    private Animator animator;
    private RectTransform rect;
    private bool goomba, goombaHit;
    private float goombaTimer;
    public static bool GetCustomProperty<Sp>(string key, out Sp value, ExitGames.Client.Photon.Hashtable properties = null)
    {
        if (properties == null)
            properties = PhotonNetwork.CurrentRoom.CustomProperties;
        if (properties == null)
        {
            value = default;
            return false;
        }

        properties.TryGetValue(key, out object temp);
        if (temp != null)
        {
            value = (Sp)temp;
            return true;
        }
        else
        {
            value = default;
            return false;
        }
    }

    void Start() {
        animator = GetComponent<Animator>();
        rect = GetComponent<RectTransform>();
        GetCustomProperty(Enums.NetRoomProperties.SpawnWithMush, out bool mush);
        if (mush)
            mario.scale = 1;
        else mario.scale = 0;
    }

    void Update() {
        animator.SetBool("goomba", goomba);
        if ((goombaTimer -= Time.deltaTime) < 0)
            rect.localPosition += movementSpeed * Time.deltaTime;

        if (rect.localPosition.x <= marioX) {
            if (goomba) {
                if (!goombaHit) {
                    mario.scale--;
                    goombaHit = true;
                    mario.scaleTimer = 0f;
                    goombaTimer = 0.5f;
                }
                if (rect.localPosition.x <= minX)
                    Reset();
            } else {
                mario.scale++;
                mario.scaleTimer = 0f;
                Reset();
            }
        }
    }

    void Reset() {
        goombaHit = false;
        goomba = mario.scale > 0 && (mario.scale >= 2 || Random.value < 0.5f);
        rect.localPosition = new Vector2(peachX, rect.localPosition.y);
    }
}
