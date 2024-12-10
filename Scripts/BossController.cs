using UnityEngine;

public class BossController : MonoBehaviour
{
    public delegate void HealthThresholdHandler(int percentage);
    public event HealthThresholdHandler OnHealthThresholdReached;

    private int maxHealth;
    private int currentHealth;
    private string[] patterns;

    public void ConfigureBoss(string[] bossPatterns)
    {
        patterns = bossPatterns;
        maxHealth = 100; // Exemplo: Vida inicial do Boss
        currentHealth = maxHealth;
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        CheckHealthPercentage();
    }

    private void CheckHealthPercentage()
    {
        int percentage = (currentHealth * 100) / maxHealth;

        if (percentage == 60 || percentage == 40 || percentage == 20)
        {
            OnHealthThresholdReached?.Invoke(percentage);
        }
    }

    public void ExitScene()
    {
        // Código para saída do Boss
        Debug.Log("Boss está saindo da cena temporariamente...");
        gameObject.SetActive(false);
    }
}
