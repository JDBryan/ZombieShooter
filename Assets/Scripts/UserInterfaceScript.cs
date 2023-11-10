using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UserInterface : MonoBehaviour
{
    [SerializeField] private Player player;
    [SerializeField] private TMP_Text weaponInfoText;
    [SerializeField] private TMP_Text waveNumberText;
    [SerializeField] private TMP_Text healthBar;
    [SerializeField] private GameObject deathScreen;
    [SerializeField] private GameObject activeWeaponUI;
    [SerializeField] private GameObject secondaryWeaponUI;
    [SerializeField] private GameObject healthBarMask;
    [SerializeField] private GameObject bulletUI;
    [SerializeField] private GameObject bulletUIParent;
    private Vector3 bulletUIPosition;
    public List<GameObject> bulletUIList;

    void Start(){
        this.bulletUIPosition = this.bulletUIParent.transform.position;
        bulletUIList = MakeUIBullets();
        SetUIWeapons();
        truncateBulletsList(player.GetActiveWeapon().clipCount);
    }

    public void SetWaveNumber(int waveNumber) {
        waveNumberText.SetText(waveNumber.ToString());
    }

    public void SetHealthBar(int health) {
        if (health >= 0){
            float healthPercent = (float)health / (float)100;
            float newHealthPosition = healthPercent * 7.76f;
            healthBarMask.transform.SetLocalPositionAndRotation(new Vector3(newHealthPosition, 0f, 0f), healthBarMask.transform.rotation);
        }
    }

    private void SetUIWeaponSprite(Weapon weapon, GameObject targetUIWeapon){
        Sprite newSprite = weapon.gunUISprite;        
        targetUIWeapon.GetComponent<SpriteRenderer>().sprite = newSprite;
    }

    public void SetUIWeapons(){
        SetUIWeaponSprite(player.GetActiveWeapon(), activeWeaponUI);
        SetUIWeaponSprite(player.GetSecondaryWeapon(), secondaryWeaponUI);
    }

    private List<GameObject> MakeUIBullets(){
        float pixelSize = 1f / 16f;
        List<GameObject> bulletList = new List<GameObject>();
        for (int index = 0; index < 30; index++){
            float xDistance = (index % 15) * pixelSize * 3f;
            float yDistance = pixelSize * 3f;
            Vector3 bulletPosition = bulletUIPosition + new Vector3(xDistance, 0f, 0f);
            if (index >= 15){
                bulletPosition.y = bulletUIPosition.y - yDistance;
            }
            GameObject newBullet = Instantiate(bulletUI, bulletPosition, this.transform.rotation);
            newBullet.transform.parent = bulletUIParent.transform;
            bulletList.Add(newBullet);         
        }
        return bulletList;
    }

    public void truncateBulletsList(int newLength){
        for (int bullet = newLength; bullet < 30; bullet++){
            bulletUIList[bullet].SetActive(false);
        }
    }

    public void fillClip(int size){
        for (int bullet = 0; bullet < size; bullet++){
            bulletUIList[bullet].SetActive(true);
        }
    }

    public void SetWeaponInfoText(Weapon weapon) {
        string ammoText = weapon.hasInfiniteAmmo ? "inf" : weapon.ammoCount.ToString();
        weaponInfoText.SetText(ammoText);
    }

    public void EnableGameOverScreen() {
        Debug.Log("ENDING GAME");
        this.deathScreen.SetActive(true);
    }

    // Update is called once per frame
    void LateUpdate()
    {
        this.SetWeaponInfoText(player.GetActiveWeapon());
    }
}
