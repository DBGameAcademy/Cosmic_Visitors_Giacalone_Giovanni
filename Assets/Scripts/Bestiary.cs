using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Bestiary : MonoBehaviour
{
    [SerializeField] AlienScriptable baseAlien;
    [SerializeField] AlienScriptable shieldAlien;
    [SerializeField] AlienScriptable specialAlien;
    [SerializeField] Player player;

    [SerializeField] GameObject damageBaseAlien;
    [SerializeField] GameObject lifeBaseAlien;

    [SerializeField] GameObject damageShieldAlien;
    [SerializeField] GameObject lifeShieldAlien;

    [SerializeField] GameObject damageSpecialAlien;
    [SerializeField] GameObject lifeSpecialAlien;

    [SerializeField] GameObject damagePlayer;
    [SerializeField] GameObject lifePlayer;

    private void Awake()
    {
        damageBaseAlien.GetComponent<TextMeshProUGUI>().text = "DAMAGE = " + baseAlien.damage;
        lifeBaseAlien.GetComponent<TextMeshProUGUI>().text = "LIFE = " + baseAlien.health;

        damageShieldAlien.GetComponent<TextMeshProUGUI>().text = "DAMAGE = " + shieldAlien.damage;
        lifeShieldAlien.GetComponent<TextMeshProUGUI>().text = "LIFE = " + shieldAlien.health;

        damageSpecialAlien.GetComponent<TextMeshProUGUI>().text = "DAMAGE = " + specialAlien.damage;
        lifeSpecialAlien.GetComponent<TextMeshProUGUI>().text = "LIFE = " + specialAlien.health;

        damagePlayer.GetComponent<TextMeshProUGUI>().text = "DAMAGE = " + player.damage;
        lifePlayer.GetComponent<TextMeshProUGUI>().text = "LIFE = " + player.Health;
    }
}
