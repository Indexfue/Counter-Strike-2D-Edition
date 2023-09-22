using Player;

namespace Weapons
{
    public sealed class MeleeWeapon : Weapon
    {
        private void OnEnable()
        {
            EventManager.Subscribe<FireKeyPressedEventArgs>(OnFireKeyPressed);
        }

        private void OnDisable()
        {
            EventManager.Unsubscribe<FireKeyPressedEventArgs>(OnFireKeyPressed);
        }
    }
}