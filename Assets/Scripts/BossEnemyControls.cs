using UnityEngine;

public class BossEnemyControls : MonoBehaviour
{
	[SerializeField] private float speed = 5f; // Adjust the speed as needed
	[SerializeField] private Vector2 objectSize;
	[SerializeField] private GameObject normalBulletPrefab;
	[SerializeField] private GameObject specialBulletPrefab;
	[SerializeField] private float normalBulletInterval = 2f; // Time between normal bullet spawns
	[SerializeField] private float specialBulletInterval = 5f; // Time between special bullet spawns
	[SerializeField] private Transform firePointN;
	[SerializeField] private Transform firePointS1;
	[SerializeField] private Transform firePointS2;


	private void Start()
	{
		objectSize = GetObjectBoundsSize();
		InvokeRepeating("SpawnNormalBullet", 0f, normalBulletInterval);
		InvokeRepeating("SpawnSpecialBullet", 0f, specialBulletInterval);
	}

	void Update()
	{
		MoveBoss();
	}

	private Vector2 GetObjectBoundsSize()
	{
		var collider = GetComponent<Collider2D>(); //TODO: tryGetComponent!
		if (collider != null)
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

	void MoveBoss()
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
		transform.Rotate(Vector3.forward * Time.deltaTime * 10f);
	}

	void SpawnNormalBullet()
	{
		// Instantiate a normal bullet at the boss's position
		Instantiate(normalBulletPrefab, firePointN.position, firePointN.rotation);
	}

	void SpawnSpecialBullet()
	{
		// Instantiate a special bullet at the boss's position
		Instantiate(specialBulletPrefab, firePointS1.position, firePointS1.rotation);
		Instantiate(specialBulletPrefab, firePointS2.position, firePointS2.rotation);
	}
}
