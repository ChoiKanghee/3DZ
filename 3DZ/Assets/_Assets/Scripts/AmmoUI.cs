using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using TMPro;   // nếu bạn dùng TextMeshPro

public class AmmoUI : MonoBehaviour
{
    public AK47GunfireController weapon;   // kéo thả script súng vào đây
    public TextMeshProUGUI ammoText;       // kéo thả Text UI vào

    private void Update()
    {
        if (weapon == null || ammoText == null) return;

        if (weapon.isReloading)
        {
            ammoText.text = "Reloading...";
        }
        else
        {
            ammoText.text = $"{weapon.currentAmmo} / {weapon.magazineSize}";
        }
    }
}
