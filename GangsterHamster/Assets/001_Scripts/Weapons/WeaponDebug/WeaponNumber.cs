using Commands.Weapon;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WeaponNumber : MonoBehaviour
{
    Text text;

    private void Start()
    {
        text = GetComponent<Text>();
    }
    private void Update()
    {
        text.text = WeaponManagement.curWeaponNumber.ToString();
    }
}
