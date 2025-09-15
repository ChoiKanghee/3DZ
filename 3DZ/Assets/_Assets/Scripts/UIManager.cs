using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    [Header("UI References")]
    public TextMeshProUGUI gunNameText;
    public TextMeshProUGUI ammoText;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    public void UpdateGunName(string name)
    {
        if (gunNameText) gunNameText.text = name;
    }

    public void UpdateAmmo(int current, int max)
    {
        if (ammoText) ammoText.text = current + " / " + max;
    }
}
