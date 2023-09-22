using Player;

namespace Weapons
{
    public sealed class PrimaryWeapon : Weapon
    {
        private void OnEnable()
        {
            EventManager.Subscribe<FireKeyPressedEventArgs>(OnFireKeyPressed);
            EventManager.Subscribe<ReloadKeyPressedEventArgs>(OnReloadKeyPressed);
            EventManager.Subscribe<FireKeyUnpressedEventArgs>(OnFireKeyUnpressed);
        }

        private void OnDisable()
        {
            EventManager.Unsubscribe<FireKeyPressedEventArgs>(OnFireKeyPressed);
            EventManager.Unsubscribe<ReloadKeyPressedEventArgs>(OnReloadKeyPressed);
            EventManager.Unsubscribe<FireKeyUnpressedEventArgs>(OnFireKeyUnpressed);
        }
    }
}