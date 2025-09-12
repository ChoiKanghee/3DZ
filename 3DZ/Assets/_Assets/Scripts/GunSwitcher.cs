using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunSwitcher : MonoBehaviour
{
    public GameObject[] guns;
    private int currentGunIndex = 0;

    void Start()
    {
        SelectGun(currentGunIndex);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1)) SelectGun(0);
        if (Input.GetKeyDown(KeyCode.Alpha2)) SelectGun(1);
    }

    private void SelectGun(int index)
    {
        for (int i = 0; i < guns.Length; i++)
        {
            if (guns[i] != null)
                guns[i].SetActive(i == index);
        }

        currentGunIndex = index;
        Debug.Log("Selected gun: " + guns[currentGunIndex].name);
    }
}