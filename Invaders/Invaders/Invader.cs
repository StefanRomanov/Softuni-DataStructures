using System;

public class Invader : IInvader
{
    public Invader(int damage, int distance)
    {
        Damage = damage;
        Distance = distance;
    }
    
    public int Damage { get; set; }
    public int Distance { get; set; }

    public int CompareTo(IInvader other)
    {
        var result = Distance.CompareTo(other.Distance);
        
        return result == 0 ? other.Damage.CompareTo(Damage) : result;
    }
}
