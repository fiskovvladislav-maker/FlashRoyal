using UnityEngine;

[CreateAssetMenu(fileName = "New Enemy", menuName = "Data/Enemy")]
public class EnemyDataSO : ScriptableObject
{
    public float hp = 10f;
    public float moveSpeed = 2f;
    public int soulReward = 5; 
    // Ссылаться на префаб самого врага мы будем прямо в инспекторе
}