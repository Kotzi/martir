public interface IDamageable: IEnemyDetector 
{
   bool takeDamage(float damageTaken); // Returns if the object was destroyed
}