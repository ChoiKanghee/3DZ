using UnityEngine;
using TMPro; // dùng cho TextMeshPro UI
using System.Collections;

public class AK47GunfireController : MonoBehaviour
{
    // --- Audio ---
    [Header("Audio Settings")]
    public AudioClip GunShotClip;
    public AudioClip ReloadClip;
    public AudioSource source;
    public Vector2 audioPitch = new Vector2(.9f, 1.1f);

    // --- Muzzle ---
    [Header("Muzzle Settings")]
    public GameObject muzzlePrefab;
    public GameObject muzzlePosition;

    // --- Config ---
    [Header("Config")]
    public string gunName = "AK47";
    public bool autoFire = true;
    public float shotDelay = 0.1f;

    // --- Options ---
    [Header("Scope Settings")]
    public GameObject scope;
    public bool scopeActive = true;
    private bool lastScopeState;

    // --- Projectile ---
    [Header("Projectile Settings")]
    public GameObject projectilePrefab;
    public GameObject projectileToDisableOnFire;

    // --- Ammo ---
    [Header("Ammo Settings")]
    public int magazineSize = 30;
    public int currentAmmo;
    public float reloadTime = 2f;
    public bool isReloading = false;

    // --- UI ---
    [Header("UI Ammo Display")]
    public TextMeshProUGUI ammoText;

    // --- Timing ---
    [SerializeField] private float timeLastFired;

    private void Start()
    {
        if (source != null) source.clip = GunShotClip;
        timeLastFired = 0;
        lastScopeState = scopeActive;
        currentAmmo = magazineSize;
        UpdateAmmoUI();
    }

    private void Update()
    {
        if (isReloading) return;

        // --- Reload ---
        if (Input.GetKeyDown(KeyCode.R))
        {
            StartCoroutine(Reload());
            return;
        }

        // --- Switch auto/semi ---
        if (Input.GetKeyDown(KeyCode.B))
        {
            autoFire = !autoFire;
            Debug.Log($"{gunName} switched to " + (autoFire ? "AUTO" : "SEMI"));
        }

        // --- Fire ---
        if (Input.GetMouseButtonDown(0) && !autoFire)
        {
            TryShoot();
        }
        else if (Input.GetMouseButton(0) && autoFire)
        {
            if ((timeLastFired + shotDelay) <= Time.time)
                TryShoot();
        }

        // --- Toggle scope ---
        if (scope && lastScopeState != scopeActive)
        {
            lastScopeState = scopeActive;
            scope.SetActive(scopeActive);
        }
    }

    private void TryShoot()
    {
        if (currentAmmo <= 0)
        {
            Debug.Log($"{gunName} hết đạn! Nhấn R để reload.");
            return;
        }
        FireWeapon();
        currentAmmo--;
        UpdateAmmoUI();
    }

    public void FireWeapon()
    {
        timeLastFired = Time.time;

        if (muzzlePrefab && muzzlePosition)
            Instantiate(muzzlePrefab, muzzlePosition.transform);

        if (projectilePrefab && muzzlePosition)
            Instantiate(projectilePrefab, muzzlePosition.transform.position, muzzlePosition.transform.rotation);

        if (projectileToDisableOnFire)
        {
            projectileToDisableOnFire.SetActive(false);
            Invoke("ReEnableDisabledProjectile", 3);
        }

        if (source && GunShotClip)
        {
            source.pitch = Random.Range(audioPitch.x, audioPitch.y);
            source.PlayOneShot(GunShotClip);
        }
    }

    private IEnumerator Reload()
    {
        isReloading = true;
        Debug.Log($"Reloading {gunName}...");

        if (source && ReloadClip)
            source.PlayOneShot(ReloadClip);

        yield return new WaitForSeconds(reloadTime);

        currentAmmo = magazineSize;
        isReloading = false;
        UpdateAmmoUI();
    }

    private void UpdateAmmoUI()
    {
        if (ammoText != null)
            ammoText.text = currentAmmo + " / " + magazineSize;
    }

    private void ReEnableDisabledProjectile()
    {
        if (projectileToDisableOnFire)
            projectileToDisableOnFire.SetActive(true);
    }
}
