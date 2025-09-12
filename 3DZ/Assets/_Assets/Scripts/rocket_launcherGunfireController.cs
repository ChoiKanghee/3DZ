using UnityEngine;
using TMPro;
using System.Collections;

public class RocketLauncherController : MonoBehaviour
{
    [Header("Gun Info")]
    public string gunName = "B41";

    // --- Audio ---
    [Header("Audio Settings")]
    public AudioClip GunShotClip;
    public AudioClip ReloadClip;
    public AudioSource source;
    public Vector2 audioPitch = new Vector2(.95f, 1.05f);

    // --- Muzzle ---
    [Header("Muzzle Settings")]
    public GameObject muzzlePrefab;
    public GameObject muzzlePosition;

    // --- Projectile ---
    [Header("Projectile Settings")]
    [Tooltip("Prefab của rocket được instantiate khi bắn")]
    public GameObject projectilePrefab;
    [Tooltip("Model rocket gắn trên launcher (ẩn khi bắn)")]
    public GameObject projectileToDisableOnFire;

    // --- Config ---
    [Header("Config")]
    public bool rotate = false;
    public float rotationSpeed = 0.25f;

    // --- Ammo ---
    [Header("Ammo Settings")]
    public int magazineSize = 1;     // 1 viên cho rocket launcher
    public int currentAmmo;
    public float reloadTime = 3f;
    public bool isReloading = false;

    // --- UI ---
    [Header("UI Ammo Display")]
    public TextMeshProUGUI ammoText;   // gán trong Inspector
    public TextMeshProUGUI gunNameText; // (tuỳ chọn) hiển thị tên súng

    private void Start()
    {
        if (source != null) source.clip = GunShotClip;
        currentAmmo = magazineSize;
        UpdateAmmoUI();
        UpdateGunNameUI();
    }

    private void Update()
    {
        if (isReloading) return;

        // Xoay thử nghiệm nếu cần
        if (rotate)
        {
            transform.localEulerAngles = new Vector3(
                transform.localEulerAngles.x,
                transform.localEulerAngles.y + rotationSpeed,
                transform.localEulerAngles.z);
        }

        // Fire: click chuột trái (1 phát)
        if (Input.GetMouseButtonDown(0))
        {
            TryFire();
        }

        // Reload thủ công
        if (Input.GetKeyDown(KeyCode.R) && !isReloading)
        {
            StartCoroutine(ReloadCoroutine());
        }

        // Nếu hết đạn, tự reload nếu muốn (ở đây chúng ta tự reload)
        if (currentAmmo <= 0 && !isReloading)
        {
            StartCoroutine(ReloadCoroutine());
        }
    }

    private void TryFire()
    {
        if (isReloading) return;

        if (currentAmmo <= 0)
        {
            Debug.Log($"{gunName} - Hết đạn! Nhấn R để reload.");
            return;
        }

        FireWeapon();
        currentAmmo--;
        UpdateAmmoUI();
    }

    public void FireWeapon()
    {
        // Muzzle flash
        if (muzzlePrefab != null && muzzlePosition != null)
        {
            Instantiate(muzzlePrefab, muzzlePosition.transform);
        }

        // Spawn projectile
        if (projectilePrefab != null && muzzlePosition != null)
        {
            Instantiate(projectilePrefab, muzzlePosition.transform.position, muzzlePosition.transform.rotation);
        }

        // Ẩn model rocket gắn trên launcher nếu có
        if (projectileToDisableOnFire != null)
        {
            projectileToDisableOnFire.SetActive(false);
        }

        // Audio
        if (source != null && GunShotClip != null)
        {
            source.pitch = Random.Range(audioPitch.x, audioPitch.y);
            source.PlayOneShot(GunShotClip);
        }
    }

    private IEnumerator ReloadCoroutine()
    {
        isReloading = true;
        Debug.Log($"{gunName} Reloading...");

        // Play reload sound
        if (source != null && ReloadClip != null)
        {
            source.PlayOneShot(ReloadClip);
        }

        // Cập nhật UI nếu muốn hiển thị "Reloading..."
        if (ammoText != null)
        {
            ammoText.text = "Reloading...";
        }

        yield return new WaitForSeconds(reloadTime);

        // Re-enable visible rocket on launcher
        if (projectileToDisableOnFire != null)
        {
            projectileToDisableOnFire.SetActive(true);
        }

        currentAmmo = magazineSize;
        isReloading = false;
        UpdateAmmoUI();
    }

    private void UpdateAmmoUI()
    {
        if (ammoText != null)
        {
            ammoText.text = $"{currentAmmo} / {magazineSize}";
        }
    }

    private void UpdateGunNameUI()
    {
        if (gunNameText != null)
        {
            gunNameText.text = gunName;
        }
    }
}