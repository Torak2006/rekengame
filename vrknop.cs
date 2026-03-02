using UnityEngine;

public class VRKnop : MonoBehaviour
{
    private bool alGedrukt = false;

    void OnTriggerEnter(Collider other)
    {
        if (alGedrukt) return;

        if (other.CompareTag("left hand") || other.CompareTag("right hand"))
        {
            alGedrukt = true;
            GameManager gm = FindObjectOfType<GameManager>();
            gm.SluitAf();
        }
    }
}