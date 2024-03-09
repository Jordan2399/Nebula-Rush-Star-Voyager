using System.Collections;
using UnityEngine;

public class BossEnemyControls : MonoBehaviour
{
    [SerializeField] private float speed = 5f; // Adjust the speed as needed
    private Vector2 objectSize;
    [SerializeField] private GameObject normalBulletPrefab;
    [SerializeField] private GameObject muzzleFlashObject;
    [SerializeField] private float muzzleDisplayTime = 0.1f; // Time in seconds to display the muzzle flash
    [SerializeField] private GameObject specialBulletPrefab;
    [SerializeField] private float normalBulletInterval = 2f; // Time between normal bullet spawns
    [SerializeField] private float specialBulletInterval = 5f; // Time between special bullet spawns
    [SerializeField] private float specialBulletDuration = 3f; // Time duration of special bullet firing
    [SerializeField] private Transform firePointN;
    [SerializeField] private Transform firePointS1;
    [SerializeField] private Transform firePointS2;
    [SerializeField] private Animator fireballAnimatorS1;
    [SerializeField] private Animator fireballAnimatorS2;
    [SerializeField] private float bulletNSpeed = 5f; // Adjust the speed as needed
    [SerializeField] private float bulletSSpeed = 10f; // Adjust the speed as needed

    private GameObject player;

    [SerializeField] private float initialLaserDelay = 10f;


    private void Start()
    {
        player = PlayerManager.instance.player;
        objectSize = GetObjectBoundsSize();
        InvokeRepeating("SpawnNormalBullet", 0f, normalBulletInterval);
        // InvokeRepeating("SpawnSpecialBullet", 0f, specialBulletInterval + specialBulletDuration + 1.5f);
        StartCoroutine(SpecialBulletRoutine());
    }

    private IEnumerator SpecialBulletRoutine()
    {
        yield return new WaitForSeconds(initialLaserDelay);
        while (true)
        {
            // Prepare and fire lasers at both fire points simultaneously
            StartCoroutine(PrepareAndFireLaser(firePointS1, fireballAnimatorS1));
            StartCoroutine(PrepareAndFireLaser(firePointS2, fireballAnimatorS2));

            // Wait for specialBulletInterval + laserDuration for the next cycle
            yield return new WaitForSeconds(specialBulletInterval + specialBulletDuration+1.5f);
        }
    }

 

 



    private void Update()
    {
        MoveBoss();
    }

    private Vector2 GetObjectBoundsSize()
    {
        var collider = GetComponent<Collider2D>(); //TODO: tryGetComponent!
        if (collider is not null)
        {
            var bounds = collider.bounds;
            return new Vector2(bounds.extents.x, bounds.extents.y);
        }
        else
        {
            Debug.LogWarning("No collider found on the object. Cannot determine bounds size.");
            return Vector2.zero;
        }
    }

    private void MoveBoss()
    {
        // Get the current position of the boss
        var currentPosition = transform.position;

        // Calculate the new position based on the y-axis movement
        var newY = Mathf.PingPong(Time.time * speed, Camera.main.orthographicSize * 2) - Camera.main.orthographicSize;

        // Calculate the screen boundaries
        var minY = Camera.main.ScreenToWorldPoint(new Vector3(0, 0, 0)).y + objectSize.y;
        var maxY = Camera.main.ScreenToWorldPoint(new Vector3(0, Screen.height, 0)).y - objectSize.y;

        // Clamp the newY position within the screen limits
        newY = Mathf.Clamp(newY, minY, maxY);

        // Update the boss position
        transform.position = new Vector3(currentPosition.x, newY, currentPosition.z);
    }

    private void SpawnNormalBullet()
    {
        StartCoroutine(ShowMuzzleFlash());
        var directionToPlayer = getPlayerDirection();
        var bulletN = Instantiate(normalBulletPrefab, firePointN.position, firePointN.rotation);
        var rigidbody = bulletN.GetComponent<Rigidbody2D>();

        if (rigidbody)
        {
            rigidbody.velocity = directionToPlayer * bulletNSpeed;
            faceTowardsPlayer(directionToPlayer, rigidbody);
        }
    }

 


    private Vector3 getPlayerDirection()
    {
        var playerPosition = player.transform.position;

        var directionToPlayer = (playerPosition - transform.position).normalized;
        return directionToPlayer;
    }

    private void faceTowardsPlayer(Vector3 directionToPlayer, Rigidbody2D bulletRigid)
    {
        var angle = Mathf.Atan2(directionToPlayer.y, directionToPlayer.x) * Mathf.Rad2Deg;
        bulletRigid.rotation = angle - 90f; // Adjust the angle if necessary
    }


    private IEnumerator PrepareAndFireLaser(Transform firePoint, Animator fireballAnimator)
    {
        // Start the preparation animation
        fireballAnimator.SetBool("IsPreparing", true);
    
        // Wait for the animation to reach its end
        yield return new WaitUntil(() => 
            fireballAnimator.GetCurrentAnimatorStateInfo(0).IsName("FireballPreparation") &&
            fireballAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f);
    
    
        // Fire the laser after the preparation is done
        FireLaser(firePoint);
        // Turn off preparation
        fireballAnimator.SetBool("IsPreparing", false);
    
        // Wait for the laser to be done
        yield return new WaitForSeconds(specialBulletDuration);
    
        // Add delay for next firing sequence to start
        yield return new WaitForSeconds(specialBulletInterval);
    }


    
    private void FireLaser(Transform firePoint)
    {
        GameObject laserInstance = Instantiate(specialBulletPrefab, firePoint.position, Quaternion.identity, firePoint);
        laserInstance.transform.localPosition = Vector3.zero;
        laserInstance.transform.localRotation = Quaternion.identity;


        BossL1LaserController laserController = laserInstance.GetComponent<BossL1LaserController>();
        if (laserController != null)
        {
            laserController.ActivateLaser(specialBulletDuration);
        }
        else
        {
            Debug.LogError("LaserController script not found on the instantiated laser prefab.");
        }
    }


    // private void FireLaser(Transform firePoint)
    // {
    //     // GameObject laserInstance = Instantiate(specialBulletPrefab, firePoint.position, Quaternion.identity);
    //     GameObject laserInstance = Instantiate(specialBulletPrefab, firePoint.position, Quaternion.identity, firePoint);
    //
    //     // laserInstance.transform.SetParent(firePoint, false);
    //     laserInstance.transform.localPosition = Vector3.zero;
    //     laserInstance.transform.localRotation = Quaternion.identity;
    //
    //
    //     BossL1LaserController laserController = laserInstance.GetComponent<BossL1LaserController>();
    //     if (laserController != null)
    //     {
    //         laserController.ActivateLaser(firePoint, specialBulletDuration);
    //     }
    //     else
    //     {
    //         Debug.LogError("LaserController script not found on the instantiated laser prefab.");
    //     }
    // }
    
    
    private IEnumerator ShowMuzzleFlash()
    {
        muzzleFlashObject.SetActive(true); // Show the muzzle flash
        yield return new WaitForSeconds(muzzleDisplayTime); // Wait for the specified duration
        muzzleFlashObject.SetActive(false); // Hide the muzzle flash
    }
}