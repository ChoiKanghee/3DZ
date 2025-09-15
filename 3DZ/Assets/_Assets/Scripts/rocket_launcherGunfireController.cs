using UnityEngine;
using TMPro;
using System.Collections;

public class RocketLauncherController : MonoBehaviour, IWeapon
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
    public TextMeshProUGUI ammoText;
    public TextMeshProUGUI gunNameText;

    private void Start()
    {
        if (source != null) source.clip = GunShotClip;
        currentAmmo = magazineSize;
        UpdateUI();
    }

    private void Update()
    {
        if (isReloading) return;

        if (rotate)
        {
            transform.localEulerAngles = new Vector3(
                transform.localEulerAngles.x,
                transform.localEulerAngles.y + rotationSpeed,
                transform.localEulerAngles.z);
        }

        if (Input.GetMouseButtonDown(0))
            TryFire();

        if (Input.GetKeyDown(KeyCode.R) && !isReloading)
            StartCoroutine(ReloadCoroutine());

        if (currentAmmo <= 0 && !isReloading)
            StartCoroutine(ReloadCoroutine());
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
        UpdateUI();
    }

    public void FireWeapon()
    {
        if (muzzlePrefab && muzzlePosition)
            Instantiate(muzzlePrefab, muzzlePosition.transform);

        if (projectilePrefab && muzzlePosition)
            Instantiate(projectilePrefab, muzzlePosition.transform.position, muzzlePosition.transform.rotation);

        if (projectileToDisableOnFire)
            projectileToDisableOnFire.SetActive(false);

        if (source && GunShotClip)
        {
            source.pitch = Random.Range(audioPitch.x, audioPitch.y);
            source.PlayOneShot(GunShotClip);
        }
    }

    private IEnumerator ReloadCoroutine()
    {
        isReloading = true;
        Debug.Log($"{gunName} Reloading...");

        if (source && ReloadClip)
            source.PlayOneShot(ReloadClip);

        if (ammoText) ammoText.text = "Reloading...";

        yield return new WaitForSeconds(reloadTime);

        if (projectileToDisableOnFire)
            projectileToDisableOnFire.SetActive(true);

        currentAmmo = magazineSize;
        isReloading = false;
        UpdateUI();
    }

    // --- IWeapon ---
    public void StopReload()
    {
        isReloading = false;
        StopAllCoroutines();
        UpdateUI();
    }

    public void UpdateUI()
    {
        if (ammoText) ammoText.text = $"{currentAmmo} / {magazineSize}";
        if (gunNameText) gunNameText.text = gunName;
    }
}
