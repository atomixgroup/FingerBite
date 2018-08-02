using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IndicatorCreater : MonoBehaviour
{
    public GameObject circleTruePrefab;
    public GameObject circleFalsePrefab;
    public Animator levelAnim;
    private int count = 1;
    private int numOfTrues = 0;
    private int score = 0;
    
    void Start()
    {
        levelAnim = GetComponent<Animator>();
       SetNumberOfCirles(0);
        
    }

    public void SetNumberOfCirles(int score)
    { 
        this.score = score;
        if (score==0)
        { 
            levelAnim.SetTrigger("Start");
            return;
            
           
        }
        
      
        if (score == 1)
        {
            count = 1;
            numOfTrues = 1;
            MakeIndicators();
            levelAnim.SetTrigger("Start");
            
        }
        else if (score > 1 && score<4)
        {
            count = 3;
            numOfTrues = score - 1;
            MakeIndicators();
        }
        else if (score==4)
        {
            count = 3;
            numOfTrues = 3;
            MakeIndicators();
            levelAnim.SetTrigger("Start");
        }
        else if (score > 4)
        {
            int number = score - 4;
            while (true)
            {
                if (number < 5)
                {
                    count = 5;
                    numOfTrues = number;
                    MakeIndicators();
                    break;
                }
                if (number == 5)
                {
                    count = 5;
                    numOfTrues = 5;
                    MakeIndicators();
                    levelAnim.SetTrigger("Start");
                    break;
                }

                if (number>5)
                {
                    number -= 5;
                }
               
            }
          
        }
 
    }

    private void MakeIndicators()
    {
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }
        for (int i = 0; i < numOfTrues; i++)
        {
            GameObject circle = Instantiate(circleTruePrefab, transform.position, Quaternion.identity) as GameObject;
            circle.transform.SetParent(transform, false);
        }

        if (count > numOfTrues)
        {
            int falseCount = count - numOfTrues;
            for (int i = 0; i < falseCount; i++)
            {
                GameObject circle =
                    Instantiate(circleFalsePrefab, transform.position, Quaternion.identity) as GameObject;
                circle.transform.SetParent(transform, false);
            }
        }
    }
    public void UpdateIndicatorAfterAnimation()
    {
        MenuManager.attackingSpeed += .1f;
        if (score==0)
        {
            MenuManager.attackingSpeed =MenuManager.attackingSpeedMin;
        }
        if (MenuManager.attackingSpeed > MenuManager.attackingSpeedMax)
        {
            MenuManager.attackingSpeed = MenuManager.attackingSpeedMax;
        }
        numOfTrues = 0;
        if (score==0)
        {
            count = 1;
        }
        else if (score >= 1 && score<4)
        {
            count=3;
        }
        else if (score >= 4)
        {
            count = 5;
        }
        MakeIndicators();
        
    }
   
}