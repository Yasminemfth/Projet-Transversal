using UnityEngine;

[CreateAssetMenu(fileName = "NewEnemyData", menuName = "Enemy/Enemy Data")]
public class EnemyData : ScriptableObject
{
    public string enemyName;
    public float speed = 2f;
    public float detectionRadius = 1.5f;
    public Color enemyColor = Color.white;
}
