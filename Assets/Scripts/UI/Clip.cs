using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Clip : MonoBehaviour
{
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] public Weapon weapon;
    [SerializeField] private Sprite bulletSprite;
    [SerializeField] private float bulletSeperation;

    [HideInInspector] private int clipSize;
    [HideInInspector] private Transform startPosition;
    [HideInInspector] private List<GameObject> bulletList;

    void Awake()
    {
        this.clipSize = weapon.clipSize;
        this.startPosition = this.transform.GetChild(0);
        this.bulletList = new List<GameObject>(); 
        this.makeClip();
    }

    public void makeClip()
    {
        float pixelLength = 1f / 16f;
        this.bulletList.Clear();
        for (int bullet = 0; bullet < this.clipSize; bullet++){
            Vector3 bulletVector = new Vector3(pixelLength * bullet * bulletSeperation, 0f, 0f);
            GameObject newBullet = Instantiate(bulletPrefab, this.startPosition.position - bulletVector, this.transform.rotation);
            newBullet.GetComponent<SpriteRenderer>().sprite = bulletSprite;
            newBullet.transform.parent = this.startPosition;
            this.bulletList.Add(newBullet);
        }    
        this.startPosition.localScale = new Vector3(0.76f,0.76f,1f);
    }

    public void UpdateClipInfo(Weapon activeWeapon)
    {
        int bulletsInClip = activeWeapon.roundsLeftInClip;
        for (int i = 0; i < bulletsInClip; i++) {
            this.bulletList[i].SetActive(true);
        }
        for (int i = bulletsInClip; i < this.clipSize; i++) {
            this.bulletList[i].SetActive(false);
        }
    }
}
