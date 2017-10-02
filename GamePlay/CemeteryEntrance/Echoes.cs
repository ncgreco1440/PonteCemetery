using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PonteCemetery.GamePlay
{
    public class Echoes : MonoBehaviour
    {
        private float t; //Time
        private WaitForFixedUpdate fixedUpdateYield = new WaitForFixedUpdate();
        private WaitForSeconds secondsToWait = new WaitForSeconds(0.33f);

        private void OnTriggerEnter(Collider collider)
        {
            if(collider.gameObject.tag == "Player")
            {
                StartCoroutine(ExpandPlayerLostSphere());
            }
        }

        private IEnumerator ExpandPlayerLostSphere()
        {
            while(t < 3.0f)
            {
                t += Time.fixedDeltaTime;
                Player.LostIndication().radius += 1.25f;
                yield return secondsToWait;
            }
        }
    }
}