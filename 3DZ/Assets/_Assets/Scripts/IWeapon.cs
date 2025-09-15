using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IWeapon
{
    void StopReload();   // Hủy reload khi đổi súng
    void UpdateUI();     // Cập nhật ammo + tên
}
