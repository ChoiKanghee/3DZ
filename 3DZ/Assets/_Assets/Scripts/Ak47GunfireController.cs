using UnityEngine;
using TMPro;
using System.Collections;

public class AK47GunfireController : MonoBehaviour, IWeapon
{
    [Header("Audio Settings")]
    public AudioClip GunShotClip;
    public AudioClip ReloadClip;
    public AudioSource source;
    public Vector2 audioPitch = new Vector2(.9f, 1.1f);

    [Header("Muzzle Settings")]
    public GameObject muzzlePrefab;
    public GameObject muzzlePosition;

    [Header("Config")]
    public string gunName = "AK47";
    public bool autoFire = true;
    public float shotDelay = 0.1f;

    [Header("Scope Settings")]
    public GameObject scope;
    public bool scopeActive = true;
    private bool lastScopeState;

    [Header("Projectile Settings")]
    public GameObject projectilePrefab;
    public GameObject projectileToDisableOnFire;

    [Header("Ammo Settings")]
    public int magazineSize = 30;
    public int currentAmmo;
    public float reloadTime = 2f;
    public bool isReloading = false;

    [Header("UI Display")]
    public TextMeshProUGUI ammoText;
    public TextMeshProUGUI gunNameText;

    [SerializeField] private float timeLastFired;

    private void Start()
    {
        if (source != null) source.clip = GunShotClip;
        timeLastFired = 0;
        lastScopeState = scopeActive;
        currentAmmo = magazineSize;
        UpdateUI();
    }

    private void Update()
    {
        if (isReloading) return;

        if (Input.GetKeyDown(KeyCode.R))
        {
            StartCoroutine(Reload());
            return;
        }

        if (Input.GetKeyDown(KeyCode.B))
        {
            autoFire = !autoFire;
            Debug.Log($"{gunName} switched to " + (autoFire ? "AUTO" : "SEMI"));
        }

        if (Input.GetMouseButtonDown(0) && !autoFire)
            TryShoot();
        else if (Input.GetMouseButton(0) && autoFire)
        {
            if ((timeLastFired + shotDelay) <= Time.time)
                TryShoot();
        }

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
        UpdateUI();
    }

    public void FireWeapon()
    {
        timeLastFired = Time.time;

        if (muzzlePrefab && muzzlePosition)
            Instantiate(muzzlePrefab, muzzlePosition.transform.position, muzzlePosition.transform.rotation);

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

        if (ammoText) ammoText.text = "Reloading...";

        yield return new WaitForSeconds(reloadTime);

        currentAmmo = magazineSize;
        isReloading = false;
        UpdateUI();
    }

    private void ReEnableDisabledProjectile()
    {
        if (projectileToDisableOnFire)
            projectileToDisableOnFire.SetActive(true);
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
        if (ammoText) ammoText.text = currentAmmo + " / " + magazineSize;
        if (gunNameText) gunNameText.text = gunName;
    }
}
