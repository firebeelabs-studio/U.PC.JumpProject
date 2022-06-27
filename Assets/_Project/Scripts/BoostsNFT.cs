using UnityEngine;

public class BoostsNFT : MonoBehaviour
{
    [SerializeField] private bool _hasAccelerationNFT;
    [SerializeField] private bool _hasMaxSpeedNFT;
    [SerializeField] private bool _hasBetterControlNFT;
    [SerializeField] private bool _hasHigherJumpNFT;

    public float AccelerationBoost;
    public float MaxSpeedBoost;
    public float BetterControlBoost;
    public float HigherJumpBoost;

    public void SetNFTBoosts()
    {
        if (_hasAccelerationNFT)
        {
            AccelerationBoost = 10;
        }
        else
        {
            AccelerationBoost = 0;
        }
        
        if (_hasMaxSpeedNFT)
        {
            MaxSpeedBoost = 2;
        }
        else
        {
            MaxSpeedBoost = 0;
        }
        
        if (_hasBetterControlNFT) 
        { 
            BetterControlBoost = 10; 
        } 
        else 
        { 
            BetterControlBoost = 0; 
        }
        
        if (_hasHigherJumpNFT) 
        { 
            HigherJumpBoost = 2; 
        } 
        else 
        { 
            HigherJumpBoost = 0; 
        }
    }
}
