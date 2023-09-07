using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerStatBar : MonoBehaviour
{
    public Image healthImage;
    public Image healthDelayImage;
    public Image powerImage;

    private void Update()
    {
        if(healthDelayImage.fillAmount>healthImage.fillAmount)
        {
            healthDelayImage.fillAmount-=Time.deltaTime;//红条大于绿条时，红条随时间降落到与铝条一致
        }
    }

    /// <summary>
    /// 接收health的变更百分比
    /// </summary>
    /// <param name="persentage">百分比：current/max</param>
    public void OnHealthChange(float persentage)
    {
        healthImage.fillAmount = persentage;
    }
}
