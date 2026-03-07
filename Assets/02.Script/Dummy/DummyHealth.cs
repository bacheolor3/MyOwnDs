using UnityEngine;

public class DummyHealth : MonoBehaviour
{
    public float maxHealth = 100f;
    private float currentHealth;

    void Start() {
        currentHealth = maxHealth;
    }

    public void TakeDamage(float amount) {
        currentHealth -= amount;
        if (currentHealth <= 0) {
            // 파괴 또는 초기화 로직
        }
    }
}
