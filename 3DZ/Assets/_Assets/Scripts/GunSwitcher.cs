using UnityEngine;

public class GunSwitcher : MonoBehaviour
{
    public GameObject[] guns;
    private int currentGunIndex = 0;
    private IWeapon currentWeapon;

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
        if (index < 0 || index >= guns.Length) return;

        // Hủy reload súng cũ
        if (currentWeapon != null)
            currentWeapon.StopReload();

        // Tắt tất cả
        for (int i = 0; i < guns.Length; i++)
        {
            if (guns[i] != null)
                guns[i].SetActive(i == index);
        }

        currentGunIndex = index;

        // Lấy weapon mới
        currentWeapon = guns[currentGunIndex].GetComponent<IWeapon>();
        if (currentWeapon != null)
            currentWeapon.UpdateUI();

        Debug.Log("Selected gun: " + guns[currentGunIndex].name);
    }
}
