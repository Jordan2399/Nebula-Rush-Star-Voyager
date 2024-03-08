using System.Collections;
using UnityEngine;

public class BossEnemyControls : MonoBehaviour
{
    [SerializeField] private float speed = 5f; // Adjust the speed as needed
    private Vector2 objectSize;
    [SerializeField] private GameObject normalBulletPrefab;
    [SerializeField] private GameObject specialBulletPrefab;
    [SerializeField] private float normalBulletInterval = 2f; // Time between normal bullet spawns
    [SerializeField] private float specialBulletInterval = 5f; // Time between special bullet spawns
    [SerializeField] private Transform firePointN;
    [SerializeField] private Transform firePointS1;
    [SerializeField] private Transform firePointS2;
    [SerializeField] private float bulletNSpeed = 5f; // Adjust the speed as needed
    [SerializeField] private float bulletSSpeed = 10f; // Adjust the speed as needed
    private GameObject player;

    
    [SerializeField] private float initialLaserDelay = 10f; // Time before the first laser fires
    [SerializeField] private Animator firePointAnimator; // Assume this is the animator for your 'preparing to fire' animation

    private void Start()
    {
        player = PlayerManager.instance.player;
        objectSize = GetObjectBoundsSize();
        // InvokeRepeating("SpawnNormalBullet", 0f, normalBulletInterval);
        // InvokeRepeating("SpawnSpecialBullet", 0f, specialBulletInterval);
        // Start the delayed laser firing coroutine instead of invoking it directly
        StartCoroutine(DelayedLaserFire());
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

        // Example: Rotate the boss as it moves
        // transform.Rotate(Vector3.forward * Time.deltaTime * 10f);
    }

    private void SpawnNormalBullet()
    {
        var directionToPlayer = getPlayerDirection();
        // Instantiate a normal bullet at the boss's position
        var bulletN = Instantiate(normalBulletPrefab, firePointN.position, firePointN.rotation);
        var rigidbody = bulletN.GetComponent<Rigidbody2D>();

        if (rigidbody)
        {
            // Set the bullet velocity towards the player
            rigidbody.velocity = directionToPlayer * bulletNSpeed;
            faceTowardsPlayer(directionToPlayer, rigidbody);
        }
    }

    private void SpawnSpecialBullet()
    {
        // var directionToPlayer = getPlayerDirection();
        // // Instantiate a special bullet at the boss's position
        // var bulletS1 =Instantiate(specialBulletPrefab, firePointS1.position, firePointS1.rotation);
        // var bulletS2 =Instantiate(specialBulletPrefab, firePointS2.position, firePointS2.rotation);
        // var rigidbody1 = bulletS1.GetComponent<Rigidbody2D>();
        // var rigidbody2 = bulletS2.GetComponent<Rigidbody2D>();
        //
        // if (rigidbody1 && rigidbody2)
        // {
        //     // Set the bullet velocity towards the player
        //     rigidbody1.velocity = directionToPlayer * bulletSSpeed;
        //     rigidbody2.velocity = directionToPlayer * bulletSSpeed;
        //     faceTowardsPlayer(directionToPlayer, rigidbody1);
        //     faceTowardsPlayer(directionToPlayer, rigidbody2);
        //
        // }
        FireLaser(firePointS1);
        FireLaser(firePointS2);
    }
    private void FireLaser(Transform firePoint)
    {
        // Instantiate the laser as a child of firePoint
        GameObject laserInstance = Instantiate(specialBulletPrefab, firePoint.position, Quaternion.identity);
        laserInstance.transform.SetParent(firePoint, false); // Parent to the fire point

        // Set the laser's local position to zero to align it with the firePoint
        laserInstance.transform.localPosition = Vector3.zero;
        // laserInstance.transform.localRotation = Quaternion.identity;

        BossL1LaserController laserController = laserInstance.GetComponent<BossL1LaserController>();

        if (laserController != null)
        {
            laserController.ActivateLaser(firePoint); // Assuming the laser should fire upwards
        }
        else
        {
            Debug.LogError("LaserController script not found on the instantiated laser prefab.");
        }
    }





    private Vector3 getPlayerDirection()
    {
        // Update the player position every time we fire
        var playerPosition = player.transform.position;

        // Calculate the direction from the enemy to the player
        var directionToPlayer = (playerPosition - transform.position).normalized;
        return directionToPlayer;
    }

    private void faceTowardsPlayer(Vector3 directionToPlayer,Rigidbody2D bulletRigid)
    {
        // Rotate the bullet to face towards the direction it's moving
        var angle = Mathf.Atan2(directionToPlayer.y, directionToPlayer.x) * Mathf.Rad2Deg;
        bulletRigid.rotation = angle - 90f; // Adjust the angle if necessary
    }
    
    
    
    private IEnumerator DelayedLaserFire()
    {
        // Initial delay before firing the first laser
        yield return new WaitForSeconds(initialLaserDelay);

        while (true)
        {
            // Play the preparing to fire animation here
            firePointAnimator.SetTrigger("PrepareToFireBossL1Special");

            // Wait for the preparation animation to finish before firing
            // You should adjust the waiting time according to your animation length
            yield return new WaitForSeconds(1f); // For example, if the animation is 1 second long

            // Fire the lasers from the points
            FireLaser(firePointS1);
            FireLaser(firePointS2);

            // Wait for the special bullet interval before the next firing sequence
            yield return new WaitForSeconds(specialBulletInterval);
        }
    }
}